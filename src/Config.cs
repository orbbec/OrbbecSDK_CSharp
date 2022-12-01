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
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
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

        /**
        * \if English
        * @brief Configure the stream to be enabled
        *
        * @param streamProfile Stream  configuration
        * \else
        * @brief 设置要打开的流配置
        *
        * @param streamProfile 流的配置
        * \endif
        */
        public void EnableStream(StreamProfile streamProfile)
        {
            IntPtr error;
            obNative.ob_config_enable_stream(_handle.Ptr, streamProfile.GetNativeHandle().Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Configure all streams to be enabled
        * \else
        * @brief 设置打开所有的流
        * \endif
        */
        public void EnableAllStream()
        {
            IntPtr error;
            obNative.ob_config_enable_all_stream(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Configure the stream to be disabled
        *
        * @param streamType Stream configuration
        * \else
        * @brief 设置要关闭的流配置
        *
        * @param streamType 流的配置
        * \endif
        */
        public void DisableStream(StreamType streamType)
        {
            IntPtr error;
            obNative.ob_config_disable_stream(_handle.Ptr, streamType, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Configure all streams to be disabled
        * \else
        * @brief 设置关闭所有的流
        * \endif
        */
        public void DisableAllStream()
        {
            IntPtr error;
            obNative.ob_config_disable_all_stream(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Set the alignment mode
        *
        * @param mode Align State Mode
        * \else
        * @brief 设置对齐模式
        *
        * @param mode 对齐状态模式
        * \endif
        */
        public void SetAlignMode(AlignMode mode)
        {
            IntPtr error;
            obNative.ob_config_set_align_mode(_handle.Ptr, mode, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_config(handle, out error);
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