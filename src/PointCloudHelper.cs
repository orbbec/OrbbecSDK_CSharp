using System;

namespace Orbbec
{
    public class PointCloudHelper
    {
        public static bool SavePointcloudToPly(string fileName, Frame frame, bool saveBinary, bool useMesh, float meshThreshold)
        {
            IntPtr error = IntPtr.Zero;
            bool result = obNative.ob_save_pointcloud_to_ply(fileName, frame == null ? IntPtr.Zero : frame.GetNativeHandle().Ptr,
                saveBinary, useMesh, meshThreshold, ref error);
            NativeException.HandleError(error);
            return result;
        }
    }
}