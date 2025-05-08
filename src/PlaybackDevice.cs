using System;

namespace Orbbec
{
    public delegate void PlaybackStatusChangedCallback(PlaybackStatus status);

    public class PlaybackDevice : Device
    {
        private PlaybackStatusChangedCallback _playbackStatusChangedCallback;
        private NativePlaybackStatusChangedCallback _nativePlaybackStatusChangedCallback;

        private void OnPlaybackStatusChanged(PlaybackStatus status, IntPtr userData)
        {
            if (_playbackStatusChangedCallback != null)
            {
                _playbackStatusChangedCallback(status);
            }
        }

        public PlaybackDevice(string filePath) : base(CreateHandle(filePath))
        {
            _nativePlaybackStatusChangedCallback = new NativePlaybackStatusChangedCallback(OnPlaybackStatusChanged);
        }

        /**
         * \if English
         * @brief Create a playback device for the specified file path
         *
         * @param filePath The file path to playback from
         * @return handle pointer to the newly created playback device
         * \else
         * @brief 根据指定的文件路径创建回放设备
         *
         * @param filePath 要回放的文件路径
         * @return 指向新创建的回放设备的句柄指针
         * \endif
         */
        private static IntPtr CreateHandle(string filePath)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_playback_device(filePath, ref error);
            NativeException.HandleError(error);
            return handle;
        }

        /**
         * \if English
         * @brief Pause playback on the specified playback device
         * \else
         * @brief 暂停指定播放设备上的播放
         * \endif
         */
        public void Pause()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_playback_device_pause(_handle.Ptr, ref error);
            NativeException.HandleError(error);
        }

        /**
         * \if English
         * @brief Resume playback on the specified playback device
         * \else
         * @brief 在指定的播放设备上恢复播放
         * \endif
         */
        public void Resume()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_playback_device_resume(_handle.Ptr, ref error);
            NativeException.HandleError(error);
        }

        /**
         * \if English
         * @brief Set the playback to a specified time point of the played data
         *
         * @param timestamp The position to set the playback to, in milliseconds
         * \else
         * @brief 将回放设备设置为指定时间点
         *
         * @param timestamp 要设置的回放位置，以毫秒为单位
         * \endif
         */
        public void Seek(UInt64 timestamp)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_playback_device_seek(_handle.Ptr, timestamp, ref error);
            NativeException.HandleError(error);
        }

        /**
        * \if English
        * @brief Set the playback to a specified time point of the played data
        *
        * @param rate The playback rate to set
        * \else
        * @brief 将回放设备设置为指定时间点
        *
        * @param rate 要设置的回放速率
        * \endif
        */
        public void SetPlaybackRate(float rate)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_playback_device_set_playback_rate(_handle.Ptr, rate, ref error);
            NativeException.HandleError(error);
        }

        /**
        * \if English
        * @brief Get the current playback status of the played data
        *
        * @return The current playback status of the played data
        * \else
        * @brief 获取当前播放设备上的播放状态
        *
        * @return 当前播放设备上的播放状态
        * \endif
        */
        public PlaybackStatus GetCurrentPlaybackStatus()
        {
            IntPtr error = IntPtr.Zero;
            PlaybackStatus status;
            obNative.ob_playback_device_get_current_playback_status(out status, _handle.Ptr, ref error); ;
            NativeException.HandleError(error);
            return status;
        }

        /**
        * \if English
        * @brief Set a callback function to be called when the playback status changes
        *
        * @param callback The callback function to set
        * \else
        * @brief 设置播放状态改变时的回调函数
        *
        * @param callback 要设置的回调函数
        * \endif
        */
        public void SetPlaybackStatusChangeCallback(PlaybackStatusChangedCallback callback)
        {
            _playbackStatusChangedCallback = callback;
            IntPtr error = IntPtr.Zero;
            obNative.ob_playback_device_set_playback_status_changed_callback(_handle.Ptr, _nativePlaybackStatusChangedCallback, IntPtr.Zero, ref error);
            NativeException.HandleError(error);
        }

        /**
        * \if English
        * @brief Get the current playback position of the played data
        *
        * @return The current playback position of the played data, in milliseconds
        * \else
        * @brief 获取当前播放设备上的播放位置
        *
        * @return 当前播放设备上的播放位置，以毫秒为单位
        * \endif
        */
        public UInt64 GetPosition()
        {
            IntPtr error = IntPtr.Zero;
            UInt64 position = obNative.ob_playback_device_get_position(_handle.Ptr, ref error);
            NativeException.HandleError(error);
            return position;
        }

        /**
        * \if English
        * @brief Get the duration of the played data
        *
        * @return The duration of the played data, in milliseconds
        * \else
        * @brief 获取回放设备的总时长
        *
        * @return 回放设备的总时长，以毫秒为单位
        * \endif
        */
        public UInt64 GetDuration()
        {
            IntPtr error = IntPtr.Zero;
            UInt64 duration = obNative.ob_playback_device_get_duration(_handle.Ptr, ref error);
            NativeException.HandleError(error);
            return duration;
        }
    }
}