using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Orbbec
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class ImuWindow : Window
    {
        private readonly FrameCallback frameCallback;
        private Sensor accelSensor;
        private Sensor gyroSensor;
        private DispatcherTimer timer = new DispatcherTimer();
        // accel info and gyro info
        private AccelValue accelValue;
        private GyroValue gyroValue;
        private ulong accelTimestamp;
        private ulong gyroTimestamp;
        private double accelTemperature;
        private double gyroTemperature;

        public ImuWindow()
        {
            InitializeComponent();

            try
            {
                Pipeline pipeline = new Pipeline();

                Device device = pipeline.GetDevice();

                frameCallback = OnFrame;

                accelSensor = device.GetSensor(SensorType.OB_SENSOR_ACCEL);
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
                Stop();
                Application.Current.Shutdown();
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
        }

        private void Control_Closing(object sender, CancelEventArgs e)
        {
            Stop();
        }
    }
}