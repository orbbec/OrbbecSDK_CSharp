using System;

namespace Orbbec
{    
    public class StreamProfile : IDisposable
    {
        protected NativeHandle _handle;

        internal StreamProfile(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        internal NativeHandle GetNativeHandle()
        {
            return _handle;
        }

        public T As<T>() where T : StreamProfile {
            switch (GetStreamType())
            {
                case StreamType.OB_STREAM_VIDEO:
                case StreamType.OB_STREAM_IR:
                case StreamType.OB_STREAM_COLOR:
                case StreamType.OB_STREAM_DEPTH:
                    _handle.Retain();
                    return new VideoStreamProfile(_handle.Ptr) as T;
                case StreamType.OB_STREAM_ACCEL:
                    _handle.Retain();
                    return new AccelStreamProfile(_handle.Ptr) as T;
                case StreamType.OB_STREAM_GYRO:
                    _handle.Retain();
                    return new GyroStreamProfile(_handle.Ptr) as T;   
            }
            return null;
        }

        /**
        * @brief 获取流的格式
        * @return Format 返回流的格式
        */
        public Format GetFormat()
        {
            IntPtr error;
            Format format = obNative.ob_stream_profile_format(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return format;
        }

        /**
        * @brief 获取流的类型
        * @return StreamType 返回流的类型
        */
        public StreamType GetStreamType()
        {
            IntPtr error;
            StreamType streamType = obNative.ob_stream_profile_type(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return streamType;
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_stream_profile(handle, out error);
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

    public class VideoStreamProfile : StreamProfile
    {
        internal VideoStreamProfile(IntPtr handle) : base(handle)
        {   
        }

        /**
        * @brief 获取流的帧率
        * @return UInt32 返回流的帧率
        */
        public UInt32 GetFPS()
        {
            IntPtr error;
            UInt32 fps = obNative.ob_video_stream_profile_fps(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return fps;
        }

        /**
        * @brief 获取流的宽
        * @return UInt32 返回流的宽
        */
        public UInt32 GetWidth()
        {
            IntPtr error;
            UInt32 width = obNative.ob_video_stream_profile_width(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return width;
        }

        /**
        * @brief 获取流的高
        * @return UInt32 返回流的高
        */
        public UInt32 GetHeight()
        {
            IntPtr error;
            UInt32 height = obNative.ob_video_stream_profile_height(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return height;
        } 
    }

    public class AccelStreamProfile : StreamProfile
    {
        internal AccelStreamProfile(IntPtr handle) : base(handle)
        {   
        }

        /**
        * @brief 获取加速计满量程范围
        * @return AccelFullScaleRange  返回量程范围值
        */
        public AccelFullScaleRange GetAccelFullScaleRange()
        {
            IntPtr error;
            AccelFullScaleRange accelFullScaleRange = obNative.ob_accel_stream_profile_full_scale_range(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return accelFullScaleRange;
        } 

        /**
        * @brief 获取加速计采样频率
        * @return AccelSampleRate  返回采样频率
        */
        public AccelSampleRate GetAccelSampleRate()
        {
            IntPtr error;
            AccelSampleRate accelSampleRate = obNative.ob_accel_stream_profile_sample_rate(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return accelSampleRate;
        } 
    }

    public class GyroStreamProfile : StreamProfile
    {
        internal GyroStreamProfile(IntPtr handle) : base(handle)
        {   
        }

        /**
        * @brief 获取陀螺仪满量程范围
        * @return GyroFullScaleRange  返回量程范围值
        */
        public GyroFullScaleRange GetGyroFullScaleRange()
        {
            IntPtr error;
            GyroFullScaleRange gyroFullScaleRange = obNative.ob_gyro_stream_profile_full_scale_range(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return gyroFullScaleRange;
        } 

        /**
        * @brief 获取陀螺仪采样频率
        * @return GyroSampleRate  返回采样频率
        */
        public GyroSampleRate GetGyroSampleRate()
        {
            IntPtr error;
            GyroSampleRate gyroSampleRate = obNative.ob_gyro_stream_profile_sample_rate(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return gyroSampleRate;
        } 
    }
}