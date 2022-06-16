using System.Threading;
using NUnit.Framework;
using Orbbec;

[TestFixture]
public class Sensor_Test
{
    private Context _context;
    private DeviceList _devList;
    private Device _device;
    private Sensor _depthSensor;
    private Sensor _colorSensor;
    private Sensor _irSensor;

    [OneTimeSetUp]
    public void SetUp()
    {
        _context = new Context();
        _devList = _context.QueryDeviceList();
        _device = _devList.GetDevice(0);
        _depthSensor = _device.GetSensor(SensorType.OB_SENSOR_DEPTH);
        _colorSensor = _device.GetSensor(SensorType.OB_SENSOR_COLOR);
        _irSensor = _device.GetSensor(SensorType.OB_SENSOR_IR);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _depthSensor.Dispose();
        _colorSensor.Dispose();
        _irSensor.Dispose();
        _device.Dispose();
        _devList.Dispose();
        _context.Dispose();
    }

    [Test]
    public void Depth_Sensor_Mode()
    {
        StreamProfileList profiles = _depthSensor.GetStreamProfileList();
        for(int i = 0; i < profiles.ProfileCount(); i++)
        {
            _depthSensor.Start(profiles.GetProfile(i), null);
            Thread.Sleep(2000);
            _depthSensor.Stop();
            Thread.Sleep(2000);
        }
    }

    [Test]
    public void Color_Sensor_Mode()
    {
        StreamProfileList profiles = _colorSensor.GetStreamProfileList();
        for(int i = 0; i < profiles.ProfileCount(); i++)
        {
            _colorSensor.Start(profiles.GetProfile(i), null);
            Thread.Sleep(2000);
            _colorSensor.Stop();
            Thread.Sleep(2000);
        }
    }

    [Test]
    public void IR_Sensor_Mode()
    {
        StreamProfileList profiles = _irSensor.GetStreamProfileList();
        for(int i = 0; i < profiles.ProfileCount(); i++)
        {
            _irSensor.Start(profiles.GetProfile(i), null);
            Thread.Sleep(2000);
            _irSensor.Stop();
            Thread.Sleep(2000);
        }
    }
}