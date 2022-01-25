using System;
using System.Runtime.InteropServices;

namespace Orbbec
{
    public class Frame : IDisposable
    {
        protected NativeHandle _handle;

        internal Frame(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        /**
        * @brief 获取帧的序号
        *
        * @return UInt64 返回帧的序号
        */
        public UInt64 GetIndex()
        {
            IntPtr error = IntPtr.Zero;
            UInt64 index = obNative.ob_frame_index(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return index;
        }

        /**
        * @brief 获取帧的格式
        *
        * @return Format 返回帧的格式
        */
        public Format GetFormat()
        {
            IntPtr error = IntPtr.Zero;
            Format format = obNative.ob_frame_format(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return format;
        }

        /**
        * @brief 获取帧的类型
        *
        * @return FrameType 返回帧的类型
        */
        public FrameType GetFrameType()
        {
            IntPtr error = IntPtr.Zero;
            FrameType frameType = obNative.ob_frame_get_type(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return frameType;
        }

        /**
        * @brief 获取帧的硬件时间戳
        * @return UInt64 返回帧硬件的时间戳
        */
        public UInt64 GetTimeStamp()
        {
            IntPtr error = IntPtr.Zero;
            UInt64 timestamp = obNative.ob_frame_time_stamp(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return timestamp;
        }

        /**
        * @brief 获取帧的系统时间戳
        * @return UInt64 返回帧的系统时间戳
        */
        public UInt64 GetSystemTimeStamp()
        {
            IntPtr error = IntPtr.Zero;
            UInt64 sysTimestamp = obNative.ob_frame_system_time_stamp(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return sysTimestamp;
        }

        /**
        * @brief 获取帧数据
        * @param data 获取到的帧数据
        */
        public void CopyData(ref Byte[] data)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr dataPtr = obNative.ob_frame_data(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            Marshal.Copy(dataPtr, data, 0, data.Length);
        }

        public IntPtr GetDataPtr()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr dataPtr = obNative.ob_frame_data(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return dataPtr; 
        }

        /**
        * @brief 获取帧数据大小
        * @return UInt32 返回帧数据大小
        */
        public UInt32 GetDataSize()
        {
            IntPtr error = IntPtr.Zero;
            UInt32 dataSize = obNative.ob_frame_data_size(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return dataSize;
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_frame(handle, out error);
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

    public class VideoFrame : Frame
    {
        internal VideoFrame(IntPtr handle) : base(handle)
        {
        }

        /**
        * @brief 获取帧的宽
        * @return UInt32 返回帧的宽
        */
        public UInt32 GetWidth()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_video_frame_width(_handle.Ptr, out error);
        }

        /**
        * @brief 获取帧的高
        * @return UInt32 返回帧的高
        */
        public UInt32 GetHeight()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_video_frame_height(_handle.Ptr, out error);
        }

        /**
        * @brief 获取帧的元数据
        * @return Byte[] 返回帧的元数据
        */
        public Byte[] GetMetadata()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr data = obNative.ob_frame_metadata(_handle.Ptr, out error);
            UInt32 dataSize = obNative.ob_frame_metadata_size(_handle.Ptr, out error);
            Byte[] buffer = new Byte[dataSize];
            Marshal.Copy(data, buffer, 0, (int)dataSize);
            return buffer;
        }

        /**
        * @brief 获取帧的元数据大小
        * @return UInt32 返回帧的元数据大小
        */
        public UInt32 GetMetadataSize()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_metadata_size(_handle.Ptr, out error);
        }
    }

    public class ColorFrame : VideoFrame
    {
        internal ColorFrame(IntPtr handle) : base(handle)
        {
        }
    }

    public class DepthFrame : VideoFrame
    {
        internal DepthFrame(IntPtr handle) : base(handle)
        {
        }

        /**
        * @brief 获取深度帧的值刻度，单位为 mm/step，
        *      如valueScale=0.1, 某坐标像素值为pixelValue=10000，
        *     则表示深度值value = pixelValue*valueScale = 10000*0.1=1000mm。
        * @return float
        */
        public float GetValueScale()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_depth_frame_get_value_scale(_handle.Ptr, out error);
        }
    }

    public class IRFrame : VideoFrame
    {
        internal IRFrame(IntPtr handle) : base(handle)
        {
        }
    }

    public class PointsFrame : Frame
    {
        internal PointsFrame(IntPtr handle) : base(handle)
        {
        }
    }

    public class AccelFrame : Frame
    {
        internal AccelFrame(IntPtr handle) : base(handle)
        {
        }

        /**
        * @brief 获取加速度帧X数据
        * @return AccelValue
        */
        public AccelValue GetAccelValue()
        {
            IntPtr error = IntPtr.Zero;
            AccelValue accelValue;
            obNative.ob_accel_frame_value(out accelValue, _handle.Ptr, out error);
            return accelValue;
        }

        /**
        * @brief 获取帧采样时的温度
        * @return float 温度值
        */
        public float GetTemperature()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_accel_frame_temperature(_handle.Ptr, out error);
        }
    }

    public class GyroFrame : Frame
    {
        internal GyroFrame(IntPtr handle) : base(handle)
        {
        }

        /**
        * @brief 获取陀螺仪帧数据
        * @return GyroValue
        */
        public GyroValue GetGyroValue()
        {
            IntPtr error = IntPtr.Zero;
            GyroValue gyroValue;
            obNative.ob_gyro_frame_value(out gyroValue, _handle.Ptr, out error);
            return gyroValue;
        }

        /**
        * @brief 获取帧采样时的温度
        * @return float 温度值
        */
        public float GetTemperature()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_gyro_frame_temperature(_handle.Ptr, out error);
        }
    }

    public class Frameset : Frame
    {
        internal Frameset(IntPtr handle) : base(handle)
        {
        }

        /**
        * @brief 帧集合中包含的帧数量
        * @return UInt32 返回帧的数量
        */
        public UInt32 GetFrameCount()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_set_frame_count(_handle.Ptr, out error);
        }

        /**
        * @brief 获取深度帧
        * @return DepthFrame 返回深度帧
        */
        public DepthFrame GetDepthFrame()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_frame_set_depth_frame(_handle.Ptr, out error);
            if(handle == IntPtr.Zero)
            {
                return null;
            }
            return new DepthFrame(handle);
        }

        /**
        * @brief 获取彩色帧
        * @return ColorFrame 返回彩色帧
        */
        public ColorFrame GetColorFrame()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_frame_set_color_frame(_handle.Ptr, out error);
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            return new ColorFrame(handle);
        }

        /**
        * @brief 获取红外帧
        * @return IRFrame 返回红外帧
        */
        public IRFrame GetIRFrame()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_frame_set_ir_frame(_handle.Ptr, out error);
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            return new IRFrame(handle);
        }

        /**
        * @brief 获取点云帧
        * @return PointsFrame 返回点云据帧
        */
        public PointsFrame GetPointsFrame()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_frame_set_points_frame(_handle.Ptr, out error);
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            return new PointsFrame(handle);
        }

        /**
        * @brief 通过传感器类型获取帧
        * @param sensorType 传感器的类型
        * @return Frame 返回相应类型的帧
        */
        public Frame GetFrame(SensorType sensorType)
        {
            throw new NotImplementedException();
        }
    }
}