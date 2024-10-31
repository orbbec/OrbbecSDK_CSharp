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

        static Action<VideoFrame> UpdateImage(Image img, Format format)
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
                    data = ConvertDepthToRGBData(data);
                }
                else if(frame.GetFrameType() == FrameType.OB_FRAME_IR)
                {
                    data = ConvertIRToRGBData(data, format);
                }
                var rect = new Int32Rect(0, 0, width, height);
                wbmp.WritePixels(rect, data, stride, 0);
            });
        }

        static byte[] ConvertDepthToRGBData(byte[] depthData)
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

        static byte[] ConvertIRToRGBData(byte[] irData, Format format)
        {
            byte[] colorData = null;
            switch (format)
            {
                case Format.OB_FORMAT_Y16:
                    colorData = new byte[(irData.Length / 2) * 3];
                    for (int i = 0; i < irData.Length; i += 2)
                    {
                        ushort irValue = (ushort)(irData[i + 1] << 8 | irData[i]);
                        byte irByte = (byte)(irValue >> 8); // Scale down to 8 bits

                        int index = (i / 2) * 3;
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
            Action<VideoFrame> updateIr;

            try
            {
                Pipeline pipeline = new Pipeline();
                StreamProfileList irProfileList = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_IR);

                StreamProfile colorProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_RGB, 0);
                StreamProfile depthProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH).GetVideoStreamProfile(0, 0, Format.OB_FORMAT_Y16, 0);
                StreamProfile irProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_IR).GetVideoStreamProfile(0, 0, irProfileList.GetProfile(0).GetFormat(), 0);
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
                imgDepth.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
            depth = UpdateImage(imgDepth, depthProfile.GetFormat());

            using (var p = colorProfile.As<VideoStreamProfile>())
                imgColor.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
            color = UpdateImage(imgColor, colorProfile.GetFormat());

            using (var p = irProfile.As<VideoStreamProfile>())
                imgIr.Source = new WriteableBitmap((int)p.GetWidth(), (int)p.GetHeight(), 96d, 96d, PixelFormats.Rgb24, null);
            ir = UpdateImage(imgIr, irProfile.GetFormat());
        }
    }
}