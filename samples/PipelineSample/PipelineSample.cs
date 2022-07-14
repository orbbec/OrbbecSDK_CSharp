using System;
using System.Threading;
using Orbbec;

class TestClass
{
    Filter f = new FormatConvertFilter();
    static void Main(string[] args)
    {
        FormatConvertFilter filter = new FormatConvertFilter();
        filter.SetConvertFormat(ConvertFormat.FORMAT_MJPEG_TO_RGB888);
        Pipeline pipeline = new Pipeline();
        //StreamProfile colorProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR).GetProfile(0);
        StreamProfile colorProfile = null;
        StreamProfileList colorProfiles = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_COLOR);
        for(int i = 0; i < colorProfiles.ProfileCount(); i++)
        {
            var profile = colorProfiles.GetProfile(i);
            if(profile.GetWidth() == 640 && profile.GetHeight() == 480 && profile.GetFPS() == 30 && profile.GetFormat() == Format.OB_FORMAT_MJPG)
            {
                Console.WriteLine("color profile: {0}x{1} {2}", profile.GetWidth(), profile.GetHeight(), profile.GetFormat());
                colorProfile = profile;
            }
        }
        StreamProfile depthProfile = pipeline.GetStreamProfileList(SensorType.OB_SENSOR_DEPTH).GetProfile(0);
        Config config = new Config();
        config.EnableStream(colorProfile);
        config.EnableStream(depthProfile);

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

                var frame = filter.Process(colorFrame);
                if (frame != null)
                {
                    var rgbFrame = frame.As<ColorFrame>();
                    Console.WriteLine("RGB {0} x {1} {2} {3}", rgbFrame.GetWidth(), rgbFrame.GetHeight(), rgbFrame.GetDataSize(), rgbFrame.GetFormat());
                    rgbFrame.Dispose();
                }
                frame.Dispose();

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