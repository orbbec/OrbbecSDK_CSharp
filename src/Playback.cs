using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Orbbec
{
    internal delegate void PlaybackCallbackInternal(IntPtr framePtr, IntPtr userDataPtr);
    public delegate void PlaybackCallback(Frame frame);

    public class Playback : IDisposable
    {
        private NativeHandle _handle;
        private PlaybackCallback _callback;
        private PlaybackCallbackInternal _internalCallback;

        public Playback(String fileName)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_playback(fileName, out error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);

            _internalCallback = new PlaybackCallbackInternal(OnFrame);
        }

        /**
        * \if English
        * @brief Start playback, playback data is returned from the callback
        *
        * @param callback Callback for playback data
        * @param user_data User data
        * @param type Type of playback data
        * \else
        * @brief 开启回放，回放数据从回调中返回
        *
        * @param callback 回放数据的回调
        * @param user_data 用户数据
        * @param type 回放数据的类型
        * \endif
        */
        public void Start(PlaybackCallback callback, MediaType mediaType)
        {
            _callback = callback;
            IntPtr error;
            obNative.ob_playback_start(_handle.Ptr, _internalCallback, IntPtr.Zero, mediaType, out error);
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

        /**
        * \if English
        * @brief stop playback
        * \else
        * @brief 停止回放
        * \endif
        */
        public void Stop()
        {
            IntPtr error;
            obNative.ob_playback_stop(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Get the device information in the recording file
        *
        * @return DeviceInfo returns device information
        * \else
        * @brief 获取录制文件内的设备信息
        *
        * @return DeviceInfo returns device information
        * \endif
        */
        public DeviceInfo GetDeviceInfo()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_playback_get_device_info(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new DeviceInfo(handle);
        }

        public CameraParam GetCameraParam()
        {
            IntPtr error;
            CameraParam cameraParam;
            obNative.ob_playback_get_camera_param(out cameraParam, _handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return cameraParam;
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_playback(handle, out error);
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