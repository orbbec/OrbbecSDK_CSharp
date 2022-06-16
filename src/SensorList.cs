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

        /**
        * @brief 获取Sensor数量
        * @return UInt32 返回Sensor的数量
        */
        public UInt32 SensorCount()
        {
            IntPtr error = IntPtr.Zero;
            UInt32 count = obNative.ob_sensor_list_get_sensor_count(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return count;
        }

        /**
        * @brief 获取指定Sensor的类型
        * @param index Sensor索引
        * @return SensorType 返回Sensor类型
        */
        public SensorType SensorType(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            SensorType sensorType = obNative.ob_sensor_list_get_sensor_type(_handle.Ptr, index, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return sensorType;
        }

        /**
        * @brief 通过Sensor类型获取Sensor
        * @param sensorType 要获取的Sensor类型
        * @return Sensor 返回Sensor对象，如果指定类型Sensor不存在，将返回空
        */
        public Sensor GetSensor(SensorType sensorType)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_sensor_list_get_sensor_by_type(_handle.Ptr, sensorType, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new Sensor(handle);
        }

        /**
        * @brief 通过索引号获取Sensor
        * @param index 要创建设备的索，范围 [0, count-1]，如果index超出范围将抛异常
        * @return Sensor 返回Sensor对象
        */
        public Sensor GetSensor(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_sensor_list_get_sensor(_handle.Ptr, index, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new Sensor(handle);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_sensor_list(handle, out error);
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