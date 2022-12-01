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

        /**
        * \if English
        * @brief filter reset, free the internal cache, stop the processing thread and clear the pending buffer frame when asynchronous processing
        * \else
        * @brief filter重置，释放内部缓存，异步处理时停止处理线程并清空待处理的缓存帧
        * \endif
        */
        public void Reset()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_filter_reset(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Processing frames (synchronous interface)
        *
        * @param frame frame to be processed
        * @return Frame processed frame
        * \else
        * @brief 处理帧（同步接口）
        *
        * @param frame 需要处理的frame
        * @return Frame 处理后的frame
        * \endif
        */
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

        /**
        * \if English
        * @brief Set the callback function (asynchronous callback interface)
        *
        * @param callback Processing result callback
        * \else
        * @brief 设置回调函数（异步回调接口）
        *
        * @param callback 处理结果回调
        * \endif
        */
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

        /**
        * \if English
        * @brief Push the pending frame into the cache (asynchronous callback interface)
        *
        * @param frame The pending frame processing result is returned by the callback function
        * \else
        * @brief 压入待处理frame到缓存（异步回调接口）
        *
        * @param frame 待处理的frame处理结果通过回调函数返回
        * \endif
        */
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

        /**
        * \if English
        * @brief Set camera parameters
        *
        * @param param Camera internal and external parameters
        * \else
        * @brief 设置相机参数
        *
        * @param param 相机内外参数
        * \endif
        */
        public void SetCameraParam(CameraParam cameraParam)
        {
            IntPtr error;
            obNative.ob_pointcloud_filter_set_camera_param(_handle.Ptr, cameraParam, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Set point cloud type parameters
        *
        * @param type Point cloud type depth point cloud or RGBD point cloud
        * \else
        * @brief 设置点云类型参数
        *
        * @param type 点云类型深度点云或RGBD点云
        * \endif
        */
        public void SetPointFormat(Format format)
        {
            IntPtr error;
            obNative.ob_pointcloud_filter_set_point_format(_handle.Ptr, format, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief  Set the frame alignment state that will be input to generate point cloud (it needs to be enabled in D2C mode, as the basis for the algorithm to
        * select the set of camera internal parameters)
        *
        * @param state Alignment status, True: enable alignment; False: disable alignment
        * \else
        * @brief  设置将要输入的用于生成点云的帧对齐状态（D2C模式下需要开启，作为算法选用那组相机内参的依据）
        *
        * @param state 对齐状态，True：开启对齐； False：关闭对齐
        * \endif
        */
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

        /**
        * \if English
        * @brief Set format conversion type
        *
        * @param type Format conversion type
        * \else
        * @brief 设置格式转化类型
        *
        * @param format 格式转化类型
        * \endif
        */
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