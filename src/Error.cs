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

        /**
        * @brief 获取SDK内部异常的详细错误日志。
        * @return String 返回详细错误信息
        */
        public String GetMessage()
        {
            IntPtr ptr = obNative.ob_error_message(_handle.Ptr);
            return Marshal.PtrToStringAnsi(ptr);
        }

        /**
        * @brief 获取SDK内部异常的错误接口函数名称。
        * @return String 返回出错的函数信息
        */
        public String GetFunction()
        {
            IntPtr ptr = obNative.ob_error_function(_handle.Ptr);
            return Marshal.PtrToStringAnsi(ptr);
        }

        /**
        * @brief 获取SDK内部异常的错误接口函数传入参数。
        * @return String 返回出错的参数信息
        */
        public String GetArgs()
        {
            IntPtr ptr = obNative.ob_error_args(_handle.Ptr);
            return Marshal.PtrToStringAnsi(ptr);
        }

        /**
        * @brief 获取该错误的异常类型，判断是具体哪个模块异常。
        * @return ExceptionType 返回错误类型
        */
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