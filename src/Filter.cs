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

        internal Filter()
        {
            _internalCallback = new FilterCallbackInternal(OnFrame);
        }

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

    public class PointCloudFilter : Filter
    {
        public PointCloudFilter()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_create_pointcloud_filter(out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public void SetCameraParam(CameraParam cameraParam)
        {
            IntPtr error;
            obNative.ob_pointcloud_filter_set_camera_param(_handle.Ptr, cameraParam, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void SetPointFormat(Format format)
        {
            IntPtr error;
            obNative.ob_pointcloud_filter_set_point_format(_handle.Ptr, format, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void SetAlignState(bool state)
        {
            IntPtr error;
            obNative.ob_pointcloud_filter_set_frame_align_state(_handle.Ptr, state, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class FormatConvertFilter : Filter
    {
        public FormatConvertFilter()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_create_format_convert_filter(out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public void SetConvertFormat(ConvertFormat format)
        {
            IntPtr error;
            obNative.ob_format_convert_filter_set_format(_handle.Ptr, format, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }
}