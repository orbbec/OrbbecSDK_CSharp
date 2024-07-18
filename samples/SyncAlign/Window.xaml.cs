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
    public partial class SyncAlignWindow : Window
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
                byte[] data = new byte[frame.GetDataSize()];
                frame.CopyData(ref data);
                if(frame.GetFrameType() == FrameType.OB_FRAME_DEPTH)
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

        public SyncAlignWindow()
        {
            InitializeComponent();

            Action<VideoFrame> updateDepth;
            Action<VideoFrame> updateColor;

            try
            {
                Pipeline pipeline = new Pipeline();
                StreamProfile colorProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_RGB, 0);
                StreamProfile depthProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_Y16, 0);
                Config config = new Config();
                config.EnableStream(colorProfile);
                config.EnableStream(depthProfile);
                config.SetAlignMode(AlignMode.ALIGN_D2C_SW_MODE);

                pipeline.EnableFrameSync();
                pipeline.Start(config);

                SetupWindow(colorProfile, depthProfile, out updateDepth, out updateColor);

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

        private void SetupWindow(StreamProfile colorProfile, StreamProfile depthProfile,
                                    out Action<VideoFrame> depth, out Action<VideoFrame> color)
        {
            using (var p = colorProfile.As<VideoStreamProfile>())
                imgDepth.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
            depth = UpdateImage(imgDepth);

            using (var p = colorProfile.As<VideoStreamProfile>())
                imgColor.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
            color = UpdateImage(imgColor);
        }
    }
}