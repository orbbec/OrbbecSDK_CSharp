using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.IO;
using SystemDrawing = System.Drawing;

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
                if (frame.GetFrameType() == FrameType.OB_FRAME_COLOR &&
                    frame.GetFormat() == Format.OB_FORMAT_MJPG)
                {
                    data = ConvertMJPGToRGB(data);
                }
                else if (frame.GetFrameType() == FrameType.OB_FRAME_DEPTH)
                {
                    data = ConvertDepthToRGBData(data);
                }
                var rect = new Int32Rect(0, 0, width, height);
                wbmp.WritePixels(rect, data, stride, 0);
            });
        }

        private static byte[] ConvertMJPGToRGB(byte[] mjpgData)
        {
            using (var ms = new MemoryStream(mjpgData))
            {
                using (var jpegImage = new SystemDrawing.Bitmap(ms))
                {
                    SystemDrawing.Rectangle rect = new SystemDrawing.Rectangle(0, 0, jpegImage.Width, jpegImage.Height);
                    SystemDrawing.Imaging.BitmapData bmpData =
                        jpegImage.LockBits(rect, SystemDrawing.Imaging.ImageLockMode.ReadOnly, SystemDrawing.Imaging.PixelFormat.Format24bppRgb);

                    IntPtr ptr = bmpData.Scan0;
                    int size = Math.Abs(bmpData.Stride) * jpegImage.Height;
                    byte[] rgbData = new byte[size];

                    Marshal.Copy(ptr, rgbData, 0, size);

                    jpegImage.UnlockBits(bmpData);

                    for (int i = 0; i < rgbData.Length; i += 3)
                    {
                        byte temp = rgbData[i];
                        rgbData[i] = rgbData[i + 2];
                        rgbData[i + 2] = temp;
                    }

                    return rgbData;
                }
            }
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
                //Context.SetLoggerToFile(LogSeverity.OB_LOG_SEVERITY_DEBUG, "C:\\Log\\OrbbecSDK");
                pipeline = new Pipeline();
                pipeline.EnableFrameSync();

                config = CreateHwD2CAlignConfig(pipeline);
                if (config == null)
                {
                    MessageBox.Show("Current device does not support hardware depth-to-color alignment.", "´íÎó", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                    return;
                }

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
                }, tokenSource.Token);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        private bool CheckIfSupportHWD2CAlign(Pipeline pipeline, StreamProfile colorStreamProfile, VideoStreamProfile depthVsp)
        {
            StreamProfileList hwD2CSupportedDepthStreamProfiles = pipeline.GetD2CDepthProfileList(colorStreamProfile, AlignMode.ALIGN_D2C_HW_MODE);
            if (hwD2CSupportedDepthStreamProfiles.ProfileCount() == 0)
                return false;

            // Iterate through the supported depth stream profiles and check if there is a match with the given depth stream profile
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

        private Config CreateHwD2CAlignConfig(Pipeline pipe)
        {
            var coloStreamProfiles = pipe.GetStreamProfileList(SensorType.OB_SENSOR_COLOR);
            var depthStreamProfiles = pipe.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH);

            // Iterate through all color and depth stream profiles to find a match for hardware depth-to-color alignment
            uint colorSpCount = coloStreamProfiles.ProfileCount();
            uint depthSpCount = depthStreamProfiles.ProfileCount();
            for (int i = 0; i < colorSpCount; i++)
            {
                var colorProfile = coloStreamProfiles.GetProfile(i);
                var colorVsp = colorProfile.As<VideoStreamProfile>();

                for (int j = 0; j < depthSpCount; j++)
                {
                    var depthProfile = depthStreamProfiles.GetProfile(j);
                    var depthVsp = depthProfile.As<VideoStreamProfile>();

                    // make sure the color and depth stream have the same fps, due to some models may not support different fps
                    if (colorVsp.GetFPS() != depthVsp.GetFPS())
                    {
                        continue;
                    }

                    // Check if the given stream profiles support hardware depth-to-color alignment
                    if (CheckIfSupportHWD2CAlign(pipe, colorProfile, depthVsp))
                    {
                        // If support, create a config for hardware depth-to-color alignment
                        Config hwD2CAlignConfig = new Config();
                        hwD2CAlignConfig.EnableStream(colorProfile);                                                     // enable color stream
                        hwD2CAlignConfig.EnableStream(depthProfile);                                                     // enable depth stream
                        hwD2CAlignConfig.SetAlignMode(AlignMode.ALIGN_D2C_HW_MODE);                                      // enable hardware depth-to-color alignment
                        hwD2CAlignConfig.SetFrameAggregateOutputMode(FrameAggregateOutputMode.OB_FRAME_AGGREGATE_OUTPUT_ALL_TYPE_FRAME_REQUIRE);  // output frameset with all types of frames
                        return hwD2CAlignConfig;
                    }
                }
            }
            return null;
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

        private void Control_Closing(object sender, CancelEventArgs e)
        {
            tokenSource.Cancel();
        }
    }
}