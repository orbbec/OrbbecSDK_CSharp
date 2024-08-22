using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Orbbec;

namespace PostProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        static Action<VideoFrame> UpdateImage(Image img)
        {
            var wbmp = img.Source as WriteableBitmap;
            return new Action<VideoFrame>(frame =>
            {
                int width = (int)frame.GetWidth();
                int height = (int)frame.GetHeight();

                // Allow for resizing image if decimation filter is enabled
                if (wbmp == null || wbmp.Width != width || wbmp.Height != height)
                {
                    wbmp = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);

                    img.Source = wbmp;
                }

                int stride = wbmp.BackBufferStride;
                int dataSize = (int)frame.GetDataSize();
                byte[] data = new byte[frame.GetDataSize()];
                frame.CopyData(ref data);
                if (frame.GetFrameType() == FrameType.OB_FRAME_DEPTH)
                {
                    data = ConvertDepthToRGB(data);
                }
                var rect = new Int32Rect(0, 0, width, height);
                wbmp.WritePixels(rect, data, stride, 0);
            });
        }

        static byte[] ConvertDepthToRGB(byte[] depthData)
        {
            byte[] colorData = new byte[(depthData.Length / 2) * 3];
            for (int i = 0; i < depthData.Length; i += 2)
            {
                ushort depthValue = (ushort)(depthData[i + 1] << 8 | depthData[i]);
                float depth = (float)depthValue / 1000;
                byte depthByte = (byte)(depth * 255);
                int index = (i / 2) * 3;
                colorData[index] = depthByte; // Red
                colorData[index + 1] = depthByte; // Green
                colorData[index + 2] = depthByte; // Blue
            }
            return colorData;
        }
        public MainWindow()
        {
            InitializeComponent();

            Action<VideoFrame> updateDepth;

            try
            {
                Pipeline pipeline = new Pipeline();
                StreamProfile depthProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_Y16, 0);
                Config config = new Config();
                config.EnableStream(depthProfile);

                pipeline.Start(config);

                Sensor depthSensor = pipeline.GetDevice().GetSensor(SensorType.OB_SENSOR_DEPTH);
                FilterList filterList = depthSensor.GetRecommendedFilters();
                Console.WriteLine($"{filterList.Count()} filters recommended.");

                //for (uint i = 1; i < filterList.Count(); i++)
                //{
                //    var filter = filterList.GetFilter(i);
                //    Console.WriteLine($"Filter {i}: {filter.GetName()}");
                //}

                SetupWindow(depthProfile, out updateDepth);

                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        using (var frames = pipeline.WaitForFrames(100))
                        {
                            var depthFrame = frames?.GetDepthFrame();
                            
                            if (depthFrame != null)
                            {
                                for (uint i = 1; i < filterList.Count(); i++)
                                {
                                    var filter = filterList.GetFilter(i);
                                    if(filter.Enabled)
                                    {
                                        depthFrame = filter.Process(depthFrame).As<DepthFrame>();
                                    }
                                }
                                Dispatcher.Invoke(DispatcherPriority.Render, updateDepth, depthFrame);
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

        private void control_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tokenSource.Cancel();
        }

        private void SetupWindow(StreamProfile depthProfile, out Action<VideoFrame> depth)
        {
            using (var p = depthProfile.As<VideoStreamProfile>())
            {
                imgDepth.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
                depth = UpdateImage(imgDepth);
            }
        }
    }
}
