using System;

namespace Orbbec
{    
    public class SensorList : IDisposable
    {
        private NativeHandle _handle;

        internal SensorList(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        public UInt32 SensorCount()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_sensor_list_get_sensor_count(_handle.Ptr, out error);
        }

        public SensorType SensorType(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_sensor_list_get_sensor_type(_handle.Ptr, index, out error);
        }

        public Sensor GetSensor(SensorType sensorType)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_get_sensor_by_type(_handle.Ptr, sensorType, out error);
            return new Sensor(handle);
        }

        public Sensor GetSensor(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_get_sensor(_handle.Ptr, index, out error);
            return new Sensor(handle);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_sensor_list(handle, out error);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}