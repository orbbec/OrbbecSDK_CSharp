using System;

namespace Orbbec
{    
    public class StreamProfile : IDisposable
    {
        private NativeHandle _handle;

        internal StreamProfile(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        internal NativeHandle GetNativeHandle()
        {
            return _handle;
        }

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
}