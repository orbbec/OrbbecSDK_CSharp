using System;
using System.Threading;
using Orbbec;

//10.10.71.55
class TestClass
{
    static void Main(string[] args)
    {
        Context ctx = new Context();
        Device dev = ctx.CreateNetDevice("192.168.1.10", 8090);
        Pipeline pipeline = new Pipeline(dev);
        StreamProfileList colorProfileList = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR);
        StreamProfile colorProfile = colorProfileList.GetVideoStreamProfile(1920, 0, Format.OB_FORMAT_MJPG, 15);
        StreamProfileList depthProfileList = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH);
        StreamProfile depthProfile = depthProfileList.GetVideoStreamProfile(1024, 0, Format.OB_FORMAT_Y16, 15);
        Config config = new Config();
        config.EnableStream(colorProfile);
        config.EnableStream(depthProfile);

        config.SetAlignMode(AlignMode.ALIGN_D2C_HW_MODE);
        // pipeline.EnableFrameSync();

        byte[] colorData = null;
        byte[] depthData = null;

        pipeline.Start(config, (frameset) =>{
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
                System.IO.File.WriteAllBytes("color.mjpg", colorData);
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
                System.IO.File.WriteAllBytes("depth.raw", depthData);
            }
            
            frameset.Dispose();
        });

        while(true)
        {
            Thread.Sleep(100);
        }
    }
}