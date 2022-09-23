using System;

namespace Orbbec
{    
    public class CameraParamList : IDisposable
    {
        private NativeHandle _handle;

        internal CameraParamList(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
        }

        /**
        * @brief 获取相机参数数量
        * @return UInt32 返回相机参数的数量
        */
        public UInt32 CameraParamCount()
        {
            IntPtr error = IntPtr.Zero;
            UInt32 count = obNative.ob_camera_param_list_count(_handle.Ptr, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return count;
        }

        /**
        * @brief 通过索引号获取相机参数
        * @param index 范围 [0, count-1]，如果index超出范围将抛异常
        * @return Sensor 返回相机参数对象
        */
        public CameraParam GetCameraParam(UInt32 index)
        {
            IntPtr error = IntPtr.Zero;
            CameraParam cameraParam;
            obNative.ob_camera_param_list_get_param(out cameraParam, _handle.Ptr, index, out error);
            if(error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return cameraParam;
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_camera_param_list(handle, out error);
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