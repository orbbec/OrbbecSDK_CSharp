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
        private FramesetCallback _callback;

        public Pipeline()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_create_pipeline(out error);
            _handle = new NativeHandle(handle, Delete);
        }

        public Pipeline(Device device)
        {
            IntPtr error;
            IntPtr handle = obNative.ob_create_pipeline_with_device(device.GetNativeHandle().Ptr, out error);
            _handle = new NativeHandle(handle, Delete);
        }

        public void Start()
        {
            IntPtr error;
            obNative.ob_pipeline_start(_handle.Ptr, out error);
        }

        public void Start(Config config)
        {
            IntPtr error;
            obNative.ob_pipeline_start_with_config(_handle.Ptr, config.GetNativeHandle().Ptr, out error);
        }

        public void Start(Config config, FramesetCallback callback)
        {
            _callback = callback;
            IntPtr error;
            obNative.ob_pipeline_start_with_callback(_handle.Ptr, config.GetNativeHandle().Ptr, OnFrameset, IntPtr.Zero, out error);
        }

        public void Stop()
        {
            IntPtr error;
            obNative.ob_pipeline_stop(_handle.Ptr, out error);
        }

        public Config GetConfig()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_pipeline_get_config(_handle.Ptr, out error);
            return new Config(handle);
        }

        public Frameset WaitForFrames(UInt32 timeoutMs)
        {
            IntPtr error;
            IntPtr handle = obNative.ob_pipeline_wait_for_frames(_handle.Ptr, timeoutMs, out error);
            return new Frameset(handle);
        }

        public Device GetDevice()
        {
            IntPtr error;
            IntPtr handle = obNative.ob_pipeline_get_device(_handle.Ptr, out error);
            return new Device(handle);
        }

        public StreamProfile[] GetStreamProfiles(SensorType sensorType)
        {
            IntPtr error;
            UInt32 count = 0;
            IntPtr handles = obNative.ob_pipeline_get_stream_profiles(_handle.Ptr, sensorType, ref count, out error);
            IntPtr[] handleArray = new IntPtr[count];
            Marshal.Copy(handles, handleArray, 0, (int)count);
            List<StreamProfile> profiles = new List<StreamProfile>();
            foreach (var handle in handleArray)
            {
                StreamProfile profile = new StreamProfile(handle);
                profiles.Add(profile);
            }
            return profiles.ToArray();
        }

        public StreamProfile[] GetAllStreamProfiles()
        {
            IntPtr error;
            UInt32 count = 0;
            IntPtr handles = obNative.ob_pipeline_get_all_stream_profiles(_handle.Ptr, ref count, out error);
            IntPtr[] handleArray = new IntPtr[count];
            Marshal.Copy(handles, handleArray, 0, (int)count);
            List<StreamProfile> profiles = new List<StreamProfile>();
            foreach (var handle in handleArray)
            {
                StreamProfile profile = new StreamProfile(handle);
                profiles.Add(profile);
            }
            return profiles.ToArray();
        }

        public void EnableFrameSync()
        {
            IntPtr error;
            obNative.ob_pipeline_enable_frame_sync(_handle.Ptr, out error);
        }

        public void DisableFrameSync()
        {
            IntPtr error;
            obNative.ob_pipeline_disable_frame_sync(_handle.Ptr, out error);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_pipeline(handle, out error);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }

        private void OnFrameset(IntPtr framesetPtr, IntPtr userDataPtr)
        {
            Frameset frameset = new Frameset(framesetPtr);
            _callback(frameset);
        }
    }
}