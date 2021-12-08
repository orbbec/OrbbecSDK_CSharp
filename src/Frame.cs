using System;
using System.Runtime.InteropServices;

namespace Orbbec
{
    public class Frame : IDisposable
    {
        protected NativeHandle _handle;

        internal Frame(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        public UInt64 GetIndex()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_index(_handle.Ptr, out error);
        }

        public Format GetFormat()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_format(_handle.Ptr, out error);
        }

        public FrameType GetFrameType()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_get_type(_handle.Ptr, out error);
        }

        public UInt64 GetTimeStamp()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_time_stamp(_handle.Ptr, out error);
        }

        public UInt64 GetSystemTimeStamp()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_system_time_stamp(_handle.Ptr, out error);
        }

        public void CopyData(ref Byte[] data)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr dataPtr = obNative.ob_frame_data(_handle.Ptr, out error);
            Marshal.Copy(dataPtr, data, 0, data.Length);
        }

        public IntPtr GetDataPtr()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr dataPtr = obNative.ob_frame_data(_handle.Ptr, out error);
            return dataPtr; 
        }

        public UInt32 GetDataSize()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_data_size(_handle.Ptr, out error);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_frame(handle, out error);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }

    public class VideoFrame : Frame
    {
        internal VideoFrame(IntPtr handle) : base(handle)
        {
        }

        public UInt32 GetWidth()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_video_frame_width(_handle.Ptr, out error);
        }

        public UInt32 GetHeight()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_video_frame_height(_handle.Ptr, out error);
        }

        public Byte[] GetMetadata()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr data = obNative.ob_frame_metadata(_handle.Ptr, out error);
            UInt32 dataSize = obNative.ob_frame_metadata_size(_handle.Ptr, out error);
            Byte[] buffer = new Byte[dataSize];
            Marshal.Copy(data, buffer, 0, (int)dataSize);
            return buffer;
        }

        public UInt32 GetMetadataSize()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_metadata_size(_handle.Ptr, out error);
        }
    }

    public class ColorFrame : VideoFrame
    {
        internal ColorFrame(IntPtr handle) : base(handle)
        {
        }
    }

    public class DepthFrame : VideoFrame
    {
        internal DepthFrame(IntPtr handle) : base(handle)
        {
        }

        public float GetValueScale()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_depth_frame_get_value_scale(_handle.Ptr, out error);
        }
    }

    public class IRFrame : VideoFrame
    {
        internal IRFrame(IntPtr handle) : base(handle)
        {
        }
    }

    public class PointsFrame : Frame
    {
        internal PointsFrame(IntPtr handle) : base(handle)
        {
        }
    }

    public class AccelFrame : Frame
    {
        internal AccelFrame(IntPtr handle) : base(handle)
        {
        }

        public AccelValue GetAccelValue()
        {
            IntPtr error = IntPtr.Zero;
            AccelValue accelValue;
            obNative.ob_accel_frame_value(out accelValue, _handle.Ptr, out error);
            return accelValue;
        }

        public float GetTemperature()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_accel_frame_temperature(_handle.Ptr, out error);
        }
    }

    public class GyroFrame : Frame
    {
        internal GyroFrame(IntPtr handle) : base(handle)
        {
        }

        public GyroValue GetGyroValue()
        {
            IntPtr error = IntPtr.Zero;
            GyroValue gyroValue;
            obNative.ob_gyro_frame_value(out gyroValue, _handle.Ptr, out error);
            return gyroValue;
        }

        public float GetTemperature()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_gyro_frame_temperature(_handle.Ptr, out error);
        }
    }

    public class Frameset : Frame
    {
        internal Frameset(IntPtr handle) : base(handle)
        {
        }

        public UInt32 GetFrameCount()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_frame_set_frame_count(_handle.Ptr, out error);
        }

        public DepthFrame GetDepthFrame()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_frame_set_depth_frame(_handle.Ptr, out error);
            return new DepthFrame(handle);
        }

        public ColorFrame GetColorFrame()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_frame_set_color_frame(_handle.Ptr, out error);
            return new ColorFrame(handle);
        }

        public IRFrame GetIRFrame()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_frame_set_ir_frame(_handle.Ptr, out error);
            return new IRFrame(handle);
        }

        public PointsFrame GetPointsFrame()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_frame_set_points_frame(_handle.Ptr, out error);
            return new PointsFrame(handle);
        }
    }
}