using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Orbbec
{
    public class Config : IDisposable
    {
        private NativeHandle _handle;

        public Config()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_create_config(out error);
            _handle = new NativeHandle(handle, Delete);
        }

        internal Config(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        internal NativeHandle GetNativeHandle()
        {
            return _handle;
        }

        public void EnableStream(StreamProfile streamProfile)
        {
            IntPtr error;
            obNative.ob_config_enable_stream(_handle.Ptr, streamProfile.GetNativeHandle().Ptr, out error);
        }

        public void EnableAllStream()
        {
            IntPtr error;
            obNative.ob_config_enable_all_stream(_handle.Ptr, out error);
        }

        public void EnableStream(StreamType streamType)
        {
            IntPtr error;
            obNative.ob_config_disable_stream(_handle.Ptr, streamType, out error);
        }

        public void DisableAllStream()
        {
            IntPtr error;
            obNative.ob_config_disable_all_stream(_handle.Ptr, out error);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_config(handle, out error);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}