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

    private void TestBoolProperty(Sensor sensor, PropertyId property)
    {
        if (!sensor.IsPropertySupported(property))
        {
            return;
        }

        BoolPropertyRange range = sensor.GetBoolPropertyRange(property);
        bool value = false;
        sensor.SetBoolProperty(property, value);
        Assert.AreEqual(sensor.GetBoolProperty(property), value);
        value = true;
        sensor.SetBoolProperty(property, value);
        Assert.AreEqual(sensor.GetBoolProperty(property), value);
        value = range.def;
        sensor.SetBoolProperty(property, value);
        Assert.AreEqual(sensor.GetBoolProperty(property), value);
    }

    private void TestIntProperty(Sensor sensor, PropertyId property)
    {
        if (!sensor.IsPropertySupported(property))
        {
            return;
        }

        IntPropertyRange range = sensor.GetIntPropertyRange(property);
        int value = range.def + range.step > range.max ? range.max : range.def + range.step;
        sensor.SetIntProperty(property, value);
        Assert.AreEqual(sensor.GetIntProperty(property), value);
        value = range.def - range.step < range.min ? range.min : range.def + range.step;
        sensor.SetIntProperty(property, value);
        Assert.AreEqual(sensor.GetIntProperty(property), value);
        value = range.def;
        sensor.SetIntProperty(property, value);
        Assert.AreEqual(sensor.GetIntProperty(property), value);
    }

    private void TestFloatProperty(Sensor sensor, PropertyId property)
    {
        if (!sensor.IsPropertySupported(property))
        {
            return;
        }

        FloatPropertyRange range = sensor.GetFloatPropertyRange(property);
        float value = range.def + range.step;
        sensor.SetFloatProperty(property, value);
        Assert.AreEqual(sensor.GetFloatProperty(property), value);
        value = range.def - range.step;
        sensor.SetFloatProperty(property, value);
        Assert.AreEqual(sensor.GetFloatProperty(property), value);
        value = range.def;
        sensor.SetFloatProperty(property, value);
        Assert.AreEqual(sensor.GetFloatProperty(property), value);
    }

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
        StreamProfile[] profiles = _depthSensor.GetStreamProfiles();
        foreach (var profile in profiles)
        {
            _depthSensor.Start(profile, null);
            Thread.Sleep(2000);
            _depthSensor.Stop();
            Thread.Sleep(2000);
        }
    }

    [Test]
    public void Color_Sensor_Mode()
    {
        StreamProfile[] profiles = _colorSensor.GetStreamProfiles();
        foreach (var profile in profiles)
        {
            _colorSensor.Start(profile, null);
            Thread.Sleep(2000);
            _colorSensor.Stop();
            Thread.Sleep(2000);
        }
    }

    [Test]
    public void IR_Sensor_Mode()
    {
        StreamProfile[] profiles = _irSensor.GetStreamProfiles();
        foreach (var profile in profiles)
        {
            _irSensor.Start(profile, null);
            Thread.Sleep(2000);
            _irSensor.Stop();
            Thread.Sleep(2000);
        }
    }

#region COLOR_SENSOR_PROPERTY
    [Test]
    public void Color_Sensor_Property_Enable_Auto_Exposure()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_ENABLE_AUTO_EXPOSURE_BOOL;
        TestBoolProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Exposure()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_EXPOSURE_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Gain()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_GAIN_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Enable_Auto_White_Balance()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_ENABLE_AUTO_WHITE_BALANCE_BOOL;
        TestBoolProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_White_Balance()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_WHITE_BALANCE_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Brighteness()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_BRIGHTNESS_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Sharpness()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_SHARPNESS_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Saturation()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_SATURATION_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Contrast()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_CONTRAST_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Gamma()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_GAMMA_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Roll()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_ROLL_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Auto_Exposure_Priority()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_AUTO_EXPOSURE_PRIORITY_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Backlight_Compensation()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_BACKLIGHT_COMPENSATION_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Hue()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_HUE_INT;
        TestIntProperty(_colorSensor, property);
    }

    [Test]
    public void Color_Sensor_Property_Power_Line_Frequency()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_POWER_LINE_FREQUENCY_INT;
        TestIntProperty(_colorSensor, property);
    }
#endregion

#region DEPTH_SENSOR_PROPERTY
    [Test]
    public void Depth_Sensor_Property_Enable_Auto_Exposure()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_ENABLE_AUTO_EXPOSURE_BOOL;
        TestBoolProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Exposure()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_EXPOSURE_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Gain()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_GAIN_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Enable_Auto_White_Balance()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_ENABLE_AUTO_WHITE_BALANCE_BOOL;
        TestBoolProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_White_Balance()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_WHITE_BALANCE_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Brighteness()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_BRIGHTNESS_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Sharpness()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_SHARPNESS_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Saturation()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_SATURATION_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Contrast()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_CONTRAST_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Gamma()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_GAMMA_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Roll()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_ROLL_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Auto_Exposure_Priority()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_AUTO_EXPOSURE_PRIORITY_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Backlight_Compensation()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_BACKLIGHT_COMPENSATION_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Hue()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_HUE_INT;
        TestIntProperty(_depthSensor, property);
    }

    [Test]
    public void Depth_Sensor_Property_Power_Line_Frequency()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_POWER_LINE_FREQUENCY_INT;
        TestIntProperty(_depthSensor, property);
    }
#endregion

#region IR_SENSOR_PROPERTY
    [Test]
    public void IR_Sensor_Property_Enable_Auto_Exposure()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_ENABLE_AUTO_EXPOSURE_BOOL;
        TestBoolProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Exposure()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_EXPOSURE_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Gain()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_GAIN_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Enable_Auto_White_Balance()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_ENABLE_AUTO_WHITE_BALANCE_BOOL;
        TestBoolProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_White_Balance()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_WHITE_BALANCE_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Brighteness()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_BRIGHTNESS_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Sharpness()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_SHARPNESS_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Saturation()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_SATURATION_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Contrast()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_CONTRAST_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Gamma()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_GAMMA_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Roll()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_ROLL_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Auto_Exposure_Priority()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_AUTO_EXPOSURE_PRIORITY_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Backlight_Compensation()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_BACKLIGHT_COMPENSATION_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Hue()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_HUE_INT;
        TestIntProperty(_irSensor, property);
    }

    [Test]
    public void IR_Sensor_Property_Power_Line_Frequency()
    {
        PropertyId property = PropertyId.OB_SENSOR_PROPERTY_POWER_LINE_FREQUENCY_INT;
        TestIntProperty(_irSensor, property);
    }
#endregion
}