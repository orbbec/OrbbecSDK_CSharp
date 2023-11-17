using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Orbbec;

class TestClass
{
    static string pointcloudPath = "points.ply";

    public static void WritePointPly(byte[] data)
    {
        int pointSize = Marshal.SizeOf(typeof(Point));
        int pointsSize = data.Length / Marshal.SizeOf(typeof(Point));

        Point[] points = new Point[pointsSize];

        IntPtr dataPtr = Marshal.AllocHGlobal(data.Length);
        Marshal.Copy(data, 0, dataPtr, data.Length);
        for (int i = 0; i < pointsSize; i++)
        {
            IntPtr pointPtr = new IntPtr(dataPtr.ToInt64() + i * pointSize);
            points[i] = (Point)Marshal.PtrToStructure(pointPtr, typeof(Point));
        }
        Marshal.FreeHGlobal(dataPtr);

        FileStream fs = new FileStream(pointcloudPath, FileMode.Create);
        var writer = new StreamWriter(fs);
        writer.Write("ply\n");
        writer.Write("format ascii 1.0\n");
        writer.Write("element vertex " + pointsSize + "\n");
        writer.Write("property float x\n");
        writer.Write("property float y\n");
        writer.Write("property float z\n");
        writer.Write("end_header\n");

        for (int i = 0; i < points.Length; i++)
        {
            writer.Write(points[i].x);
            writer.Write(" ");
            writer.Write(points[i].y);
            writer.Write(" ");
            writer.Write(points[i].z);
            writer.Write("\n");
        }

        writer.Close();
        fs.Close();
    }

    static void Main(string[] args)
    {
        Pipeline pipeline = new Pipeline();
        StreamProfile colorProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR).GetProfile(0);
        StreamProfile depthProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH).GetProfile(0);
        Config config = new Config();
        config.EnableStream(colorProfile);
        config.EnableStream(depthProfile);
        config.SetAlignMode(AlignMode.ALIGN_D2C_HW_MODE);
        pipeline.Start(config);
        byte[] data = null;

        PointCloudFilter pointCloudFilter = new PointCloudFilter();
        CameraParam cameraParam = pipeline.GetCameraParam();
        pointCloudFilter.SetCameraParam(cameraParam);
        pointCloudFilter.SetPointFormat(Format.OB_FORMAT_POINT);
        byte[] pointsData = null;

        bool saved = false;
        int frameCount = 0;

        while(true)
        {
            Frameset frameset = pipeline.WaitForFrames(1000);
            if(frameset == null)
            {
                continue;
            }
            ColorFrame colorFrame = frameset.GetColorFrame();
            DepthFrame depthFrame = frameset.GetDepthFrame();

            if(colorFrame != null)
            {
                Console.WriteLine("Color {0} x {1} {2}", colorFrame.GetWidth(), colorFrame.GetHeight(), colorFrame.GetDataSize());
                if (data == null)
                {
                    data = new byte[colorFrame.GetDataSize()];
                }
                colorFrame.CopyData(ref data);
                colorFrame.Dispose();
                Console.WriteLine("Color {0}-{1}", data[0], data[data.Length - 1]);
                data = null;
            }

            if(depthFrame != null)
            {
                // Console.WriteLine("Depth {0} x {1} {2}", depthFrame.GetWidth(), depthFrame.GetHeight(), depthFrame.GetDataSize());
                // if (data == null)
                // {
                //     data = new byte[depthFrame.GetDataSize()];
                // }
                // depthFrame.CopyData(ref data);
                // depthFrame.Dispose();
                // Console.WriteLine("Depth {0}-{1}-{2}", data[0], data[data.Length - 1], data[data.Length / 2]);
                // data = null;
                var frame = pointCloudFilter.Process(frameset);
                if (frame != null)
                {
                    var pointFrame = frame.As<PointsFrame>();
                    var dataSize = pointFrame.GetDataSize();
                    if (pointsData == null || pointsData.Length != dataSize)
                    {
                        pointsData = new byte[dataSize];
                    }
                    pointFrame.CopyData(ref pointsData);
                    pointFrame.Dispose();
                    frame.Dispose();

                    //点云保存非常耗时，最好放在一个单独线程执行
                    if (!saved && (++frameCount)==10)
                    {
                        WritePointPly(pointsData);
                        saved = true;
                    }
                }
            }

            Thread.Sleep(100);
        }
    }
}