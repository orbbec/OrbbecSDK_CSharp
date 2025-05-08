using System;

namespace Orbbec
{
    public class RecordDevice : IDisposable
    {
        private NativeHandle _handle;

        public RecordDevice(RecordDevice other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (ReferenceEquals(this, other))
                throw new ArgumentException("Cannot move from self");

            _handle = other.TransferHandle();
        }

        /**
         * \if English
         * @brief Create a recording device for the specified device with a specified file path and compression enabled
         *
         * @param device The device to record
         * @param filePath The file path to record to
         * @param compressionEnabled Whether to enable compression for the recording
         * \else
         * @brief 为指定设备创建录制设备，指定文件路径并启用压缩功能
         *
         * @param device 要录制的设备
         * @param filePath 要录制的文件路径
         * @param compressionEnabled 是否启用压缩
         * \endif
         */
        public RecordDevice(Device device, string filePath, bool compressionEnabled = true)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_record_device(device.GetNativeHandle().Ptr, filePath, compressionEnabled, ref error);
            NativeException.HandleError(error);
            _handle = new NativeHandle(handle, Delete);
        }

        /**
         * \if English
         * @brief Pause recording on the specified recording device
         * \else
         * @brief 暂停指定录制设备的录制
         * \endif
         */
        public void Pause()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_record_device_pause(_handle.Ptr, ref error);
            NativeException.HandleError(error);
        }

        /**
         * \if English
         * @brief Resume recording on the specified recording device
         * \else
         * @brief 在指定的录制设备上恢复录制
         * \endif
         */
        public void Resume()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_record_device_resume(_handle.Ptr, ref error);
            NativeException.HandleError(error);
        }

        internal NativeHandle GetNativeHandle() => _handle;

        public NativeHandle TransferHandle()
        {
            var temp = _handle;
            _handle = null;
            return temp;
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_record_device(handle, ref error);
            NativeException.HandleError(error);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}