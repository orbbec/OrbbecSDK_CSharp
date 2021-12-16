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
        if (!_device.IsPropertySupported(property))
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
        if (!_device.IsPropertySupported(property))
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
        if (!_device.IsPropertySupported(property))
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
    public void Device_Property_Flash_Write_Protection()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_FLASH_WRITE_PROTECTION_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_TEC_Enable()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TEC_ENABLE_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_LDP()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_LDP_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Emitter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_EMITTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Flood()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_FLOOD_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Temperature_Compensation_Enable()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TEMPERATURE_COMPENSATION_ENABLE_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Align_Software()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_ALIGN_SOFTWARE_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Depth_Mirror()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_MIRROR_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Depth_Flip()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_FLIP_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Postfilter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_POSTFILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Holefilter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_HOLEFILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_IR_Mirror()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_IR_MIRROR_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_IR_Flip()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_IR_FLIP_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_IR_Switch()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_IR_SWITCH_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Hardware_Sync()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_HARDWARE_SYNC_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Soft_Filter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_SOFT_FILTER_BOOL;
        TestBoolProperty(property);
    }

    // [Test]
    // public void Device_Property_Soft_Reset()
    // {
    //     PropertyId property = PropertyId.OB_DEVICE_PROPERTY_SOFT_RESET_BOOL;
    //     TestBoolProperty(property);
    // }

    [Test]
    public void Device_Property_Stop_Depth_Stream()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_STOP_DEPTH_STREAM_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Stop_IR_Stream()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_STOP_IR_STREAM_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Align_Hardware()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_ALIGN_HARDWARE_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Orientation_Switch()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_ORIENTATION_SWITCH_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Enable_Calibration()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_ENABLE_CALIBRATION_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Gaussian_Filter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_GAUSSIAN_FILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Scatter_Filter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_SCATTER_FILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Bilateral_Filter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_BILATERAL_FILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Fly_Point_Filter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_FLY_POINT_FILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Median_Filter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_MEDIAN_FILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Confidence_Filter()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_CONFIDENCE_FILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Shuffle_Mode()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_SHUFFLE_MODE_BOOL;
        TestBoolProperty(property);
    }

    // [Test]
    // public void Device_Property_Reboot_Device()
    // {
    //     PropertyId property = PropertyId.OB_DEVICE_PROPERTY_REBOOT_DEVICE_BOOL;
    //     TestBoolProperty(property);
    // }

    // [Test]
    // public void Device_Property_Factory_Mode()
    // {
    //     PropertyId property = PropertyId.OB_DEVICE_PROPERTY_FACTORY_RESET_BOOL;
    //     TestBoolProperty(property);
    // }

    [Test]
    public void Device_Property_IR_Mode_Switch()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_IR_MODE_SWITCH_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Frame_Rate_Mode_Switch()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_FRAME_RATE_MODE_SWITCH_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Hardware_Distortion_Switch()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_HARDWARE_DISTORTION_SWITCH_BOOL;
        TestBoolProperty(property);
    }

    // [Test]
    // public void Device_Property_Anti_Collusion_Activation()
    // {
    //     PropertyId property = PropertyId.OB_DEVICE_PROPERTY_ANTI_COLLUSION_ACTIVATION_STATUS_BOOL;
    //     TestBoolProperty(property);
    // }

    [Test]
    public void Device_Property_Software_Distortion_Switch()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_SOFTWARE_DISTORTION_SWITCH_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Stop_Color_Stream()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_STOP_COLOR_STREAM_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Factory_Mode()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_FACTORY_MODE_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Sbg_Rectify_Mode()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_SBG_RECTIFY_MODE_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Color_Mirror()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_COLOR_MIRROR_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Color_Flip()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_COLOR_FLIP_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Indicator_Light()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_INDICATOR_LIGHT_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Calibration_Mode()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_CALIBRATION_MODE_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void Device_Property_Disparity_To_Depth()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DISPARITY_TO_DEPTH_BOOL;
        TestBoolProperty(property);
    }
#endregion

