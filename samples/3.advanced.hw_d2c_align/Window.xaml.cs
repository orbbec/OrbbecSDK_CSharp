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
    public partial class HWD2CAlignWindow : Window
    {
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private bool enableAlignMode = true;
        private Pipeline pipeline;
        private Config config;

        private static Action<VideoFrame> UpdateImage(Image img)
        {
            var wbmp = img.Source as WriteableBitmap;
            return new Action<VideoFrame>(frame =>
            {
                int width = (int)frame.GetWidth();
                int height = (int)frame.GetHeight();
                int stride = wbmp.BackBufferStride;
                byte[] data = new byte[frame.GetDataSize()];
                frame.CopyData(ref data);
                if (frame.GetFrameType() == FrameType.OB_FRAME_DEPTH)
                {
                    data = ConvertDepthToRGBData(data);
                }
                var rect = new Int32Rect(0, 0, width, height);
                wbmp.WritePixels(rect, data, stride, 0);
            });
        }

        private static byte[] ConvertDepthToRGBData(byte[] depthData)
        {
            byte[] colorData = new byte[depthData.Length / 2 * 3];
            for (int i = 0; i < depthData.Length; i += 2)
            {
                ushort depthValue = (ushort)((depthData[i + 1] << 8) | depthData[i]);
                float depth = (float)depthValue / 1000;
                byte depthByte = (byte)(depth * 255);
                int index = i / 2 * 3;
                colorData[index] = depthByte; // Red
                colorData[index + 1] = depthByte; // Green
                colorData[index + 2] = depthByte; // Blue
            }
            return colorData;
        }

        public HWD2CAlignWindow()
        {
            InitializeComponent();

            Action<VideoFrame> updateColor = null;
            Action<VideoFrame> updateDepth = null;

            try
            {
                pipeline = new Pipeline();
                pipeline.EnableFrameSync();

                StreamProfile colorProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR).GetVideoStreamProfile(640, 480, Format.OB_FORMAT_RGB, 0);
                StreamProfile depthProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_Y16, 0);
                if (!CheckIfSupportHWD2CAlign(pipeline, colorProfile, depthProfile))
                {
                    MessageBox.Show("Current device does not support hardware depth-to-color alignment.", "´íÎó", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                config = new Config();
                config.SetAlignMode(AlignMode.ALIGN_D2C_HW_MODE);
                config.SetFrameAggregateOutputMode(FrameAggregateOutputMode.OB_FRAME_AGGREGATE_OUTPUT_ALL_TYPE_FRAME_REQUIRE);
                config.EnableStream(colorProfile);
                config.EnableStream(depthProfile);

                pipeline.Start(config);

                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        using (var frames = pipeline.WaitForFrames(100))
                        {
                            var colorFrame = frames?.GetColorFrame();
                            var depthFrame = frames?.GetDepthFrame();

                            if (colorFrame != null)
                            {
                                //Dispatcher.Invoke(DispatcherPriority.Render, updateColor, colorFrame);
                                updateColor = UpdateFrame(imgColor, updateColor, colorFrame);
                            }
                            if (depthFrame != null)
                            {
                                //Dispatcher.Invoke(DispatcherPriority.Render, updateDepth, depthFrame);
                                updateDepth = UpdateFrame(imgDepth, updateDepth, depthFrame);
                            }
                        }
                    }
                }, tokenSource.Token).ContinueWith(t =>
                {
                    config.Dispose();
                    pipeline.Stop();
                    pipeline.Dispose();
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        private bool CheckIfSupportHWD2CAlign(Pipeline pipeline, StreamProfile colorStreamProfile, StreamProfile depthStreamProfile)
        {
            StreamProfileList hwD2CSupportedDepthStreamProfiles = pipeline.GetD2CDepthProfileList(colorStreamProfile, AlignMode.ALIGN_D2C_HW_MODE);
            if (hwD2CSupportedDepthStreamProfiles.ProfileCount() == 0)
                return false;

            // Iterate through the supported depth stream profiles and check if there is a match with the given depth stream profile
            VideoStreamProfile depthVsp = depthStreamProfile.As<VideoStreamProfile>();
            int count = (int)hwD2CSupportedDepthStreamProfiles.ProfileCount();
            for (int i = 0; i < count; i++)
            {
                StreamProfile sp = hwD2CSupportedDepthStreamProfiles.GetProfile(i);
                VideoStreamProfile vsp = sp.As<VideoStreamProfile>();
                if (vsp.GetWidth() == depthVsp.GetWidth() && vsp.GetHeight() == depthVsp.GetHeight() && vsp.GetFormat() == depthVsp.GetFormat()
                   && vsp.GetFPS() == depthVsp.GetFPS())
                {
                    // Found a matching depth stream profile, it is means the given stream profiles support hardware depth-to-color alignment
                    return true;
                }
            }
            return false;
        }

        private void ToggleAlign_Click(object sender, RoutedEventArgs e)
        {
            enableAlignMode = !enableAlignMode;

            if (enableAlignMode)
            {
                config.SetAlignMode(AlignMode.ALIGN_D2C_HW_MODE);
            }
            else
            {
                config.SetAlignMode(AlignMode.ALIGN_DISABLE);
            }

            pipeline.Stop();
            pipeline.Start(config);
        }

        private Action<VideoFrame> UpdateFrame(Image image, Action<VideoFrame> updateAction, VideoFrame frame)
        {
            Dispatcher.Invoke(() =>
            {
                if (!(image.Source is WriteableBitmap writeableBitmap) ||
                    writeableBitmap.PixelWidth != (int)frame.GetWidth() || writeableBitmap.PixelHeight != (int)frame.GetHeight())
                {
                    image.Visibility = Visibility.Visible;
                    image.Source = new WriteableBitmap((int)frame.GetWidth(), (int)frame.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
                    updateAction = UpdateImage(image);
                }
                updateAction?.Invoke(frame);
            }, DispatcherPriority.Render);
            return updateAction;
        }

        private void Control_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tokenSource.Cancel();
        }
    }
}