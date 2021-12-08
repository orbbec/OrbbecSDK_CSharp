using System;

namespace Orbbec
{
    public class Version
    {
        public static int GetVersion()
        {
            return obNative.ob_get_version();
        }

        public static int GetMajorVersion()
        {
            return obNative.ob_get_major_version();
        }

        public static int GetMinorVersion()
        {
            return obNative.ob_get_minor_version();
        }

        public static int GetPatchVersion()
        {
            return obNative.ob_get_patch_version();
        }
    }
}