using System.Threading;
using NUnit.Framework;
using Orbbec;

[TestFixture]
public class Pipeline_Test
{
    private Config _config;
    private Pipeline _pipe;
    private Device _device;

    [OneTimeSetUp]
    public void SetUp()
    {
        _config = new Config();
        _pipe = new Pipeline();
        _device = _pipe.GetDevice();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _config.Dispose();
        _device.Dispose();
        _pipe.Dispose();
    }

    [Test]
    public void Pipeline_AllStreamProfiles()
    {
        StreamProfile[] profiles = _pipe.GetAllStreamProfiles();
        Assert.Greater(profiles.Length, 0);
        foreach (var profile in profiles)
        {
            profile.Dispose();
        }
    }

    [Test]
    public void Pipeline_ColorStreamProfiles()
    {
        StreamProfile[] profiles = _pipe.GetStreamProfiles(SensorType.OB_SENSOR_COLOR);
        Assert.Greater(profiles.Length, 0);
        foreach (var profile in profiles)
        {
            profile.Dispose();
        }
    }

    [Test]
    public void Pipeline_DepthStreamProfiles()
    {
        StreamProfile[] profiles = _pipe.GetStreamProfiles(SensorType.OB_SENSOR_DEPTH);
        Assert.Greater(profiles.Length, 0);
        foreach (var profile in profiles)
        {
            profile.Dispose();
        }
    }

    [Test]
    public void Pipeline_IRStreamProfiles()
    {
        StreamProfile[] profiles = _pipe.GetStreamProfiles(SensorType.OB_SENSOR_IR);
        Assert.Greater(profiles.Length, 0);
        foreach (var profile in profiles)
        {
            profile.Dispose();
        }
    }

    [Test]
    public void Pipeline_GetDevice()
    {
        Sensor colorSen = _device.GetSensor(SensorType.OB_SENSOR_COLOR);
        Assert.AreEqual(colorSen.GetSensorType(), SensorType.OB_SENSOR_COLOR);
        Sensor depthSen = _device.GetSensor(SensorType.OB_SENSOR_DEPTH);
        Assert.AreEqual(depthSen.GetSensorType(), SensorType.OB_SENSOR_DEPTH);
        Sensor irSen = _device.GetSensor(SensorType.OB_SENSOR_IR);
        Assert.AreEqual(irSen.GetSensorType(), SensorType.OB_SENSOR_IR);
        colorSen.Dispose();
        depthSen.Dispose();
        irSen.Dispose();
    }

    [Test]
    public void Pipeline_StartColor()
    {
        _config.DisableAllStream();
        StreamProfile[] profiles = _pipe.GetStreamProfiles(SensorType.OB_SENSOR_COLOR);
        _config.EnableStream(profiles[0]);
        _pipe.Start(_config);
        Thread.Sleep(2000);
        _pipe.Stop();
        foreach (var profile in profiles)
        {
            profile.Dispose();
        }
    }

    [Test]
    public void Pipeline_StartDepth()
    {
        _config.DisableAllStream();
        StreamProfile[] profiles = _pipe.GetStreamProfiles(SensorType.OB_SENSOR_DEPTH);
        _config.EnableStream(profiles[0]);
        _pipe.Start(_config);
        Thread.Sleep(2000);
        _pipe.Stop();
        foreach (var profile in profiles)
        {
            profile.Dispose();
        }
    }

    [Test]
    public void Pipeline_StartIR()
    {
        _config.DisableAllStream();
        StreamProfile[] profiles = _pipe.GetStreamProfiles(SensorType.OB_SENSOR_IR);
        _config.EnableStream(profiles[0]);
        _pipe.Start(_config);
        Thread.Sleep(2000);
        _pipe.Stop();
        foreach (var profile in profiles)
        {
            profile.Dispose();
        }
    }

    [Test]
    public void Pipeline_StartAll()
    {
        _config.EnableAllStream();
        _pipe.Start(_config);
        Thread.Sleep(2000);
        _pipe.Stop();
    }
}