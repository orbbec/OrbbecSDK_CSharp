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

        CameraParamList cameraParamList = dev.GetCalibrationCameraParamList();
        for(uint i = 0; i < cameraParamList.Count(); i++)
        {
            CameraParam cameraParam = cameraParamList.GetCameraParam(i);
            Console.WriteLine(cameraParam.depthIntrinsic.fx);
            Console.WriteLine(cameraParam.depthIntrinsic.fy);
            Console.WriteLine(cameraParam.depthIntrinsic.cx);
            Console.WriteLine(cameraParam.depthIntrinsic.cy);
            Console.WriteLine(cameraParam.depthIntrinsic.width);
            Console.WriteLine(cameraParam.depthIntrinsic.height);
            Console.WriteLine(cameraParam.depthDistortion.k1);
            Console.WriteLine(cameraParam.depthDistortion.k2);
            Console.WriteLine(cameraParam.depthDistortion.k3);
            Console.WriteLine(cameraParam.depthDistortion.k4);
            Console.WriteLine(cameraParam.depthDistortion.k5);
            Console.WriteLine(cameraParam.depthDistortion.k6);
            Console.WriteLine(cameraParam.depthDistortion.p1);
            Console.WriteLine(cameraParam.depthDistortion.p2);
        }

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
            var profile = profiles.GetProfile(i).As<VideoStreamProfile>();
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
            var profile = profiles.GetProfile(i).As<VideoStreamProfile>();
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
            var profile = profiles.GetProfile(i).As<VideoStreamProfile>();
            Console.WriteLine("IR {0} x {1} @ {2} {3}", profile.GetWidth(), profile.GetHeight(), profile.GetFPS(), profile.GetFormat());
        }

        while (true)
        {
            Thread.Sleep(100);
        }
    }
}