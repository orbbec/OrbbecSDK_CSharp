using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Orbbec
{
    internal delegate void FrameCallbackInternal(IntPtr framePtr, IntPtr userDataPtr);
    public delegate void FrameCallback(Frame frame);

    public class Sensor : IDisposable
    {
        private NativeHandle _handle;
        private FrameCallback _callback;
        private FrameCallbackInternal _internalCallback;

        internal Sensor(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
            _internalCallback = new FrameCallbackInternal(OnFrame);
        }

        internal NativeHandle GetNativeHandle()
        {
            return _handle;
        }

        /**
        * @brief 传感器类型
        * @return SensorType 返回传感器类型
        */
        public SensorType GetSensorType()
        {
            IntPtr error = IntPtr.Zero;
            SensorType sensorType = obNative.ob_sensor_get_type(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return sensorType;
        }

        /**
        * @brief 获取传感器的流配置列表
        * @return StreamProfileList 返回流配置列表
        */
        public StreamProfileList GetStreamProfileList()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_sensor_get_stream_profile_list(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new StreamProfileList(handle);
        }

        /**
        * @brief 开启流并设置帧数据回调
        * @param streamProfile 流的配置
        * @param callback 设置帧数据到达时的回调
        */
        public void Start(StreamProfile streamProfile, FrameCallback callback)
        {
            _callback = callback;
            IntPtr error;
            obNative.ob_sensor_start(_handle.Ptr, streamProfile.GetNativeHandle().Ptr, _internalCallback, IntPtr.Zero, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        private void OnFrame(IntPtr framePtr, IntPtr userDataPtr)
        {
            IntPtr error;
            FrameType type = obNative.ob_frame_get_type(framePtr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            Frame frame;
            switch (type)
            {
                case FrameType.OB_FRAME_COLOR:
                    frame = new ColorFrame(framePtr);
                    break;
                case FrameType.OB_FRAME_DEPTH:
                    frame = new DepthFrame(framePtr);
                    break;
                case FrameType.OB_FRAME_IR:
                    frame = new IRFrame(framePtr);
                    break;
                default:
                    throw new Exception(string.Format("Unknown frame type: {0}", type));
            }
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
        * @brief 停止流
        */
        public void Stop()
        {
            IntPtr error;
            obNative.ob_sensor_stop(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_sensor(handle, out error);
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