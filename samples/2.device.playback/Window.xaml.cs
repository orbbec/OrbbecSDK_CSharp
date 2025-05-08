using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Common;

namespace Orbbec
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class PlaybackWindow : Window
    {
        private PlaybackDevice playback;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private Dictionary<string, Action<VideoFrame>> imageUpdateActions = new Dictionary<string, Action<VideoFrame>>();
        private volatile bool exited = false;
        private volatile bool isPaused = false;
        private string pattern = @"x([\d.]+)";

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
                    data = ImageConverter.ConvertMJPGToRGBData(data);
                }
                else if (frame.GetFrameType() == FrameType.OB_FRAME_DEPTH)
                {
                    data = ImageConverter.ConvertDepthToRGBData(data);
                }
                else if (frame.GetFrameType() == FrameType.OB_FRAME_IR ||
                    frame.GetFrameType() == FrameType.OB_FRAME_IR_LEFT ||
                    frame.GetFrameType() == FrameType.OB_FRAME_IR_RIGHT)
                {
                    data = ImageConverter.ConvertIRToRGBData(data, format);
                }
                var rect = new Int32Rect(0, 0, width, height);
                wbmp.WritePixels(rect, data, stride, 0);
            });
        }

        public PlaybackWindow()
        {
            InitializeComponent();
            pauseResumeButton.Click += (s, e) => OnPauseResume();

            try
            {
                string filePath;
                // Get valid .bag file path from user input
                if (GetRosbagPath(out filePath))
                {
                    // Create a playback device with a Rosbag file
                    playback = new PlaybackDevice(filePath);
                    // Create a pipeline with the playback device
                    Pipeline pipe = new Pipeline(playback);
                    // Enable all recording streams from the playback device
                    Config config = new Config();

                    // Set playback status change callback, when the playback stops, start the pipeline again with the same config
                    playback.SetPlaybackStatusChangeCallback(status =>
                    {
                        if (status == PlaybackStatus.OB_PLAYBACK_STOPPED && !exited)
                        {
                            pipe.Stop();
                            Thread.Sleep(1000);
                            pipe.Start(config);
                        }
                    });

                    SensorList sensorList = playback.GetSensorList();
                    for (uint i = 0; i < sensorList.SensorCount(); i++)
                    {
                        SensorType sensorType = sensorList.SensorType(i);
                        config.EnableStream(sensorType);
                    }

                    // Start the pipeline with the config
                    pipe.Start(config);

                    Task.Factory.StartNew(() =>
                    {
                        while (!tokenSource.Token.IsCancellationRequested)
                        {
                            using (var frames = pipe.WaitForFrames(1000))
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
                                var accelFrame = frames.GetFrame(FrameType.OB_FRAME_ACCEL)?.As<AccelFrame>();
                                var gyroFrame = frames.GetFrame(FrameType.OB_FRAME_GYRO)?.As<GyroFrame>();

                                if (colorFrame != null)
                                {
                                    UpdateFrame("color", imgColor, colorFrame);
                                }
                                if (depthFrame != null)
                                {
                                    UpdateFrame("depth", imgDepth, depthFrame);
                                }
                                if (irFrame != null)
                                {
                                    UpdateFrame("ir", imgIr, irFrame);
                                }
                                if (irLeftFrame != null)
                                {
                                    UpdateFrame("irLeft", imgIrLeft, irLeftFrame);
                                }
                                if (irRightFrame != null)
                                {
                                    UpdateFrame("irRight", imgIrRight, irRightFrame);
                                }
                                if (accelFrame != null)
                                {
                                    var accelValue = accelFrame.GetAccelValue();
                                    var accelTimestamp = accelFrame.GetTimeStampUs();
                                    var accelTemperature = accelFrame.GetTemperature();
                                    Dispatcher.InvokeAsync(() =>
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
                                    Dispatcher.InvokeAsync(() =>
                                    {
                                        tbGyro.Text = string.Format("Gyro tsp:{0}\nGyroTemperature:{1}\nGyro.x:{2}\nGyro.y:{3}\nGyro.z:{4}",
                                            gyroTimestamp, gyroTemperature.ToString("F2"),
                                            gyroValue.x, gyroValue.y, gyroValue.z);
                                    });
                                }
                            }
                        }
                    }, tokenSource.Token).ContinueWith(t =>
                    {
                        exited = true;
                        pipe.Stop();
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        public bool GetRosbagPath(out string rosbagPath)
        {
            while (true)
            {
                Console.WriteLine("Please input the path of the Rosbag file (.bag) to playback:");
                Console.Write("Path: ");
                string input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(input) &&
                    ((input.StartsWith("\"") && input.EndsWith("\"")) ||
                    (input.StartsWith("'") && input.EndsWith("'"))))
                {
                    input = input.Substring(1, input.Length - 2);
                }

                if (!string.IsNullOrEmpty(input) && input.EndsWith(".bag", StringComparison.OrdinalIgnoreCase))
                {
                    rosbagPath = input;
                    Console.WriteLine($"Playback file confirmed: {rosbagPath}\n");
                    return true;
                }

                Console.WriteLine("Invalid file format. Please provide a .bag file.\n");
            }
        }

        private void UpdateFrame(string type, Image image, VideoFrame frame)
        {
            Dispatcher.InvokeAsync(() =>
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

                    imageUpdateActions[type] = UpdateImage(image, frame.GetFormat());
                }
                if (imageUpdateActions.TryGetValue(type, out var action))
                {
                    action?.Invoke(frame);
                }
            }, DispatcherPriority.Render);
        }

        private void OnPauseResume()
        {
            if (playback == null || exited)
                return;

            if (!isPaused)
            {
                playback.Pause();
                isPaused = true;
                pauseResumeButton.Content = "Resume";
                Console.WriteLine("Pause Playback...");
            }
            else
            {
                playback.Resume();
                isPaused = false;
                pauseResumeButton.Content = "Pause";
                Console.WriteLine("Resume Playback...");
            }
        }

        private void Control_Closing(object sender, CancelEventArgs e)
        {
            tokenSource.Cancel();

            if (playback != null)
            {
                playback.Dispose();
            }
        }
    }
}