#region PROPERTY_INT
    [Test]
    public void Device_Property_Laser_Pulse_Width()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_LASER_PULSE_WIDTH_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Flood_Level()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_FLOOD_LEVEL_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Min_Depth()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_MIN_DEPTH_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Max_Depth()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_MAX_DEPTH_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Chip_Type()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_CHIP_TYPE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Usb_Speed()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_USB_SPEED_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Ldp_Thres_Up()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_LDP_THRES_UP_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Ldp_Thres_Low()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_LDP_THRES_LOW_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Max_Diff()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_MAX_DIFF_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Max_Speckle_Size()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_MAX_SPECKLE_SIZE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Timestamp_Offset()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TIMESTAMP_OFFSET_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Exposure_Time()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_EXPOSURE_TIME_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Gain()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_GAIN_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Mirror()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_MIRROR_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Fan_Work_Mode()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_FAN_WORK_MODE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Align_Hardware_Mode()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_ALIGN_HARDWARE_MODE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Sync_Mode_Int()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_SYNC_MODE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Sync_Ext_Out_Delay_Time()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_SYNC_EXT_OUT_DELAY_TIME_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_IR_Frame_Rate()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_IR_FRAME_RATE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Frame_Rate()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_FRAME_RATE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Tec_Max_Current()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TEC_MAX_CURRENT_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Tec_Max_Current_Config()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TEC_MAX_CURRENT_CONFIG_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Fan_Work_Mode_Config()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_FAN_WORK_MODE_CONFIG_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Bit_Per_Pixel()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_BIT_PER_PIXEL_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_IR_Bit_Per_Pixel()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_IR_BIT_PER_PIXEL_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Depth_Unit()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_DEPTH_UNIT_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Tof_Filter_Range()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TOF_FILTER_RANGE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void Device_Property_Laser_Mode()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_LASER_MODE_INT;
        TestIntProperty(property);
    }
#endregion

#region PROPERTY_FLOAT
    [Test]
    public void Device_Property_Laser_Current()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_LASER_CURRENT_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_Temperature_Calibrated_IR()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TEMPERATURE_CALIBRATED_IR_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_Temperature_Calibrated_LDMP()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TEMPERATURE_CALIBRATED_LDMP_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_Temperature_Compensation_Coefficient_IR()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TEMPERATURE_COMPENSATION_COEFFICIENT_IR_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_Temperature_Compensation_Coefficient_LDMP()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_TEMPERATURE_COMPENSATION_COEFFICIENT_LDMP_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_Zero_Plane_Distance()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_ZERO_PLANE_DISTANCE_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_Zero_Plane_Pixel_Size()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_ZERO_PLANE_PIXEL_SIZE_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_Laser_Temperature()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_LASER_TEMPERATURE_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_RT_IR_Temp()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_RT_IR_TEMP_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_RT_LDMP_Temp()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_RT_LDMP_TEMP_FLOAT;
        TestFloatProperty(property);
    }

    [Test]
    public void Device_Property_RT_RGB_Temp()
    {
        PropertyId property = PropertyId.OB_DEVICE_PROPERTY_RT_RGB_TEMP_FLOAT;
        TestFloatProperty(property);
    }
#endregion

#region SDK_PROPERTY
    [Test]
    public void SDK_Property_Depth_Soft_Filter()
    {
        PropertyId property = PropertyId.OB_SDK_PROPERTY_DEPTH_SOFT_FILTER_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void SDK_Property_Depth_Max_Diff()
    {
        PropertyId property = PropertyId.OB_SDK_PROPERTY_DEPTH_MAX_DIFF_INT;
        TestIntProperty(property);
    }

    [Test]
    public void SDK_Property_Depth_Max_Speckle_Size()
    {
        PropertyId property = PropertyId.OB_SDK_PROPERTY_DEPTH_MAX_SPECKLE_SIZE_INT;
        TestIntProperty(property);
    }

    [Test]
    public void SDK_Property_Soft_Filter_Type()
    {
        PropertyId property = PropertyId.OB_SDK_PROPERTY_SOFT_FILTER_TYPE_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void SDK_Property_Disparity_To_Depth()
    {
        PropertyId property = PropertyId.OB_SDK_PROPERTY_DISPARITY_TO_DEPTH_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void SDK_Property_IR_Mirror()
    {
        PropertyId property = PropertyId.OB_SDK_PROPERTY_IR_MIRROR_BOOL;
        TestBoolProperty(property);
    }

    [Test]
    public void SDK_Property_Depth_RLE_Decode()
    {
        PropertyId property = PropertyId.OB_SDK_PROPERTY_DEPTH_RLE_DECODE_BOOL;
        TestBoolProperty(property);
    }
#endregion
}