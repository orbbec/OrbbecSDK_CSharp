using System;

namespace Orbbec
{
    // public delegate void DeviceStateCallback();

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

        public DeviceInfo GetDeviceInfo()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_device_get_device_info(_handle.Ptr, out error);
            return new DeviceInfo(handle);
        }

        public SensorList GetSensorList()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_device_get_sensor_list(_handle.Ptr, out error);
            return new SensorList(handle);
        }

        public Sensor GetSensor(SensorType sensorType)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_device_get_sensor(_handle.Ptr, sensorType, out error);
            return new Sensor(handle);
        }

        public void SetIntProperty(PropertyId propertyId, Int32 property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_set_int_property(_handle.Ptr, propertyId, property, out error);
        }

        public Int32 GetIntProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_device_get_int_property(_handle.Ptr, propertyId, out error);
        }

        public void SetFloatProperty(PropertyId propertyId, float property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_set_float_property(_handle.Ptr, propertyId, property, out error);
        }

        public float GetFloatProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_device_get_float_property(_handle.Ptr, propertyId, out error);
        }

        public void SetBoolProperty(PropertyId propertyId, bool property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_set_bool_property(_handle.Ptr, propertyId, property, out error);
        }

        public bool GetBoolProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_device_get_bool_property(_handle.Ptr, propertyId, out error);
        }

        public void SetStructuredData(PropertyId propertyId, IntPtr data, UInt32 dataSize)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_set_structured_data(_handle.Ptr, propertyId, data, dataSize, out error);
        }

        public void GetStructuredData(PropertyId propertyId, IntPtr data, ref UInt32 dataSize)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_get_structured_data(_handle.Ptr, propertyId, data, ref dataSize, out error);
        }

        public bool IsPropertySupported(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_device_is_property_supported(_handle.Ptr, propertyId, out error);
        }

        public IntPropertyRange GetIntPropertyRange (PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            IntPropertyRange range;
            obNative.ob_device_get_int_property_range(out range, _handle.Ptr, propertyId, out error);
            return range;
        }

        public FloatPropertyRange GetFloatPropertyRange (PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            FloatPropertyRange range;
            obNative.ob_device_get_float_property_range(out range, _handle.Ptr, propertyId, out error);
            return range;
        }

        public BoolPropertyRange GetBoolPropertyRange(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            BoolPropertyRange range;
            obNative.ob_device_get_bool_property_range(out range, _handle.Ptr, propertyId, out error);
            return range;
        }

        // public DeviceState GetDeviceState()
        // {
        //     IntPtr error = IntPtr.Zero;
        //     DeviceState state;
        //     obNative.ob_device_get_device_state(out state, _handle.Ptr, out error);
        //     return state;
        // }

        // public void SetDeviceStateCallback(DeviceStateCallback callback)
        // {
        //     IntPtr error = IntPtr.Zero;
        //     obNative.ob_device_state_changed(_handle.Ptr, callback, IntPtr.Zero, out error);
        // }

        public bool ActivateAuthorization(String authCode)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_device_activate_authorization(_handle.Ptr, authCode, out error);
        }

        public void WriteAuthorizationCode(String authCode)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_device_write_authorization_code(_handle.Ptr, authCode, out error);
        }

        public CameraIntrinsic GetCameraIntrinsic(SensorType sensorType)
        {
            IntPtr error = IntPtr.Zero;
            CameraIntrinsic intrinsic;
            obNative.ob_device_get_camera_intrinsic(out intrinsic, _handle.Ptr, sensorType, out error);
            return intrinsic;
        }

        public CameraDistortion GetCameraDistortion(SensorType sensorType)
        {
            IntPtr error = IntPtr.Zero;
            CameraDistortion distortion;
            obNative.ob_device_get_camera_distortion(out distortion, _handle.Ptr, sensorType, out error);
            return distortion;
        }

        public D2CTransform GetD2CTransform()
        {
            IntPtr error = IntPtr.Zero;
            D2CTransform d2CTransform;
            obNative.ob_device_get_d2c_transform(out d2CTransform, _handle.Ptr, out error);
            return d2CTransform;
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_device(handle, out error);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}