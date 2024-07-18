using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Orbbec
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MultiStreamWindow : Window
    {
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        static Action<VideoFrame> UpdateImage(Image img)
        {
            var wbmp = img.Source as WriteableBitmap;
            return new Action<VideoFrame>(frame =>
            {
                int width = (int)frame.GetWidth();
                int height = (int)frame.GetHeight();
                int stride = wbmp.BackBufferStride;
                int dataSize = (int)frame.GetDataSize();
                var rect = new Int32Rect(0, 0, width, height);
                wbmp.WritePixels(rect, frame.GetDataPtr(), dataSize, stride);
            });
        }

        public MultiStreamWindow()
        {
            InitializeComponent();

            Action<VideoFrame> updateDepth;
            Action<VideoFrame> updateColor;
            Action<VideoFrame> updateIr;

            try
            {
                Pipeline pipeline = new Pipeline();
                StreamProfile colorProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_RGB, 0);
                StreamProfile depthProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_Y16, 0);
                StreamProfile irProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_IR).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_Y16, 0);
                Config config = new Config();
                config.EnableStream(colorProfile);
                config.EnableStream(depthProfile);
                config.EnableStream(irProfile);

                pipeline.Start(config);

                SetupWindow(colorProfile, depthProfile, irProfile, out updateDepth, out updateColor, out updateIr);

                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        using (var frames = pipeline.WaitForFrames(100))
                        {
                            var colorFrame = frames?.GetColorFrame();
                            var depthFrame = frames?.GetDepthFrame();
                            var irFrame = frames?.GetIRFrame();

                            if (colorFrame != null)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Render, updateColor, colorFrame);
                            }
                            if (depthFrame != null)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Render, updateDepth, depthFrame);
                            }
                            if (irFrame != null)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Render, updateIr, irFrame);
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

        private void SetupWindow(StreamProfile colorProfile, StreamProfile depthProfile, StreamProfile irProfile, 
                                    out Action<VideoFrame> depth, out Action<VideoFrame> color, out Action<VideoFrame> ir)
        {
            using (var p = depthProfile.As<VideoStreamProfile>())
                imgDepth.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Gray16, null);
            depth = UpdateImage(imgDepth);

            using (var p = colorProfile.As<VideoStreamProfile>())
                imgColor.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
            color = UpdateImage(imgColor);

            using (var p = irProfile.As<VideoStreamProfile>())
                imgIr.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Gray16, null);
            ir = UpdateImage(imgIr);
        }
    }
}