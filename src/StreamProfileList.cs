using System;

namespace Orbbec
{    
    public class StreamProfileList : IDisposable
    {
        private NativeHandle _handle;

        internal StreamProfileList(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        public VideoStreamProfile GetVideoStreamProfile(int width, int height, Format format, int fps)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_stream_profile_list_get_video_stream_profile(_handle.Ptr, width, height, format, fps, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new VideoStreamProfile(handle);
        }

        public StreamProfile GetProfile(int index)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_stream_profile_list_get_profile(_handle.Ptr, index, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new StreamProfile(handle);
        }

        public UInt32 ProfileCount()
        {
            IntPtr error = IntPtr.Zero;
            UInt32 count = obNative.ob_stream_profile_list_count(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return count;
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_stream_profile_list(handle, out error);
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