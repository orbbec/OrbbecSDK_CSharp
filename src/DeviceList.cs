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

        /**
        * @brief 获取设备数量
        *
        * @return UInt32 返回设备的数量
        */
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

        /**
        * @brief 获取指定设备的名称
        *
        * @param index 设备索引
        * @return String 返回设备的名称
        */
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

        /**
        * @brief 获取指定设备的pid
        *
        * @param index 设备索引
        * @return int 返回设备的pid
        */
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

        /**
        * @brief 获取指定设备的vid
        *
        * @param index 设备索引
        * @return int 返回设备的vid
        */
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

        /**
        * @brief 获取指定设备的uid
        *
        * @param index 设备索引
        * @return String 返回设备的uid
        */
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

        /**
        * @brief 从设备列表中获取指定设备对象,
        * @attention 如果设备有在其他地方被获取创建，重复获取将会抛异常
        * @param index 要创建设备的索引
        * @return Device 返回设备对象
        */
        public Device GetDevice(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_device_list_get_device(_handle.Ptr, index, out error);
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