using System;
using System.Runtime.InteropServices;

namespace Orbbec
{
    public delegate void DeviceChangedCallback(DeviceList removed, DeviceList added);
    internal delegate void DeviceChangedCallbackInternal(IntPtr removedPtr, IntPtr addedPtr, IntPtr userDataPtr);

    public class Context : IDisposable
    {
        private NativeHandle _handle;
        private DeviceChangedCallback _callback;

        public Context()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_context(out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public Context(String configPath)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_context_with_config(configPath, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public DeviceList QueryDeviceList()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_query_device_list(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new DeviceList(handle);
        }

        public void SetDeviceChangedCallback(DeviceChangedCallback callback)
        {
            _callback = callback;
            IntPtr error = IntPtr.Zero;
            IntPtr callbackPtr = IntPtr.Zero;
            if(callback != null)
            {
                callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
            }
            obNative.ob_set_device_changed_callback(_handle.Ptr, OnDeviceChanged, callbackPtr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void SetLoggerServerity(LogServerity logServerity)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_set_logger_serverity(_handle.Ptr, logServerity, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void SetLoggerToFile(LogServerity logServerity, String fileName)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_set_logger_to_file(_handle.Ptr, logServerity, fileName, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void SetLoggerToConsole(LogServerity logServerity, String fileName)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_set_logger_to_console(_handle.Ptr, logServerity, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_context(handle, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void Dispose()
        {
            _handle.Dispose();
        }

        private static void OnDeviceChanged(IntPtr removedPtr, IntPtr addedPtr, IntPtr userDataPtr)
        {
            if(userDataPtr == IntPtr.Zero)
            {
                return;
            }
            DeviceChangedCallback callback = (DeviceChangedCallback)Marshal.GetDelegateForFunctionPointer(userDataPtr, typeof(DeviceChangedCallback));
            if(callback == null)
            {
                return;
            }
            DeviceList removed = new DeviceList(removedPtr);
            DeviceList added = new DeviceList(addedPtr);
            callback(removed, added);
        }
    }
}