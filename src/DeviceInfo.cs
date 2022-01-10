using System;
using System.Runtime.InteropServices;

namespace Orbbec
{
    public class DeviceInfo : IDisposable
    {
        private NativeHandle _handle;

        internal DeviceInfo(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        public String Name()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr ptr = obNative.ob_device_info_name(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return Marshal.PtrToStringAnsi(ptr);
        }

        public int Pid()
        {
            IntPtr error = IntPtr.Zero;
            int pid = obNative.ob_device_info_pid(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return pid;
        }

        public int Vid()
        {
            IntPtr error = IntPtr.Zero;
            int vid = obNative.ob_device_info_vid(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return vid;
        }

        public String Uid()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr ptr = obNative.ob_device_info_uid(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return Marshal.PtrToStringAnsi(ptr);
        }

        public String SerialNumber()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr ptr = obNative.ob_device_info_serial_number(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return Marshal.PtrToStringAnsi(ptr);
        }

        public String FirmwareVersion()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr ptr = obNative.ob_device_info_firmware_version(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return Marshal.PtrToStringAnsi(ptr);
        }

        public String UsbType()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr ptr = obNative.ob_device_info_usb_type(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return Marshal.PtrToStringAnsi(ptr);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_device_info(handle, out error);
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