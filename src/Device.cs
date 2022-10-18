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
        * @brief 获取设备信息
        *
        * @return DeviceInfo 返回设备的信息
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
        * @brief 获取设备传感器列表
        *
        * @return SensorList 返回传感器列表
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
        * @brief 获取指定类型传感器
        * 如果设备没有打开传感器，在SDK内部会自动打开设备并返回实例
        *
        * @return Sensor 返回传感器示例
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
        * @brief 设置int类型的设备属性
        *
        * @param propertyId 属性id
        * @param property 要设置的属性
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
        * @brief 获取int类型的设备属性
        *
        * @param propertyId 属性id
        * @return int32_t 获取的属性数据
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
        * @brief 设置float类型的设备属性
        *
        * @param propertyId 属性id
        * @param property 要设置的属性
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
        * @brief 获取float类型的设备属性
        *
        * @param propertyId 属性id
        * @return float 获取的属性数据
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
        * @brief 设置bool类型的设备属性
        *
        * @param propertyId 属性id
        * @param property 要设置的属性
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
        * @brief 获取bool类型的设备属性
        *
        * @param propertyId 属性id
        * @return bool 获取的属性数据
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
        * @brief 设置结构体类型的设备属性
        *
        * @param propertyId 属性id
        * @param data 要设置的属性数据
        * @param dataSize 要设置的属性大小
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
        * @brief 获取结构体类型的设备属性
        *
        * @param propertyId 属性id
        * @param data 获取的属性数据
        * @param dataSize 获取的属性大小
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
        * @brief 判断设备的属性是否支持
        *
        * @param propertyId 属性id
        * @return true 支持该属性
        * @return false 不支持该属性
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
        * @brief 获取int类型的设备属性的范围(包括当前值和默认值)
        *
        * @param propertyId 属性id
        * @return IntPropertyRange 属性的范围
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
        * @brief 获取float类型的设备属性的范围(包括当前值和默认值)
        *
        * @param propertyId 属性id
        * @return FloatPropertyRange 属性的范围
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
        * @brief 获取bool类型的设备属性的范围(包括当前值和默认值)
        *
        * @param propertyId 属性id
        * @return BoolPropertyRange 属性的范围
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
        * @brief 验证设备授权码
        * @param authCode 授权码
        * @return bool 激活是否成功
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
        * @brief 写入设备授权码
        * @param authCode 授权码
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