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
            return obNative.ob_stream_profile_format(_handle.Ptr, out error);
        }

        public StreamType GetStreamType()
        {
            IntPtr error;
            return obNative.ob_stream_profile_type(_handle.Ptr, out error);
        }

        public UInt32 GetFPS()
        {
            IntPtr error;
            return obNative.ob_video_stream_profile_fps(_handle.Ptr, out error);
        }

        public UInt32 GetWidth()
        {
            IntPtr error;
            return obNative.ob_video_stream_profile_width(_handle.Ptr, out error);
        }

        public UInt32 GetHeight()
        {
            IntPtr error;
            return obNative.ob_video_stream_profile_height(_handle.Ptr, out error);
        } 

        public AccelFullScaleRange GetAccelFullScaleRange()
        {
            IntPtr error;
            return obNative.ob_accel_stream_profile_full_scale_range(_handle.Ptr, out error);
        } 

        public AccelSampleRate GetAccelSampleRate()
        {
            IntPtr error;
            return obNative.ob_accel_stream_profile_sample_rate(_handle.Ptr, out error);
        } 

        public GyroFullScaleRange GetGyroFullScaleRange()
        {
            IntPtr error;
            return obNative.ob_gyro_stream_profile_full_scale_range(_handle.Ptr, out error);
        } 

        public GyroSampleRate GetGyroSampleRate()
        {
            IntPtr error;
            return obNative.ob_gyro_stream_profile_sample_rate(_handle.Ptr, out error);
        } 

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_stream_profile(handle, out error);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}