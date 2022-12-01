using System;

namespace Orbbec
{
    public delegate void DeviceStateCallback(UInt64 state, String message);
    public delegate void DeviceStateCallbackInternal(UInt64 state, IntPtr message, IntPtr userData);
    public delegate void SetDataCallback(DataTranState state, uint percent);
    public delegate void SetDataCallbackInternal(DataTranState state, uint percent, IntPtr userData);
    public delegate void GetDataCallback(DataTranState state, DataChunk dataChunk);
    public delegate void GetDataCallbackInternal(DataTranState state, IntPtr dataChunk, IntPtr userData);

    public class Device : IDisposable
    {
        private NativeHandle _handle;

        internal Device(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        internal NativeHandle GetNativeHandle()
        {
            return _handle;
        }

        /**
        * \if English
        * @brief Get device information
        *
        * @return DeviceInfo returns device information
        * \else
        * @brief 获取设备信息
        *
        * @return DeviceInfo 返回设备的信息
        * \endif
        */
        public DeviceInfo GetDeviceInfo()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_device_get_device_info(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new DeviceInfo(handle);
        }

        /**
        * \if English
        * @brief Get device sensor list
        *
        * @return SensorList returns the sensor list
        * \else
        * @brief 获取设备传感器列表
        *
        * @return SensorList 返回传感器列表
        * \endif
        */
        public SensorList GetSensorList()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_device_get_sensor_list(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new SensorList(handle);
        }

        /**
        * \if English
        * @brief Get specific type of sensor
        * if device not open, SDK will automatically open the connected device and return to the instance
        *
        * @return Sensor eturns the sensor example, if the device does not have the device,returns nullptr
        * \else
        * @brief 获取指定类型传感器
        * 如果设备没有打开传感器，在SDK内部会自动打开设备并返回实例
        *
        * @return Sensor 返回传感器示例，如果设备没有该设备，返回nullptr
        * \endif
        */
        public Sensor GetSensor(SensorType sensorType)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_device_get_sensor(_handle.Ptr, sensorType, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new Sensor(handle);
        }

        /**
        * \if English
        * @brief Set int type of device property
        *
        * @param propertyId Property id
        * @param property Property to be set
        * \else
        * @brief 设置int类型的设备属性
        *
        * @param propertyId 属性id
        * @param property 要设置的属性
        * \endif
        */
        public void SetIntProperty(PropertyId propertyId, Int32 property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_set_int_property(_handle.Ptr, propertyId, property, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Get int type of device property
        *
        * @param propertyId Property id
        * @return Int32 Property to get
        * \else
        * @brief 获取int类型的设备属性
        *
        * @param propertyId 属性id
        * @return Int32 获取的属性数据
        * \endif
        */
        public Int32 GetIntProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            Int32 value = obNative.ob_device_get_int_property(_handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return value;
        }

        /**
        * \if English
        * @brief Set float type of device property
        *
        * @param propertyId Property id
        * @param property Property to be set
        * \else
        * @brief 设置float类型的设备属性
        *
        * @param propertyId 属性id
        * @param property 要设置的属性
        * \endif
        */
        public void SetFloatProperty(PropertyId propertyId, float property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_set_float_property(_handle.Ptr, propertyId, property, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Get float type of device property
        *
        * @param propertyId Property id
        * @return float Property to get
        * \else
        * @brief 获取float类型的设备属性
        *
        * @param propertyId 属性id
        * @return float 获取的属性数据
        * \endif
        */
        public float GetFloatProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            float value = obNative.ob_device_get_float_property(_handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return value;
        }

        /**
        * \if English
        * @brief Set bool type of device property
        *
        * @param propertyId Property id
        * @param property Property to be set
        * \else
        * @brief 设置bool类型的设备属性
        *
        * @param propertyId 属性id
        * @param property 要设置的属性
        * \endif
        */
        public void SetBoolProperty(PropertyId propertyId, bool property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_set_bool_property(_handle.Ptr, propertyId, property, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Get bool type of device property
        *
        * @param propertyId Property id
        * @return bool Property to get
        * \else
        * @brief 获取bool类型的设备属性
        *
        * @param propertyId 属性id
        * @return bool 获取的属性数据
        * \endif
        */
        public bool GetBoolProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            bool value = obNative.ob_device_get_bool_property(_handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return value;
        }

        /**
        * \if English
        * @brief Set structured data type of device property
        *
        * @param propertyId Property id
        * @param data Property data to be set
        * @param dataSize The size of the attribute to be set
        * \else
        * @brief 设置structured data类型的设备属性
        *
        * @param propertyId 属性id
        * @param data 要设置的属性数据
        * @param dataSize 要设置的属性大小
        * \endif
        */
        public void SetStructuredData(PropertyId propertyId, IntPtr data, UInt32 dataSize)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_set_structured_data(_handle.Ptr, propertyId, data, dataSize, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Get structured data type of device property
        *
        * @param propertyId Property id
        * @param data Property data obtained
        * @param dataSize Get the size of the attribute
        * \else
        * @brief 获取structured data类型的设备属性
        *
        * @param propertyId 属性id
        * @param data 获取的属性数据
        * @param dataSize 获取的属性大小
        * \endif
        */
        public void GetStructuredData(PropertyId propertyId, IntPtr data, ref UInt32 dataSize)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_get_structured_data(_handle.Ptr, propertyId, data, ref dataSize, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Judge property permission support
        *
        * @param propertyId Property id
        * @param permission Types of read and write permissions that need to be interpreted
        * @return bool returns whether it is supported
        * \else
        * @brief 判断属性权限支持情况
        *
        * @param propertyId 属性id
        * @param permission 需要判读的读写权限类型
        * @return bool 返回是否支持
        * \endif
        */
        public bool IsPropertySupported(PropertyId propertyId, PermissionType permissionType)
        {
            IntPtr error = IntPtr.Zero;
            bool isSupported = obNative.ob_device_is_property_supported(_handle.Ptr, propertyId, permissionType, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return isSupported;
        }

        /**
        * \if English
        * @brief Get int type device property range (ncluding current valueand default value)
        *
        * @param propertyId Property id
        * @return IntPropertyRange Property range
        * \else
        * @brief 获取int类型的设备属性的范围(包括当前值和默认值)
        *
        * @param propertyId 属性id
        * @return IntPropertyRange 属性的范围
        * \endif
        */
        public IntPropertyRange GetIntPropertyRange (PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            IntPropertyRange range;
            obNative.ob_device_get_int_property_range(out range, _handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        /**
        * \if English
        * @brief Get float type device property range((including current valueand default value)
        *
        * @param propertyId Property id
        * @return FloatPropertyRange Property range
        * \else
        * @brief 获取float类型的设备属性的范围(包括当前值和默认值)
        *
        * @param propertyId 属性id
        * @return FloatPropertyRange 属性的范围
        * \endif
        */
        public FloatPropertyRange GetFloatPropertyRange (PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            FloatPropertyRange range;
            obNative.ob_device_get_float_property_range(out range, _handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        /**
        * \if English
        * @brief Get bool type device property range (including current value anddefault value)
        *
        * @param propertyId Property id
        * @return GetBoolPropertyRange Property range
        * \else
        * @brief 获取bool类型的设备属性的范围(包括当前值和默认值)
        *
        * @param propertyId 属性id
        * @return GetBoolPropertyRange 属性的范围
        * \endif
        */
        public BoolPropertyRange GetBoolPropertyRange(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            BoolPropertyRange range;
            obNative.ob_device_get_bool_property_range(out range, _handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        /**
        * \if English
        * @brief Get the current state
        * @return UInt64 device state information
        * \else
        * @brief 获取当前设备状态
        * @return UInt64 设备状态信息
        * \endif
        */
        public UInt64 GetDeviceState()
        {
            IntPtr error = IntPtr.Zero;
            UInt64 state = obNative.ob_device_get_device_state(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return state;
        }

        // public void SetDeviceStateCallback(DeviceStateCallback callback)
        // {
        //     IntPtr error = IntPtr.Zero;
        //     obNative.ob_device_state_changed(_handle.Ptr, callback, IntPtr.Zero, out error);
        // }

        /**
        * \if English
        * @brief Verify device authorization code
        * @param authCode Authorization code
        * @return bool whether the activation is successfu
        * \else
        * @brief 验证设备授权码
        * @param authCode 授权码
        * @return bool 激活是否成功
        * \endif
        */
        public bool ActivateAuthorization(String authCode)
        {
            IntPtr error = IntPtr.Zero;
            bool authorization = obNative.ob_device_activate_authorization(_handle.Ptr, authCode, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return authorization;
        }

        /**
        * \if English
        * @brief Write authorization code
        * @param authCode  Authorization code
        * \else
        * @brief 写入设备授权码
        * @param authCode 授权码
        * \endif
        */
        public void WriteAuthorizationCode(String authCode)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_write_authorization_code(_handle.Ptr, authCode, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        // public CameraParamList GetCameraParamList()
        // {
        //     IntPtr error = IntPtr.Zero;
        //     IntPtr handle = obNative.ob_device_get_calibration_camera_param_list(_handle.Ptr, out error);
        //     if(error != IntPtr.Zero)
        //     {
        //         throw new NativeException(new Error(error));
        //     }
        //     return new CameraParamList(handle);
        // }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_device(handle, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}