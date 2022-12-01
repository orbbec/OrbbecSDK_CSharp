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
        * \if English
        * @brief Get device name
        *
        * @return String returns the device name
        * \else
        * @brief 获取设备名称
        *
        * @return String 返回设备名称
        * \endif
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
        * \if English
        * @brief Get the pid of the device
        *
        * @return int returns the pid of the device
        * \else
        * @brief 获取设备的pid
        *
        * @return int 返回设备的pid
        * \endif
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
        * \if English
        * @brief Get the vid of the device
        *
        * @return int returns the vid of the device
        * \else
        * @brief 获取设备的vid
        *
        * @return int 返回设备的vid
        * \endif
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
        * \if English
        * @brief Get system assigned uid for distinguishing between different devices
        *
        * @return String returns the uid of the device
        * \else
        * @brief 获取设备的uid，该uid标识设备接入os操作系统时，给当前设备分派的唯一id，用来区分不同的设备
        *
        * @return String 返回设备的uid
        * \endif
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
        * \if English
        * @brief Get the serial number of the device
        *
        * @return String returns the serial number of the device
        * \else
        * @brief 获取设备的序列号
        *
        * @return String 返回设备的序列号
        * \endif
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
        * \if English
        * @brief Get the version number of the firmware
        *
        * @return String returns the version number of the firmware
        * \else
        * @brief 获取固件的版本号
        *
        * @return String 返回固件的版本号
        * \endif
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
        * \if English
        * @brief Get usb connection type (DEPRECATED)
        *
        * @return String returns usb connection type
        * \else
        * @brief 获取usb连接类型 (废弃接口)
        *
        * @return String 返回usb连接类型
        * \endif
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