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

        internal Sensor(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        internal NativeHandle GetNativeHandle()
        {
            return _handle;
        }

        public SensorType GetSensorType()
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_sensor_get_type(_handle.Ptr, out error);
        }

        public void SetIntProperty(PropertyId propertyId, Int32 property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_set_int_property(_handle.Ptr, propertyId, property, out error);
        }

        public Int32 GetIntProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_sensor_get_int_property(_handle.Ptr, propertyId, out error);
        }

        public void SetFloatProperty(PropertyId propertyId, float property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_set_float_property(_handle.Ptr, propertyId, property, out error);
        }

        public float GetFloatProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_sensor_get_float_property(_handle.Ptr, propertyId, out error);
        }

        public void SetBoolProperty(PropertyId propertyId, bool property)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_set_bool_property(_handle.Ptr, propertyId, property, out error);
        }

        public bool GetBoolProperty(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_sensor_get_bool_property(_handle.Ptr, propertyId, out error);
        }

        public void SetStructuredData(PropertyId propertyId, IntPtr data, UInt32 dataSize)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_set_structured_data(_handle.Ptr, propertyId, data, dataSize, out error);
        }

        public void GetStructuredData(PropertyId propertyId, IntPtr data, ref UInt32 dataSize)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sensor_get_structured_data(_handle.Ptr, propertyId, data, ref dataSize, out error);
        }

        public bool IsPropertySupported(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            return obNative.ob_sensor_is_property_supported(_handle.Ptr, propertyId, out error);
        }

        public IntPropertyRange GetIntPropertyRange (PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            IntPropertyRange range;
            obNative.ob_sensor_get_int_property_range(out range, _handle.Ptr, propertyId, out error);
            return range;
        }

        public FloatPropertyRange GetFloatPropertyRange (PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            FloatPropertyRange range;
            obNative.ob_sensor_get_float_property_range(out range, _handle.Ptr, propertyId, out error);
            return range;
        }

        public BoolPropertyRange GetBoolPropertyRange(PropertyId propertyId)
        {
            IntPtr error = IntPtr.Zero;
            BoolPropertyRange range;
            obNative.ob_sensor_get_bool_property_range(out range, _handle.Ptr, propertyId, out error);
            return range;
        }

        public StreamProfile[] GetStreamProfiles()
        {
            IntPtr error;
            UInt32 count = 0;
            IntPtr handles = obNative.ob_sensor_get_stream_profiles(_handle.Ptr, ref count, out error);
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

        public void Start(StreamProfile streamProfile, FrameCallback callback)
        {
            _callback = callback;
            IntPtr error;
            IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
            obNative.ob_sensor_start(_handle.Ptr, streamProfile.GetNativeHandle().Ptr, OnFrame, callbackPtr, out error);
        }

        private static void OnFrame(IntPtr framePtr, IntPtr userDataPtr)
        {
            FrameCallback callback = (FrameCallback)Marshal.GetDelegateForFunctionPointer(userDataPtr, typeof(FrameCallback));
            if(callback == null)
            {
                return;
            }
            IntPtr error;
            FrameType type = obNative.ob_frame_get_type(framePtr, out error);
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
            callback(frame);
        }

        public void Stop()
        {
            IntPtr error;
            obNative.ob_sensor_stop(_handle.Ptr, out error);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_sensor(handle, out error);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}