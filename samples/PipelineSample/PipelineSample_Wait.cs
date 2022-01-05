using System;
using System.Threading;
using Orbbec;

class TestClass
{
    static void Main(string[] args)
    {
        Pipeline pipeline = new Pipeline();
        StreamProfile colorProfile = pipeline.GetStreamProfiles(SensorType.OB_SENSOR_COLOR)[0];
        StreamProfile depthProfile = pipeline.GetStreamProfiles(SensorType.OB_SENSOR_DEPTH)[0];
        Config config = new Config();
        config.EnableStream(colorProfile);
        config.EnableStream(depthProfile);
        pipeline.Start(config);
        byte[] data = null;

        while(true)
        {
            Frameset frameset = pipeline.WaitForFrames(1000);
            if(frameset == null)
            {
                continue;
            }
            ColorFrame colorFrame = frameset.GetColorFrame();
            DepthFrame depthFrame = frameset.GetDepthFrame();

            if(colorFrame != null)
            {
                Console.WriteLine("Color {0} x {1} {2}", colorFrame.GetWidth(), colorFrame.GetHeight(), colorFrame.GetDataSize());
                if (data == null)
                {
                    data = new byte[colorFrame.GetDataSize()];
                }
                colorFrame.CopyData(ref data);
                colorFrame.Dispose();
                Console.WriteLine("Color {0}-{1}", data[0], data[data.Length - 1]);
                data = null;
            }

            if(depthFrame != null)
            {
                Console.WriteLine("Depth {0} x {1} {2}", depthFrame.GetWidth(), depthFrame.GetHeight(), depthFrame.GetDataSize());
                if (data == null)
                {
                    data = new byte[depthFrame.GetDataSize()];
                }
                depthFrame.CopyData(ref data);
                depthFrame.Dispose();
                Console.WriteLine("Depth {0}-{1}-{2}", data[0], data[data.Length - 1], data[data.Length / 2]);
                data = null;
            }

            Thread.Sleep(100);
        }
    }
}