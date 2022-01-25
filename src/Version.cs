using System;

namespace Orbbec
{
    public class Version
    {
        /**
        * @brief 获取SDK版本号
        * @return int 返回SDK版本号
        */
        public static int GetVersion()
        {
            return obNative.ob_get_version();
        }

        /**
        * @brief 获取SDK主版本号
        * @return int 返回SDK主版本号
        */
        public static int GetMajorVersion()
        {
            return obNative.ob_get_major_version();
        }

        /**
        * @brief 获取SDK副版本号
        * @return int 返回SDK副版本号
        */
        public static int GetMinorVersion()
        {
            return obNative.ob_get_minor_version();
        }

        /**
        * @brief 获取SDK修订版本号
        * @return int 返回SDK修订版本号
        */
        public static int GetPatchVersion()
        {
            return obNative.ob_get_patch_version();
        }
    }
}