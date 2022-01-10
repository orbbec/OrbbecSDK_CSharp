using System;
using System.Runtime.InteropServices;

namespace Orbbec
{    
    public class DeviceList : IDisposable
    {
        private NativeHandle _handle;

        internal DeviceList(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        public UInt32 DeviceCount()
        {
            IntPtr error = IntPtr.Zero;
            UInt32 count = obNative.ob_device_list_count(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return count;
        }

        public String Name(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr ptr = obNative.ob_device_list_get_device_name(_handle.Ptr, index, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return Marshal.PtrToStringAnsi(ptr);
        }

        public int Pid(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            int pid = obNative.ob_device_list_get_device_pid(_handle.Ptr, index, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return pid;
        }

        public int Vid(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            int vid = obNative.ob_device_list_get_device_vid(_handle.Ptr, index, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return vid;
        }

        public String Uid(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr ptr = obNative.ob_device_list_get_device_uid(_handle.Ptr, index, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return Marshal.PtrToStringAnsi(ptr);
        }

        public Device GetDevice(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_get_device(_handle.Ptr, index, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new Device(handle);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_device_list(handle, out error);
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