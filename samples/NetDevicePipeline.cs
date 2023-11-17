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
        Context ctx = new Context();
        Device dev = ctx.CreateNetDevice("192.168.1.10", 8090);
        Pipeline pipeline = new Pipeline(dev);
        StreamProfileList colorProfileList = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR);
        StreamProfile colorProfile = colorProfileList.GetVideoStreamProfile(1920, 0, Format.OB_FORMAT_MJPG, 15);
        StreamProfileList depthProfileList = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH);
        StreamProfile depthProfile = depthProfileList.GetVideoStreamProfile(1024, 0, Format.OB_FORMAT_Y16, 15);
        Config config = new Config();
        config.EnableStream(colorProfile);
        config.EnableStream(depthProfile);

        config.SetAlignMode(AlignMode.ALIGN_D2C_HW_MODE);
        // pipeline.EnableFrameSync();

        byte[] colorData = null;
        byte[] depthData = null;
        byte[] pointsData = null;
        CameraParam cameraParam;
        PointCloudFilter pointCloudFilter = new PointCloudFilter();

        bool saved = false;

        pipeline.Start(config, (frameset) =>{
            if(frameset == null)
            {
                Console.WriteLine("empty frameset");
                return;
            }

            ColorFrame colorFrame = frameset.GetColorFrame();
            if(colorFrame != null)
            {
                if(colorData == null)
                {
                    colorData = new byte[colorFrame.GetDataSize()];
                }
                Console.WriteLine("Color {0} x {1} {2}", colorFrame.GetWidth(), colorFrame.GetHeight(), colorFrame.GetDataSize());
                colorFrame.CopyData(ref colorData);
                Console.WriteLine("Color {0}-{1}", colorData[0], colorData[colorData.Length - 1]);
                colorFrame.Dispose();
                System.IO.File.WriteAllBytes("color.mjpg", colorData);
            }


            DepthFrame depthFrame = frameset.GetDepthFrame();
            if(depthFrame != null)
            {
                if(depthData == null)
                {
                    depthData = new byte[depthFrame.GetDataSize()];
                }
                Console.WriteLine("Depth {0} x {1} {2}", depthFrame.GetWidth(), depthFrame.GetHeight(), depthFrame.GetDataSize());
                depthFrame.CopyData(ref depthData);
                Console.WriteLine("Depth {0}-{1}", depthData[0], depthData[depthData.Length - 1]);
                depthFrame.Dispose();
                System.IO.File.WriteAllBytes("depth.raw", depthData);
            }

            if (depthFrame != null)
            {
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
                    if (!saved)
                    {
                        WritePointPly(pointsData);
                        saved = true;
                    }
                }
            }

            frameset.Dispose();
        });

        cameraParam = pipeline.GetCameraParam();
        pointCloudFilter.SetCameraParam(cameraParam);
        pointCloudFilter.SetPointFormat(Format.OB_FORMAT_POINT);

        while (true)
        {
            Thread.Sleep(100);
        }
    }
}