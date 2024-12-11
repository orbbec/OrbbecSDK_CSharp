using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;
using System.Runtime.InteropServices;
using SystemDrawing = System.Drawing;

namespace Orbbec
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MultiStreamWindow : Window
    {
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        private static Action<VideoFrame> UpdateImage(Image img, Format format)
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
                else if (frame.GetFrameType() == FrameType.OB_FRAME_IR ||
                    frame.GetFrameType() == FrameType.OB_FRAME_IR_LEFT ||
                    frame.GetFrameType() == FrameType.OB_FRAME_IR_RIGHT)
                {
                    data = ConvertIRToRGBData(data, format);
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

                    // Adjust the order of BGR to RGB
                    for (int i = 0; i < rgbData.Length; i += 3)
                    {
                        // BGR -> RGB: Exchange Blue and Red
                        byte temp = rgbData[i];      // Blue
                        rgbData[i] = rgbData[i + 2]; // Red
                        rgbData[i + 2] = temp;       // Exchange Blue and Red
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

        private static byte[] ConvertIRToRGBData(byte[] irData, Format format)
        {
            byte[] colorData = null;
            switch (format)
            {
                case Format.OB_FORMAT_Y16:
                    colorData = new byte[irData.Length / 2 * 3];
                    for (int i = 0; i < irData.Length; i += 2)
                    {
                        ushort irValue = (ushort)((irData[i + 1] << 8) | irData[i]);
                        byte irByte = (byte)(irValue >> 8); // Scale down to 8 bits

                        int index = i / 2 * 3;
                        colorData[index] = irByte; // Red
                        colorData[index + 1] = irByte; // Green
                        colorData[index + 2] = irByte; // Blue
                    }
                    break;
                case Format.OB_FORMAT_Y8:
                    colorData = new byte[irData.Length * 3];
                    for (int i = 0; i < irData.Length; i++)
                    {
                        byte irByte = irData[i];

                        int index = i * 3;
                        colorData[index] = irByte;
                        colorData[index + 1] = irByte;
                        colorData[index + 2] = irByte;
                    }
                    break;
            }

            return colorData;
        }

        public MultiStreamWindow()
        {
            InitializeComponent();

            Action<VideoFrame> updateColor = null;
            Action<VideoFrame> updateDepth = null;
            Action<VideoFrame> updateIr = null;
            Action<VideoFrame> updateIrLeft = null;
            Action<VideoFrame> updateIrRight = null;

            try
            {
                //Context.SetLoggerToFile(LogSeverity.OB_LOG_SEVERITY_DEBUG, "C:\\Log\\OrbbecSDK");
                // Create a pipeline with default device.
                Pipeline pipeline = new Pipeline();
                Config config = new Config();

                Device device = pipeline.GetDevice();
                SensorList sensorList = device.GetSensorList();
                for (uint i = 0; i < sensorList.SensorCount(); i++)
                {
                    SensorType sensorType = sensorList.SensorType(i);

                    if (sensorType == SensorType.OB_SENSOR_ACCEL || sensorType == SensorType.OB_SENSOR_GYRO)
                    {
                        continue;
                    }
                    config.EnableStream(sensorType);
                }
                pipeline.Start(config);

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

                            var colorFrame = frames.GetFrame(FrameType.OB_FRAME_COLOR)?.As<VideoFrame>();
                            var depthFrame = frames.GetFrame(FrameType.OB_FRAME_DEPTH)?.As<VideoFrame>();
                            var irFrame = frames.GetFrame(FrameType.OB_FRAME_IR)?.As<VideoFrame>();
                            var irLeftFrame = frames.GetFrame(FrameType.OB_FRAME_IR_LEFT)?.As<VideoFrame>();
                            var irRightFrame = frames.GetFrame(FrameType.OB_FRAME_IR_RIGHT)?.As<VideoFrame>();

                            if (colorFrame != null)
                            {
                                updateColor = UpdateFrame(imgColor, updateColor, colorFrame);
                            }
                            if (depthFrame != null)
                            {
                                updateDepth = UpdateFrame(imgDepth, updateDepth, depthFrame);
                            }
                            if (irFrame != null)
                            {
                                updateIr = UpdateFrame(imgIr, updateIr, irFrame);
                            }
                            if (irLeftFrame != null)
                            {
                                updateIrLeft = UpdateFrame(imgIrLeft, updateIrLeft, irLeftFrame);
                            }
                            if (irRightFrame != null)
                            {
                                updateIrRight = UpdateFrame(imgIrRight, updateIrRight, irRightFrame);
                            }
                        }
                    }
                }, tokenSource.Token);
                
                Pipeline imuPipeline = new Pipeline(device);

                Config imuConfig = new Config();
                imuConfig.EnableAccelStream();
                imuConfig.EnableGyroStream();
                imuPipeline.Start(imuConfig);

                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        using (var renderImuFrameSet = imuPipeline.WaitForFrames(100))
                        {
                            if (renderImuFrameSet == null)
                            {
                                continue;
                            }

                            var accelFrame = renderImuFrameSet.GetFrame(FrameType.OB_FRAME_ACCEL)?.As<AccelFrame>();
                            var gyroFrame = renderImuFrameSet.GetFrame(FrameType.OB_FRAME_GYRO)?.As<GyroFrame>();

                            if (accelFrame != null)
                            {
                                var accelValue = accelFrame.GetAccelValue();
                                var accelTimestamp = accelFrame.GetTimeStampUs();
                                var accelTemperature = accelFrame.GetTemperature();
                                Dispatcher.Invoke(() =>
                                {
                                    tbAccel.Text = string.Format("Accel tsp:{0}\nAccelTemperature:{1}\nAccel.x:{2}\nAccel.y:{3}\nAccel.z:{4}",
                                        accelTimestamp, accelTemperature.ToString("F2"),
                                        accelValue.x, accelValue.y, accelValue.z);
                                });
                            }

                            if (gyroFrame != null)
                            {
                                var gyroValue = gyroFrame.GetGyroValue();
                                var gyroTimestamp = gyroFrame.GetTimeStampUs();
                                var gyroTemperature = gyroFrame.GetTemperature();
                                Dispatcher.Invoke(() =>
                                {
                                    tbGyro.Text = string.Format("Gyro tsp:{0}\nGyroTemperature:{1}\nGyro.x:{2}\nGyro.y:{3}\nGyro.z:{4}",
                                        gyroTimestamp, gyroTemperature.ToString("F2"),
                                        gyroValue.x, gyroValue.y, gyroValue.z);
                                });
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

        private Action<VideoFrame> UpdateFrame(Image image, Action<VideoFrame> updateAction, VideoFrame frame)
        {
            Dispatcher.Invoke(() =>
            {
                if (!(image.Source is WriteableBitmap writeableBitmap))
                {
                    if (frame.GetFrameType() == FrameType.OB_FRAME_IR)
                    {
                        irGrid.Visibility = Visibility.Visible;
                    }
                    else if (frame.GetFrameType() == FrameType.OB_FRAME_IR_LEFT)
                    {
                        irLeftGrid.Visibility = Visibility.Visible;
                    }
                    else if (frame.GetFrameType() == FrameType.OB_FRAME_IR_RIGHT)
                    {
                        irRightGrid.Visibility = Visibility.Visible;
                    }
                    image.Source = new WriteableBitmap((int)frame.GetWidth(), (int)frame.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
                    updateAction = UpdateImage(image, frame.GetFormat());
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