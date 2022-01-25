using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Orbbec
{
    internal delegate void FrameCallbackInternal(IntPtr framePtr, IntPtr userDataPtr);
    public delegate void FrameCallback(Frame frame);

    public class Sensor : IDisposable
    {
        private NativeHandle _handle;
        private FrameCallback _callback;
        private FrameCallbackInternal _internalCallback;

        internal Sensor(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
            _internalCallback = new FrameCallbackInternal(OnFrame);
        }

        internal NativeHandle GetNativeHandle()
        {
            return _handle;
        }

        /**
        * @brief 传感器类型
        * @return SensorType 返回传感器类型
        */
        public SensorType GetSensorType()
        {
            IntPtr error = IntPtr.Zero;
            SensorType sensorType = obNative.ob_sensor_get_type(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return sensorType;
        }

        /**
        * @brief 设置int类型的设备属性
        * @param propertyId 属性id
        * @param property 要设置的属性
        */
        public void SetIntProperty(PropertyId propertyId, Int32 property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_set_int_property(_handle.Ptr, propertyId, property, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 获取int类型的设备属性
        * @param propertyId 属性id
        * @return Int32 获取的属性数据
        */
        public Int32 GetIntProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            Int32 value = obNative.ob_sensor_get_int_property(_handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return value;
        }

        /**
        * @brief 设置float类型的设备属性
        * @param propertyId 属性id
        * @param property 要设置的属性
        */
        public void SetFloatProperty(PropertyId propertyId, float property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_set_float_property(_handle.Ptr, propertyId, property, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 获取float类型的设备属性
        * @param propertyId 属性id
        * @return float 获取的属性数据
        */
        public float GetFloatProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            float value = obNative.ob_sensor_get_float_property(_handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return value;
        }

        /**
        * @brief 设置bool类型的设备属性
        * @param propertyId 属性id
        * @param property 要设置的属性
        */
        public void SetBoolProperty(PropertyId propertyId, bool property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_set_bool_property(_handle.Ptr, propertyId, property, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 获取bool类型的设备属性
        * @param propertyId 属性id
        * @return bool 获取的属性数据
        */
        public bool GetBoolProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            bool value = obNative.ob_sensor_get_bool_property(_handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return value;
        }

        /**
        * @brief 设置结构体定义数据类型的设备属性
        * @param propertyId 属性id
        * @param data 要设置的属性数据
        * @param dataSize 要设置的属性大小
        */
        public void SetStructuredData(PropertyId propertyId, IntPtr data, UInt32 dataSize)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_set_structured_data(_handle.Ptr, propertyId, data, dataSize, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 获取结构体定义数据类型的设备属性
        * @param propertyId 属性id
        * @param data 获取的属性数据
        * @param dataSize 获取的属性大小
        */
        public void GetStructuredData(PropertyId propertyId, IntPtr data, ref UInt32 dataSize)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_get_structured_data(_handle.Ptr, propertyId, data, ref dataSize, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 判断传感器属性是否支持
        * @param propertyId 属性id
        * @return true 支持该属性
        * @return false 不支持该属性
        */
        public bool IsPropertySupported(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            bool isSupported = obNative.ob_sensor_is_property_supported(_handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return isSupported;
        }

        /**
        * @brief 获取int类型的设备属性的范围
        * @param propertyId 属性id
        * @return IntPropertyRange 属性的范围
        */
        public IntPropertyRange GetIntPropertyRange (PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            IntPropertyRange range;
            obNative.ob_sensor_get_int_property_range(out range, _handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        /**
        * @brief 获取float类型的设备属性的范围
        * @param propertyId 属性id
        * @return FloatPropertyRange 属性的范围
        */
        public FloatPropertyRange GetFloatPropertyRange (PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            FloatPropertyRange range;
            obNative.ob_sensor_get_float_property_range(out range, _handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        /**
        * @brief 获取bool类型的设备属性的范围
        * @param propertyId 属性id
        * @return BoolPropertyRange 属性的范围
        */
        public BoolPropertyRange GetBoolPropertyRange(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            BoolPropertyRange range;
            obNative.ob_sensor_get_bool_property_range(out range, _handle.Ptr, propertyId, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        /**
        * @brief 获取传感器的流配置列表
        * @return StreamProfile[] 返回流配置列表
        */
        public StreamProfile[] GetStreamProfiles()
        {
            IntPtr error;
            UInt32 count = 0;
            IntPtr handles = obNative.ob_sensor_get_stream_profiles(_handle.Ptr, ref count, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            IntPtr[] handleArray = new IntPtr[count];
            Marshal.Copy(handles, handleArray, 0, (int)count);
            List<StreamProfile> profiles = new List<StreamProfile>();
            foreach (var handle in handleArray)
            {
                StreamProfile profile = new StreamProfile(handle);
                profiles.Add(profile);
            }
            return profiles.ToArray();
        }

        /**
        * @brief 开启流并设置帧数据回调
        * @param streamProfile 流的配置
        * @param callback 设置帧数据到达时的回调
        */
        public void Start(StreamProfile streamProfile, FrameCallback callback)
        {
            _callback = callback;
            IntPtr error;
            obNative.ob_sensor_start(_handle.Ptr, streamProfile.GetNativeHandle().Ptr, _internalCallback, IntPtr.Zero, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        private void OnFrame(IntPtr framePtr, IntPtr userDataPtr)
        {
            IntPtr error;
            FrameType type = obNative.ob_frame_get_type(framePtr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            Frame frame;
            switch (type)
            {
                case FrameType.OB_FRAME_COLOR:
                    frame = new ColorFrame(framePtr);
                    break;
                case FrameType.OB_FRAME_DEPTH:
                    frame = new DepthFrame(framePtr);
                    break;
                case FrameType.OB_FRAME_IR:
                    frame = new IRFrame(framePtr);
                    break;
                default:
                    throw new Exception(string.Format("Unknown frame type: {0}", type));
            }
            if(_callback != null)
            {
                _callback(frame);
            }
            else
            {
                frame.Dispose();
            }
        }

        /**
        * @brief 停止流
        */
        public void Stop()
        {
            IntPtr error;
            obNative.ob_sensor_stop(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_sensor(handle, out error);
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