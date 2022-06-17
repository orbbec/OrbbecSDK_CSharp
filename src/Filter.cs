using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Orbbec
{
    internal delegate void FilterCallbackInternal(IntPtr framePtr, IntPtr userDataPtr);
    public delegate void FilterCallback(Frame frame);

    public class Filter : IDisposable
    {
        protected NativeHandle _handle;
        private FilterCallback _callback;
        private FilterCallbackInternal _internalCallback;

        internal Filter(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
            _internalCallback = new FilterCallbackInternal(OnFrame);
        }

        public void Reset()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_filter_reset(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public Frame Process(Frame frame)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_filter_process(_handle.Ptr, frame.GetNativeHandle().Ptr, out error);
            if(handle == IntPtr.Zero)
            {
                return null;
            }
            return new Frame(handle);
        }

        public void SetCallback(FilterCallback callback)
        {
            _callback = callback;
            IntPtr error;
            obNative.ob_filter_set_callback(_handle.Ptr, _internalCallback, IntPtr.Zero, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void PushFrame(Frame frame)
        {
            IntPtr error;
            obNative.ob_filter_push_frame(_handle.Ptr, frame.GetNativeHandle().Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        private void OnFrame(IntPtr framePtr, IntPtr userDataPtr)
        {
            Frame frame = new Frame(framePtr);
            if(_callback != null)
            {
                _callback(frame);
            }
            else
            {
                frame.Dispose();
            }
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_filter(handle, out error);
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