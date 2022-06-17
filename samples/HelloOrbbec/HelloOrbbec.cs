using System;
using System.Threading;
using Orbbec;

class TestClass
{
    static void Main(string[] args)
    {
        // Display the number of command line arguments.
        Console.WriteLine(args.Length);
        Context ctx = new Context();
        DeviceList devList = ctx.QueryDeviceList();
        Console.WriteLine(devList.DeviceCount());
        Device dev = devList.GetDevice(0);
        DeviceInfo devInfo = dev.GetDeviceInfo();
        Console.WriteLine(devInfo.Name());
        Console.WriteLine(devInfo.SerialNumber());
        Console.WriteLine(devInfo.Pid());
        Console.WriteLine(devInfo.Vid());
        Console.WriteLine(devInfo.Uid());

        SensorList senList = dev.GetSensorList();
        Console.WriteLine(senList.SensorCount());

        Sensor colorSen = dev.GetSensor(SensorType.OB_SENSOR_COLOR);
        Console.WriteLine(colorSen.GetSensorType());

        Sensor depthSen = dev.GetSensor(SensorType.OB_SENSOR_DEPTH);
        Console.WriteLine(depthSen.GetSensorType());

        Sensor irSen = dev.GetSensor(SensorType.OB_SENSOR_IR);
        Console.WriteLine(irSen.GetSensorType());

        StreamProfileList profiles = colorSen.GetStreamProfileList();
        Console.WriteLine(profiles.ProfileCount());

        for (int i = 0; i < profiles.ProfileCount(); i++)
        {
            var profile = profiles.GetProfile(i);
            Console.WriteLine("Color {0} x {1} @ {2} {3}", profile.GetWidth(), profile.GetHeight(), profile.GetFPS(), profile.GetFormat());
        }

        byte[] colorData = null;
        
        colorSen.Start(profiles.GetProfile(0), (frame)=>{
            if(frame == null) 
            {
                Console.WriteLine("empty color frame");
                return;
            }
            var vf = frame.As<ColorFrame>();
            // Console.WriteLine(vf.GetDataSize());
            Console.WriteLine("Color {0} x {1} {2}", vf.GetWidth(), vf.GetHeight(), vf.GetDataSize());
            if(colorData == null)
            {
                colorData = new byte[vf.GetDataSize()];
            }
            vf.CopyData(ref colorData);
            vf.Dispose();
            Console.WriteLine("Color {0}-{1}", colorData[0], colorData[colorData.Length - 1]);
        });

        profiles = depthSen.GetStreamProfileList();
        Console.WriteLine(profiles.ProfileCount());

        for (int i = 0; i < profiles.ProfileCount(); i++)
        {
            var profile = profiles.GetProfile(i);
            Console.WriteLine("Depth {0} x {1} @ {2} {3}", profile.GetWidth(), profile.GetHeight(), profile.GetFPS(), profile.GetFormat());
        }

        byte[] depthData = null;

        depthSen.Start(profiles.GetProfile(0), (frame) => {
            if(frame == null) 
            {
                Console.WriteLine("empty depth frame");
                return;
            }
            var vf = frame.As<DepthFrame>();
            // Console.WriteLine(vf.GetDataSize());
            Console.WriteLine("Depth {0} x {1} {2}", vf.GetWidth(), vf.GetHeight(), vf.GetDataSize());
            if (depthData == null)
            {
                depthData = new byte[vf.GetDataSize()];
            }
            vf.CopyData(ref depthData);
            vf.Dispose();
            Console.WriteLine("Depth {0}-{1}", depthData[0], depthData[depthData.Length - 1]);
        });

        profiles = irSen.GetStreamProfileList();
        Console.WriteLine(profiles.ProfileCount());

        for (int i = 0; i < profiles.ProfileCount(); i++)
        {
            var profile = profiles.GetProfile(i);
            Console.WriteLine("IR {0} x {1} @ {2} {3}", profile.GetWidth(), profile.GetHeight(), profile.GetFPS(), profile.GetFormat());
        }

        dev.SetBoolProperty(PropertyId.OB_DEVICE_PROPERTY_EMITTER_BOOL, false);
        dev.SetBoolProperty(PropertyId.OB_DEVICE_PROPERTY_EMITTER_BOOL, true);

        while (true)
        {
            Thread.Sleep(100);
        }
    }
}