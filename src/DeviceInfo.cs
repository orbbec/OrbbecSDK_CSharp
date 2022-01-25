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

        /**
        * @brief 获取设备名称
        *
        * @return String 返回设备名称
        */
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

        /**
        * @brief 获取设备的pid
        *
        * @return int 返回设备的pid
        */
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

        /**
        * @brief 获取设备的vid
        *
        * @return int 返回设备的vid
        */
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

        /**
        * @brief 获取设备的uid，该uid标识设备接入os操作系统时，给当前设备分派的唯一id，用来区分不同的设备
        *
        * @return String 返回设备的uid
        */
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

        /**
        * @brief 获取设备的序列号
        *
        * @return String 返回设备的序列号
        */
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

        /**
        * @brief 获取固件的版本号
        *
        * @return String 返回固件的版本号
        */
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

        /**
        * @brief 获取usb连接类型
        *
        * @return String 返回usb连接类型
        */
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