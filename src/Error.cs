using System;
using System.Runtime.InteropServices;

namespace Orbbec
{
    public class Error : IDisposable
    {
        private NativeHandle _handle;

        internal Error(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        public String GetMessage()
        {
            IntPtr ptr = obNative.ob_error_message(_handle.Ptr);
            return Marshal.PtrToStringAnsi(ptr);
        }

        public String GetFunction()
        {
            IntPtr ptr = obNative.ob_error_function(_handle.Ptr);
            return Marshal.PtrToStringAnsi(ptr);
        }

        public String GetArgs()
        {
            IntPtr ptr = obNative.ob_error_args(_handle.Ptr);
            return Marshal.PtrToStringAnsi(ptr);
        }

        public ExceptionType GetExceptionType()
        {
            return obNative.ob_error_exception_type(_handle.Ptr);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_error(handle);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}