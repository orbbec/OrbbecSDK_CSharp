using System;
using System.ComponentModel;
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
    public partial class InfraredWindow : Window
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
                data = ConvertIRToRGBData(data, format);
                var rect = new Int32Rect(0, 0, width, height);
                wbmp.WritePixels(rect, data, stride, 0);
            });
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

        public InfraredWindow()
        {
            InitializeComponent();

            Action<VideoFrame> updateIr = null;
            Action<VideoFrame> updateIrLeft = null;
            Action<VideoFrame> updateIrRight = null;

            try
            {
                Pipeline pipeline = new Pipeline();
                Device device = pipeline.GetDevice();
                SensorList sensorList = device.GetSensorList();
                Config config = new Config();
                for (uint i = 0, N = sensorList.SensorCount(); i < N; i++)
                {
                    SensorType sensorType = sensorList.SensorType(i);
                    if (sensorType == SensorType.OB_SENSOR_IR ||
                        sensorType == SensorType.OB_SENSOR_IR_LEFT ||
                        sensorType == SensorType.OB_SENSOR_IR_RIGHT)
                    {
                        config.EnableVideoStream(sensorType, 0, 0, 30, Format.OB_FORMAT_Y8);
                    }
                }

                pipeline.Start(config);

                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        using (var frames = pipeline.WaitForFrames(100))
                        {
                            var irFrame = frames?.GetIRFrame();
                            var irLeftFrame = frames?.GetFrame(FrameType.OB_FRAME_IR_LEFT)?.As<VideoFrame>();
                            var irRightFrame = frames?.GetFrame(FrameType.OB_FRAME_IR_RIGHT)?.As<VideoFrame>();

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
                if (!(image.Source is WriteableBitmap) && updateAction == null)
                {
                    image.Visibility = Visibility.Visible;
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