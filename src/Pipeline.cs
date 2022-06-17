using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Orbbec
{
    internal delegate void FramesetCallbackInternal(IntPtr framesetPtr, IntPtr userDataPtr);
    public delegate void FramesetCallback(Frameset frameset);

    public class Pipeline : IDisposable
    {
        private NativeHandle _handle;
        private Device _device;
        private FramesetCallback _callback;
        private FramesetCallbackInternal _internalCallback;

        /**
        * @brief Pipeline 是SDK的高级接口，适用于应用，算法等重点关注RGBD数据流常见，Pipeline在SDK内部可以提供对齐，同步后的FrameSet桢集合
        * 直接方便客户使用。
        * Pipeline()无参数版本，默认打开连接到OS的设备列表中的第一个设备。若应用已经通过DeviceList获取设备，此时打开Pipeline()会抛出设备已经创建异常。
        * 需要开发者捕获异常处理。
        */
        public Pipeline()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_create_pipeline(out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
            _internalCallback = new FramesetCallbackInternal(OnFrameset);
        }

        /**
        * @brief
        * 适用于多设备操作常见，此时需要通过DeviceList获取多个设备，通过该接口实现device和pipeline绑定。
        */
        public Pipeline(Device device)
        {
            _device = device;
            IntPtr error;
            IntPtr handle = obNative.ob_create_pipeline_with_device(device.GetNativeHandle().Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
            _internalCallback = new FramesetCallbackInternal(OnFrameset);
        }

        /**
        * @brief 启动pipeline
        */
        public void Start()
        {
            IntPtr error;
            obNative.ob_pipeline_start(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 启动pipeline并配置参数
        * @param config pipeline的参数配置
        */
        public void Start(Config config)
        {
            IntPtr error;
            obNative.ob_pipeline_start_with_config(_handle.Ptr, config.GetNativeHandle().Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 启动pipeline并设置帧集合数据回调
        * @param config pipeline的参数配置
        * @param callback 设置帧集合中的所有帧数据都到达时触发回调
        */
        public void Start(Config config, FramesetCallback callback)
        {
            _callback = callback;
            IntPtr error;
            obNative.ob_pipeline_start_with_callback(_handle.Ptr, config.GetNativeHandle().Ptr, _internalCallback, IntPtr.Zero, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 停止pipeline
        */
        public void Stop()
        {
            IntPtr error;
            obNative.ob_pipeline_stop(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 获取pipeline的配置参数
        * @return Config 返回配置的参数
        */
        public Config GetConfig()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_pipeline_get_config(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new Config(handle);
        }

        /**
        * @brief 等待帧集合数据
        * @param timeoutMs 等待超时时间(毫秒)
        * @return Frameset 返回等待的帧集合数据
        */
        public Frameset WaitForFrames(UInt32 timeoutMs)
        {
            IntPtr error;
            IntPtr handle = obNative.ob_pipeline_wait_for_frameset(_handle.Ptr, timeoutMs, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            if(handle == IntPtr.Zero)
            {
                return null;
            }
            return new Frameset(handle);
        }

        /**
        * @brief 获取设备对象
        * @return Device 返回设备对象
        */
        public Device GetDevice()
        {
            if(_device != null)
            {
                return _device;
            }
            IntPtr error;
            IntPtr handle = obNative.ob_pipeline_get_device(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return new Device(handle);
        }

        /**
        * @brief 获取指定传感器的流配置
        * @param sensorType 传感器的类型
        * @return StreamProfileList 返回流配置列表
        */
        public StreamProfileList GetStreamProfileList(SensorType sensorType)
        {
            IntPtr error;
            IntPtr handle = obNative.ob_pipeline_get_stream_profile_list(_handle.Ptr, sensorType, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            
            return new StreamProfileList(handle);
        }

        /**
        * @brief 打开帧同步功能
        */
        public void EnableFrameSync()
        {
            IntPtr error;
            obNative.ob_pipeline_enable_frame_sync(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * @brief 关闭帧同步功能
        */
        public void DisableFrameSync()
        {
            IntPtr error;
            obNative.ob_pipeline_disable_frame_sync(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public CameraParam GetCameraParam()
        {
            IntPtr error;
            CameraParam cameraParam;
            obNative.ob_pipeline_get_camera_param(out cameraParam, _handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return cameraParam;
        }

        public void StartRecord(String fileName)
        {
            IntPtr error;
            obNative.ob_pipeline_start_record(_handle.Ptr, fileName, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void StopRecord()
        {
            IntPtr error;
            obNative.ob_pipeline_stop_record(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_pipeline(handle, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void Dispose()
        {
            _handle.Dispose();
        }

        private void OnFrameset(IntPtr framesetPtr, IntPtr userDataPtr)
        {
            Frameset frameset = new Frameset(framesetPtr);
            if(_callback != null)
            {
                _callback(frameset);
            }
            else
            {
                frameset.Dispose();
            }
        }
    }
}