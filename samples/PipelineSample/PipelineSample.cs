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

        byte[] colorData = null;
        byte[] depthData = null;

        pipeline.Start(config, (frameset)=>{
            if(frameset == null)
            {
                Console.WriteLine("empty frameset");
                return;
            }
            ColorFrame colorFrame = frameset.GetColorFrame();
            if(colorFrame != null)
            {
                if(colorData == null)
                {
                    colorData = new byte[colorFrame.GetDataSize()];
                }
                Console.WriteLine("Color {0} x {1} {2}", colorFrame.GetWidth(), colorFrame.GetHeight(), colorFrame.GetDataSize());
                colorFrame.CopyData(ref colorData);
                Console.WriteLine("Color {0}-{1}", colorData[0], colorData[colorData.Length - 1]);
                colorFrame.Dispose();
            }
            
            DepthFrame depthFrame = frameset.GetDepthFrame();
            if(depthFrame != null)
            {
                if(depthData == null)
                {
                    depthData = new byte[depthFrame.GetDataSize()];
                }
                Console.WriteLine("Depth {0} x {1} {2}", depthFrame.GetWidth(), depthFrame.GetHeight(), depthFrame.GetDataSize());
                depthFrame.CopyData(ref depthData);
                Console.WriteLine("Depth {0}-{1}", depthData[0], depthData[depthData.Length - 1]);
                depthFrame.Dispose();
            }
            
            frameset.Dispose();
        });

        while(true)
        {
            Thread.Sleep(100);
        }
    }
}