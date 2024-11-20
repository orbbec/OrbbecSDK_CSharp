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
        private readonly FrameCallback frameCallback;
        private Pipeline pipeline;
        private Sensor accelSensor;
        private Sensor gyroSensor;
        private DispatcherTimer timer = new DispatcherTimer();
        private Device device;
        // accel info and gyro info
        private AccelValue accelValue;
        private GyroValue gyroValue;
        private ulong accelTimestamp;
        private ulong gyroTimestamp;
        private double accelTemperature;
        private double gyroTemperature;

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
                if (frame.GetFrameType() == FrameType.OB_FRAME_DEPTH)
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

            Action<VideoFrame> updateDepth;
            Action<VideoFrame> updateColor;
            Action<VideoFrame> updateIrLeft;
            Action<VideoFrame> updateIrRight;

            try
            {
                Context.SetLoggerToFile(LogSeverity.OB_LOG_SEVERITY_DEBUG, "C:\\Log\\OrbbecSDK");
                // Create a pipeline with default device.
                pipeline = new Pipeline();

                StreamProfile colorProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_RGB, 0);
                StreamProfile depthProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_Y16, 0);
                StreamProfile irLeftProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_IR_LEFT).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_Y8, 0);
                StreamProfile irRightProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_IR_RIGHT).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_Y8, 0);
                // Configure which streams to enable or disable for the Pipeline by creating a Config.
                Config config = new Config();
                config.EnableStream(colorProfile);
                config.EnableStream(depthProfile);
                config.EnableStream(irLeftProfile);
                config.EnableStream(irRightProfile);

                pipeline.Start(config);

                SetupWindow(colorProfile, depthProfile, irLeftProfile, irRightProfile,
                    out updateColor, out updateDepth, out updateIrLeft, out updateIrRight);

                config.Dispose();

                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        using (var frames = pipeline.WaitForFrames(100))
                        {
                            if (frames == null) continue;

                            var colorFrame = frames.GetFrame(FrameType.OB_FRAME_COLOR)?.As<VideoFrame>();
                            var depthFrame = frames.GetFrame(FrameType.OB_FRAME_DEPTH)?.As<VideoFrame>();
                            var irLeftFrame = frames.GetFrame(FrameType.OB_FRAME_IR_LEFT)?.As<VideoFrame>();
                            var irRightFrame = frames.GetFrame(FrameType.OB_FRAME_IR_RIGHT)?.As<VideoFrame>();

                            if (colorFrame != null)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Render, updateColor, colorFrame);
                            }
                            if (depthFrame != null)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Render, updateDepth, depthFrame);
                            }
                            if (irLeftFrame != null)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Render, updateIrLeft, irLeftFrame);
                            }
                            if (irRightFrame != null)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Render, updateIrRight, irRightFrame);
                            }
                        }
                    }
                }, tokenSource.Token);

                // Enumerate and config all sensors.
                device = pipeline.GetDevice();

                frameCallback = OnFrame;

                accelSensor =  device.GetSensor(SensorType.OB_SENSOR_ACCEL);
                using (StreamProfileList accelProfileList = accelSensor.GetStreamProfileList())
                {
                    using (StreamProfile accelProfile = accelProfileList.GetProfile(0))
                    {
                        accelSensor.Start(accelProfile, frameCallback);
                    }
                }

                gyroSensor = device.GetSensor(SensorType.OB_SENSOR_GYRO);
                using (StreamProfileList gyroProfileList = gyroSensor.GetStreamProfileList())
                {
                    using (StreamProfile gyroProfile = gyroProfileList.GetProfile(0))
                    {
                        gyroSensor.Start(gyroProfile, frameCallback);
                    }
                }

                timer.Interval = TimeSpan.FromMilliseconds(16);
                timer.Tick += Timer_Tick;
                timer.Start();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        private void Control_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tokenSource.Cancel();
            Stop();
        }

        private void SetupWindow(StreamProfile colorProfile, StreamProfile depthProfile, StreamProfile irLeftProfile, StreamProfile irRightProfile,
                                    out Action<VideoFrame> color, out Action<VideoFrame> depth, out Action<VideoFrame> irLeft, out Action<VideoFrame> irRight)
        {
            using (var p = colorProfile.As<VideoStreamProfile>())
            {
                imgColor.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
                color = UpdateImage(imgColor, colorProfile.GetFormat());
            }

            using (var p = depthProfile.As<VideoStreamProfile>())
            {
                imgDepth.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
                depth = UpdateImage(imgDepth, depthProfile.GetFormat());
            }

            using (var p = irLeftProfile.As<VideoStreamProfile>())
            {
                imgIrLeft.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
                irLeft = UpdateImage(imgIrLeft, irLeftProfile.GetFormat());
            }

            using (var p = irRightProfile.As<VideoStreamProfile>())
            {
                imgIrRight.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
                irRight = UpdateImage(imgIrRight, irRightProfile.GetFormat());
            }
        }

        private void OnFrame(Frame frame)
        {
            if (frame.GetFrameType() == FrameType.OB_FRAME_ACCEL)
            {
                var accelFrame = frame.As<AccelFrame>();
                if (accelFrame != null)
                {
                    accelValue = accelFrame.GetAccelValue();
                    accelTimestamp = accelFrame.GetTimeStampUs();
                    accelTemperature = accelFrame.GetTemperature();
                }
            }
            if (frame.GetFrameType() == FrameType.OB_FRAME_GYRO)
            {
                var gyroFrame = frame.As<GyroFrame>();
                if (gyroFrame != null)
                {
                    gyroValue = gyroFrame.GetGyroValue();
                    gyroTimestamp = gyroFrame.GetTimeStampUs();
                    gyroTemperature = gyroFrame.GetTemperature();
                }
            }
            frame.Dispose();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tbAccel.Text = string.Format("Accel tsp:{0}\nAccelTemperature:{1}\nAccel.x:{2}\nAccel.y:{3}\nAccel.z:{4}",
                accelTimestamp, accelTemperature.ToString("F2"),
                accelValue.x, accelValue.y, accelValue.z);

            tbGyro.Text = string.Format("Gyro tsp:{0}\nGyroTemperature:{1}\nGyro.x:{2}\nGyro.y:{3}\nGyro.z:{4}",
                gyroTimestamp, gyroTemperature.ToString("F2"),
                gyroValue.x, gyroValue.y, gyroValue.z);
        }

        private void Stop()
        {

            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
                timer = null;
            }
            if (accelSensor != null)
            {
                accelSensor.Stop();
                accelSensor.Dispose();
            }
            if (gyroSensor != null)
            {
                gyroSensor.Stop();
                gyroSensor.Dispose();
            }
            if (pipeline != null)
            {
                pipeline.Stop();
                pipeline.Dispose();
            }
            if (device != null)
            {
                device.Dispose();
            }
        }
    }
}