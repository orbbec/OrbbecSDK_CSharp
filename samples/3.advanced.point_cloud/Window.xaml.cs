using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;

namespace Orbbec
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class PointCloudWindow : Window
    {
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private PointCloudFilter pointCloud;

        private string dirPath;

        private bool save = false;

        public PointCloudWindow()
        {
            InitializeComponent();

            dirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dirPath += "Low";
            dirPath = Path.Combine(dirPath, "Orbbec");

            try
            {
                Config config = new Config();
                config.EnableVideoStream(StreamType.OB_STREAM_DEPTH, 0, 0, 0, Format.OB_FORMAT_Y16);
                config.EnableVideoStream(StreamType.OB_STREAM_COLOR, 0, 0, 0, Format.OB_FORMAT_RGB);
                config.SetFrameAggregateOutputMode(FrameAggregateOutputMode.OB_FRAME_AGGREGATE_OUTPUT_ALL_TYPE_FRAME_REQUIRE);

                Pipeline pipeline = new Pipeline();
                pipeline.EnableFrameSync();
                pipeline.Start(config);

                pointCloud = new PointCloudFilter();
                AlignFilter align = new AlignFilter(StreamType.OB_STREAM_COLOR);

                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        using (var frames = pipeline.WaitForFrames(100))
                        {
                            if (frames == null)
                            {
                                continue;
                            }

                            if (save)
                            {
                                Frame alignedFrameset = align.Process(frames);
                                Frame frame = pointCloud.Process(alignedFrameset);

                                if (frame != null)
                                {
                                    PointsFrame pointsFrame = frame.As<PointsFrame>();
                                    if (pointsFrame.GetFormat() == Format.OB_FORMAT_POINT)
                                    {
                                        SavePointsToPly(pointsFrame, "DepthPoints.ply");
                                    }
                                    else if (pointsFrame.GetFormat() == Format.OB_FORMAT_RGB_POINT)
                                    {
                                        SaveRGBPointsToPly(pointsFrame, "RGBPoints.ply");
                                    }
                                }

                                save = false;
                            }
                        }
                    }
                }, tokenSource.Token);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        private void SavePointsToPly(PointsFrame frame, string fileName)
        {
            if (dirPath != null && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string pointcloudPath = Path.Combine(dirPath, $"{timestamp}_{fileName}");

            Dispatcher.Invoke(() =>
            {
                tipsText.Text = string.Empty;
                tipsText.Text = pointcloudPath;
            });

            byte[] data = new byte[frame.GetDataSize()];
            frame.CopyData(ref data);

            int pointSize = Marshal.SizeOf(typeof(Point));
            int pointsSize = data.Length / Marshal.SizeOf(typeof(Point));

            Point[] points = new Point[pointsSize];

            IntPtr dataPtr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, dataPtr, data.Length);
            for (int i = 0; i < pointsSize; i++)
            {
                IntPtr pointPtr = new IntPtr(dataPtr.ToInt64() + i * pointSize);
                points[i] = Marshal.PtrToStructure<Point>(pointPtr);
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

        private void SaveRGBPointsToPly(PointsFrame frame, string fileName)
        {
            if (dirPath != null && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string colorPointcloudPath = Path.Combine(dirPath, $"{timestamp}_{fileName}");

            Dispatcher.Invoke(() =>
            {
                tipsText.Text = string.Empty;
                tipsText.Text = colorPointcloudPath;
            });

            byte[] data = new byte[frame.GetDataSize()];
            frame.CopyData(ref data);

            int pointSize = Marshal.SizeOf(typeof(ColorPoint));
            int pointsSize = data.Length / Marshal.SizeOf(typeof(ColorPoint));

            ColorPoint[] points = new ColorPoint[pointsSize];

            IntPtr dataPtr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, dataPtr, data.Length);
            for (int i = 0; i < pointsSize; i++)
            {
                IntPtr pointPtr = new IntPtr(dataPtr.ToInt64() + i * pointSize);
                points[i] = Marshal.PtrToStructure<ColorPoint>(pointPtr);
            }
            Marshal.FreeHGlobal(dataPtr);

            FileStream fs = new FileStream(colorPointcloudPath, FileMode.Create);
            var writer = new StreamWriter(fs);
            writer.Write("ply\n");
            writer.Write("format ascii 1.0\n");
            writer.Write("element vertex " + pointsSize + "\n");
            writer.Write("property float x\n");
            writer.Write("property float y\n");
            writer.Write("property float z\n");
            writer.Write("property uchar red\n");
            writer.Write("property uchar green\n");
            writer.Write("property uchar blue\n");
            writer.Write("end_header\n");

            for (int i = 0; i < points.Length; i++)
            {
                writer.Write(points[i].x);
                writer.Write(" ");
                writer.Write(points[i].y);
                writer.Write(" ");
                writer.Write(points[i].z);
                writer.Write(" ");
                writer.Write(points[i].r);
                writer.Write(" ");
                writer.Write(points[i].g);
                writer.Write(" ");
                writer.Write(points[i].b);
                writer.Write("\n");
            }

            writer.Close();
            fs.Close();
        }

        private void PointcloudButton_Click(object sender, RoutedEventArgs e)
        {
            pointCloud.SetCreatePointFormat(Format.OB_FORMAT_POINT);
            save = true;
        }

        private void ColorPointcloudButton_Click(object sender, RoutedEventArgs e)
        {
            pointCloud.SetCreatePointFormat(Format.OB_FORMAT_RGB_POINT);
            save = true;
        }

        private void Control_Closing(object sender, CancelEventArgs e)
        {
            tokenSource.Cancel();
            if (pointCloud != null)
            {
                pointCloud.Dispose();
            }
        }
    }
}