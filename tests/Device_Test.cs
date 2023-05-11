using NUnit.Framework;
using Orbbec;

[TestFixture]
public class Device_Test
{
    private Context _context;
    private DeviceList _devList;
    private Device _device;
    private SensorList _senList;

    private void TestBoolProperty(PropertyId property)
    {
        if (!_device.IsPropertySupported(property, PermissionType.OB_PERMISSION_READ_WRITE))
        {
            return;
        }

        BoolPropertyRange range = _device.GetBoolPropertyRange(property);
        bool value = false;
        _device.SetBoolProperty(property, value);
        Assert.AreEqual(_device.GetBoolProperty(property), value);
        value = true;
        _device.SetBoolProperty(property, value);
        Assert.AreEqual(_device.GetBoolProperty(property), value);
        value = range.def;
        _device.SetBoolProperty(property, value);
        Assert.AreEqual(_device.GetBoolProperty(property), value);
    }

    private void TestIntProperty(PropertyId property)
    {
        if (!_device.IsPropertySupported(property, PermissionType.OB_PERMISSION_READ_WRITE))
        {
            return;
        }

        IntPropertyRange range = _device.GetIntPropertyRange(property);
        int value = range.def + range.step > range.max ? range.max : range.def + range.step;
        _device.SetIntProperty(property, value);
        Assert.AreEqual(_device.GetIntProperty(property), value);
        value = range.def - range.step < range.min ? range.min : range.def + range.step;
        _device.SetIntProperty(property, value);
        Assert.AreEqual(_device.GetIntProperty(property), value);
        value = range.def;
        _device.SetIntProperty(property, value);
        Assert.AreEqual(_device.GetIntProperty(property), value);
    }

    private void TestFloatProperty(PropertyId property)
    {
        if (!_device.IsPropertySupported(property, PermissionType.OB_PERMISSION_READ_WRITE))
        {
            return;
        }

        FloatPropertyRange range = _device.GetFloatPropertyRange(property);
        float value = range.def + range.step > range.max ? range.max : range.def + range.step;
        _device.SetFloatProperty(property, value);
        Assert.AreEqual(_device.GetFloatProperty(property), value);
        value = range.def - range.step < range.min ? range.min : range.def + range.step;
        _device.SetFloatProperty(property, value);
        Assert.AreEqual(_device.GetFloatProperty(property), value);
        value = range.def;
        _device.SetFloatProperty(property, value);
        Assert.AreEqual(_device.GetFloatProperty(property), value);
    }

    [OneTimeSetUp]
    public void SetUp()
    {
        _context = new Context();
        _devList = _context.QueryDeviceList();
        _device = _devList.GetDevice(0);
        _senList = _device.GetSensorList();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _senList.Dispose();
        _device.Dispose();
        _devList.Dispose();
        _context.Dispose();
    }

    [Test]
    public void Device_ColorSensor()
    {
        Sensor sensor = _senList.GetSensor(SensorType.OB_SENSOR_COLOR);
        SensorType type = sensor.GetSensorType();
        Assert.AreEqual(type, SensorType.OB_SENSOR_COLOR);
        sensor.Dispose();
    }

    [Test]
    public void Device_DepthSensor()
    {
        Sensor sensor = _senList.GetSensor(SensorType.OB_SENSOR_DEPTH);
        SensorType type = sensor.GetSensorType();
        Assert.AreEqual(type, SensorType.OB_SENSOR_DEPTH);
        sensor.Dispose();
    }

    [Test]
    public void Device_IRSensor()
    {
        Sensor sensor = _senList.GetSensor(SensorType.OB_SENSOR_IR);
        SensorType type = sensor.GetSensorType();
        Assert.AreEqual(type, SensorType.OB_SENSOR_IR);
        sensor.Dispose();
    }

#region PROPERTY_BOOL
    [Test]
    public void Device_Property_LDP()
    {
        PropertyId property = PropertyId.OB_PROP_LDP_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Flood()
    {
        PropertyId property = PropertyId.OB_PROP_FLOOD_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Depth_Mirror()
    {
        PropertyId property = PropertyId.OB_PROP_DEPTH_MIRROR_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Depth_Flip()
    {
        PropertyId property = PropertyId.OB_PROP_DEPTH_FLIP_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Postfilter()
    {
        PropertyId property = PropertyId.OB_PROP_DEPTH_POSTFILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Holefilter()
    {
        PropertyId property = PropertyId.OB_PROP_DEPTH_HOLEFILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_IR_Mirror()
    {
        PropertyId property = PropertyId.OB_PROP_IR_MIRROR_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_IR_Flip()
    {
        PropertyId property = PropertyId.OB_PROP_IR_FLIP_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Soft_Filter()
    {
        PropertyId property = PropertyId.OB_PROP_DEPTH_SOFT_FILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Align_Hardware()
    {
        PropertyId property = PropertyId.OB_PROP_DEPTH_ALIGN_HARDWARE_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Hardware_Distortion_Switch()
    {
        PropertyId property = PropertyId.OB_PROP_HARDWARE_DISTORTION_SWITCH_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Color_Mirror()
    {
        PropertyId property = PropertyId.OB_PROP_COLOR_MIRROR_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Color_Flip()
    {
        PropertyId property = PropertyId.OB_PROP_COLOR_FLIP_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Indicator_Light()
    {
        PropertyId property = PropertyId.OB_PROP_INDICATOR_LIGHT_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Disparity_To_Depth()
    {
        PropertyId property = PropertyId.OB_PROP_DISPARITY_TO_DEPTH_BOOL;
        TestBoolProperty(property);
    }
#endregion

#region PROPERTY_INT
    [Test]
    public void Device_Property_Laser_Pulse_Width()
    {
        PropertyId property = PropertyId.OB_PROP_LASER_PULSE_WIDTH_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Flood_Level()
    {
        PropertyId property = PropertyId.OB_PROP_FLOOD_LEVEL_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Min_Depth()
    {
        PropertyId property = PropertyId.OB_PROP_MIN_DEPTH_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Max_Depth()
    {
        PropertyId property = PropertyId.OB_PROP_MAX_DEPTH_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Max_Diff()
    {
        PropertyId property = PropertyId.OB_PROP_DEPTH_MAX_DIFF_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Max_Speckle_Size()
    {
        PropertyId property = PropertyId.OB_PROP_DEPTH_MAX_SPECKLE_SIZE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Timestamp_Offset()
    {
        PropertyId property = PropertyId.OB_PROP_TIMESTAMP_OFFSET_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Fan_Work_Mode()
    {
        PropertyId property = PropertyId.OB_PROP_FAN_WORK_MODE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Align_Hardware_Mode()
    {
        PropertyId property = PropertyId.OB_PROP_DEPTH_ALIGN_HARDWARE_MODE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Filter_Range()
    {
        PropertyId property = PropertyId.OB_PROP_TOF_FILTER_RANGE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Laser_Mode()
    {
        PropertyId property = PropertyId.OB_PROP_LASER_MODE_INT;
        TestIntProperty(property);
    }
#endregion

#region PROPERTY_FLOAT
    [Test]
    public void Device_Property_Laser_Current()
    {
        PropertyId property = PropertyId.OB_PROP_LASER_CURRENT_FLOAT;
        TestFloatProperty(property);
    }
#endregion

#region SDK_PROPERTY
    [Test]
    public void SDK_Property_Disparity_To_Depth()
    {
        PropertyId property = PropertyId.OB_PROP_SDK_DISPARITY_TO_DEPTH_BOOL;
        TestBoolProperty(property);
    }
#endregion
}