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

        StreamProfile[] profiles = colorSen.GetStreamProfiles();
        Console.WriteLine(profiles.Length);

        foreach (var profile in profiles)
        {
            Console.WriteLine("Color {0} x {1} @ {2} {3}", profile.GetWidth(), profile.GetHeight(), profile.GetFPS(), profile.GetFormat());
        }

        byte[] data = null;
        
        colorSen.Start(profiles[0], (frame)=>{
            var vf = (frame as VideoFrame);
            // Console.WriteLine(vf.GetDataSize());
            Console.WriteLine("Color {0} x {1} {2}", vf.GetWidth(), vf.GetHeight(), vf.GetDataSize());
            if(data == null)
            {
                data = new byte[vf.GetDataSize()];
            }
            vf.CopyData(ref data);
            vf.Dispose();
            Console.WriteLine("Color {0}-{1}", data[0], data[data.Length - 1]);
            data = null;
        });

        profiles = depthSen.GetStreamProfiles();
        Console.WriteLine(profiles.Length);

        foreach (var profile in profiles)
        {
            Console.WriteLine("Depth {0} x {1} @ {2} {3}", profile.GetWidth(), profile.GetHeight(), profile.GetFPS(), profile.GetFormat());
        }

        depthSen.Start(profiles[0], (frame) => {
            var vf = (frame as VideoFrame);
            // Console.WriteLine(vf.GetDataSize());
            Console.WriteLine("Depth {0} x {1} {2}", vf.GetWidth(), vf.GetHeight(), vf.GetDataSize());
            if (data == null)
            {
                data = new byte[vf.GetDataSize()];
            }
            vf.CopyData(ref data);
            vf.Dispose();
            Console.WriteLine("Depth {0}-{1}", data[0], data[data.Length - 1]);
            data = null;
        });

        profiles = irSen.GetStreamProfiles();
        Console.WriteLine(profiles.Length);

        foreach (var profile in profiles)
        {
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