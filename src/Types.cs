using System;
using System.Runtime.InteropServices;

namespace Orbbec
{
    /**
    * \if English
    * @brief the permission type of api or property
    * \else
    * @brief 接口/属性的访问权限类型
    * \endif
    */
    public enum PermissionType
    {
        OB_PERMISSION_DENY = 0, /**< \if English no permission \else 无访问权限 \endif */
        OB_PERMISSION_READ = 1, /**< \if English can read \else 可读 \endif */
        OB_PERMISSION_WRITE = 2, /**< \if English can write \else 可写 \endif */
        OB_PERMISSION_READ_WRITE = 3, /**< \if English can read and write \else 可读写 \endif */
    }

    /**
     * \if English
     * @brief error code
     * \else
     * @brief 错误码
     * \endif
     */
    public enum Status
    {
        OB_STATUS_OK = 0, /**< \if English status ok \else 状态正常 \endif */
        OB_STATUS_ERROR = 1, /**< \if English status error \else 状态异常 \endif */
    }

    /**
     * \if English
     * @brief log level, the higher the level, the stronger the log filter
     * \else
     * @brief log等级, 等级越高Log过滤力度越大
     * \endif
     */
    public enum LogSeverity
    {
        OB_LOG_SEVERITY_DEBUG, /**< \if English debug \else 调试 \endif */
        OB_LOG_SEVERITY_INFO,  /**< \if English information \else 信息 \endif */
        OB_LOG_SEVERITY_WARN,  /**< \if English warning \else 警告 \endif */
        OB_LOG_SEVERITY_ERROR, /**< \if English error \else 错误 \endif */
        OB_LOG_SEVERITY_FATAL, /**< \if English fatal error \else 致命错误 \endif */
        OB_LOG_SEVERITY_NONE   /**< \if English none (close LOG) \else 无(关闭LOG) \endif */
    }

    /**
    * \if English
    * @brief The exception types in the SDK, through the exception type, you can easily determine the specific type of error.
    * For detailed error API interface functions and error logs, please refer to the information of ob_error
    * \else
    * @brief SDK内部的异常类型，通过异常类型，可以简单判断具体哪个类型的错误
    * 详细的错误API接口函数、错误日志请参考ob_error的信息
    * \endif
    */
    public enum ExceptionType
    {
        OB_EXCEPTION_TYPE_UNKNOWN, /**< \if English Unknown error, an error not clearly defined by the SDK \else 未知错误，SDK未明确定义的错误 \endif */
        OB_EXCEPTION_TYPE_CAMERA_DISCONNECTED, /**< \if English SDK device disconnection exception \else SDK的设备断开的异常 \endif */
        OB_EXCEPTION_TYPE_PLATFORM, /**< \if English An error in the SDK adaptation platform layer means an error in the implementation of a specific system
                                   platform \else 在SDK适配平台层错误，代表是具体一个系统平台实现上错误 \endif */
        OB_EXCEPTION_TYPE_INVALID_VALUE, /**< \if English Invalid parameter type exception, need to check input parameter \else 无效的参数类型异常，需要检查输入参数
                                        \endif */
        OB_EXCEPTION_TYPE_WRONG_API_CALL_SEQUENCE, /**< \if English Exception caused by API version mismatch\else API版本不匹配带来的异常 \endif */
        OB_EXCEPTION_TYPE_NOT_IMPLEMENTED, /**< \if English SDK and firmware have not yet implemented functions \else SDK及固件还未实现功能 \endif */
        OB_EXCEPTION_TYPE_IO,              /**< \if English SDK access IO exception error \else SDK访问IO异常错误 \endif */
        OB_EXCEPTION_TYPE_MEMORY,          /**< \if English SDK access and use memory errors, which means that the frame fails to allocate memory \else
                                          SDK的访问和使用内存错误，代表桢分配内存失败\endif */
        OB_EXCEPTION_TYPE_UNSUPPORTED_OPERATION, /**< \if English Unsupported operation type error by SDK or RGBD device \else SDK或RGBD设备不支持的操作类型错误
                                                \endif */
    }

    /**
     * \if English
     * @brief Enumeration value describing the sensor type
     * \else
     * @brief 描述传感器类型的枚举值
     * \endif
     */
    public enum SensorType
    {
        OB_SENSOR_UNKNOWN = 0, /**< \if English Unknown type sensor \else 未知类型传感器 \endif */
        OB_SENSOR_IR = 1, /**< \if English IR \else 红外 \endif */
        OB_SENSOR_COLOR = 2, /**< \if English Color \else 彩色 \endif */
        OB_SENSOR_DEPTH = 3, /**< \if English Depth \else 深度 \endif */
        OB_SENSOR_ACCEL = 4, /**< \if English Accel \else 加速度计 \endif */
        OB_SENSOR_GYRO = 5, /**< \if English Gyro \else 陀螺仪 \endif */
    }

    /**
    * \if English
    * @brief Enumeration value describing the type of data stream
    * \else
    * @brief 描述数据流类型的枚举值
    * \endif
    */
    public enum StreamType
    {
        OB_STREAM_UNKNOWN = -1, /**< \if English Unknown type stream \else 未知类型数据流 \endif */
        OB_STREAM_VIDEO = 0, /**< \if English Video stream (infrared, color, depth streams are all video streams) \else 视频流(红外、彩色、深度流) \endif */
        OB_STREAM_IR = 1, /**< \if English IR stream \else 红外流 \endif */
        OB_STREAM_COLOR = 2, /**< \if English color stream \else 彩色流 \endif */
        OB_STREAM_DEPTH = 3, /**< \if English depth stream \else 深度流 \endif */
        OB_STREAM_ACCEL = 4, /**< \if English Accelerometer data stream \else 加速度计数据流 \endif */
        OB_STREAM_GYRO = 5, /**< \if English Gyroscope data stream \else 陀螺仪数据流 \endif */
    }

    /**
     * \if English
     * @brief Describe the Frame type enumeration value
     * \else
     * @brief 描述Frame类型枚举值
     * \endif
     */
    public enum FrameType
    {
        OB_FRAME_UNKNOWN = -1, /**< \if English Unknown type frame \else 未知类型数据帧 \endif */
        OB_FRAME_VIDEO = 0,  /**< \if English Describes the Frame type enumeration value \else 视频帧(红外、彩色、深度帧都属于视频帧) \endif */
        OB_FRAME_IR = 1,  /**< \if English IR frame \else 红外帧 \endif */
        OB_FRAME_COLOR = 2,  /**< \if English color stream \else 彩色帧 \endif */
        OB_FRAME_DEPTH = 3,  /**< \if English depth stream \else 深度帧 \endif */
        OB_FRAME_ACCEL = 4,  /**< \if English Accelerometer data frame \else 加速度计数据帧 \endif */
        OB_FRAME_SET = 5, /**< \if English Frame collection (internally contains a variety of data frames) \else 帧集合(内部包含多种数据帧) \endif */
        OB_FRAME_POINTS = 6, /**< \if English point cloud frame \else 点云帧 \endif */
        OB_FRAME_GYRO = 7, /**< \if English Gyroscope data frame \else 陀螺仪数据帧 \endif */
    }

    /**
    * \if English
    * @brief Enumeration value describing the pixel format
    * \else
    * @brief 描述像素格式的枚举值
    * \endif
    */
    public enum Format
    {
        OB_FORMAT_YUYV = 0,  /**< \if English YUYV format \else YUYV格式 \endif */
        OB_FORMAT_YUY2 = 1,  /**< \if English YUY2 format (the actual format is the same as YUYV) \else YUY2格式(实际格式与YUYV相同) \endif */
        OB_FORMAT_UYVY = 2,  /**< \if English UYVY format \else UYVY格式 \endif */
        OB_FORMAT_NV12 = 3,  /**< \if English NV12 format \else NV12格式 \endif */
        OB_FORMAT_NV21 = 4,  /**< \if English NV21 format \else NV21格式 \endif */
        OB_FORMAT_MJPG = 5,  /**< \if English MJPG encoding format \else MJPG编码格式 \endif */
        OB_FORMAT_H264 = 6,  /**< \if English H.264 encoding format \else H.264编码格式 \endif */
        OB_FORMAT_H265 = 7,  /**< \if English H.265 encoding format \else H.265编码格式 \endif */
        OB_FORMAT_Y16 = 8,  /**< \if English Y16 format, single channel 16bit depth \else Y16格式，单通道16bit深度 \endif */
        OB_FORMAT_Y8 = 9,  /**< \if English Y8 format, single channel 8bit depth \else Y8格式，单通道8bit深度 \endif */
        OB_FORMAT_Y10 = 10, /**< \if English Y10 format, single channel 10bit depth (SDK will unpack into Y16 by default) \else
                            Y10格式，单通道10bit深度(SDK默认会解包成Y16) \endif */
        OB_FORMAT_Y11 = 11,  /**< \if English Y11 format, single channel 11bit depth (SDK will unpack into Y16 by default)\else
                            Y11格式，单通道11bit深度(SDK默认会解包成Y16) \endif */
        OB_FORMAT_Y12 = 12,  /**< \if English Y12 format, single channel 12bit depth (SDK will unpack into Y16 by default) \else
                            Y12格式，单通道12bit深度(SDK默认会解包成Y16) \endif */
        OB_FORMAT_GRAY = 13, /**< \if English GRAY (the actual format is the same as YUYV) \else GRAY灰度(实际格式与YUYV相同) \endif */
        OB_FORMAT_HEVC = 14, /**< \if English HEVC encoding format (the actual format is the same as H265) \else HEVC编码格式(实际格式与H265相同) \endif */
        OB_FORMAT_I420 = 15, /**< \if English I420 format \else I420格式  \endif */
        OB_FORMAT_ACCEL = 16, /**< \if English Acceleration data format \else 加速度数据格式  \endif */
        OB_FORMAT_GYRO = 17, /**< \if English Gyroscope Data Format \else 陀螺仪数据格式  \endif */
        OB_FORMAT_POINT = 19, /**< \if English xyz 3D coordinate point format \else 纯x-y-z三维坐标点格式  \endif */
        OB_FORMAT_RGB_POINT = 20, /**< \if English xyz 3D coordinate point format with RGB information \else 带RGB信息的x-y-z三维坐标点格式 \endif */
        OB_FORMAT_RLE = 21, /**< \if English RLE pressure test format (SDK will be unpacked into Y16 by default) \else RLE压测格式(SDK默认会解包成Y16) \endif */
        OB_FORMAT_RGB888 = 22,    /**< \if English RGB888 format \else RGB888格式  \endif */
        OB_FORMAT_BGR = 23,    /**< \if English BGR format (actual BRG888) \else BGR格式(实际BRG888) \endif */
        OB_FORMAT_Y14 = 24,    /**< \if English Y14 format, single channel 14bit depth (SDK will unpack into Y16 by default) \else
                                 Y14格式，单通道14bit深度(SDK默认会解包成Y16) \endif */
        OB_FORMAT_BGRA = 25,   /**< BGRA */
        OB_FORMAT_UNKNOWN = 0xff, /**< \if English unknown format \else 未知格式 \endif */
    }

    /**
    * \if English
    * @brief Firmware upgrade status
    * \else
    * @brief 固件升级状态
    * \endif
    */
    public enum UpgradeState
    {
        STAT_FILE_TRANSFER = 4,  /**< \if English file transfer \else 文件传输中 \endif */
        STAT_DONE = 3,  /**< \if English update completed \else 升级完成 \endif */
        STAT_IN_PROGRESS = 2,  /**< \if English upgrade in process \else 升级中 \endif */
        STAT_START = 1,  /**< \if English start the upgrade \else 开始升级 \endif */
        STAT_VERIFY_IMAGE = 0,  /**< \if English Image file verification \else 镜像文件校验中 \endif */
        ERR_VERIFY = -1, /**< \if English Verification failed \else 校验失败 \endif */
        ERR_PROGRAM = -2, /**< \if English Program execution failed \else 程序执行失败 \endif */
        ERR_ERASE = -3, /**< \if English Flash parameter failed \else Flash参数失败 \endif */
        ERR_FLASH_TYPE = -4, /**< \if English Flash type error \else Flash类型错误 \endif */
        ERR_IMAGE_SIZE = -5, /**< \if English Image file size error \else 镜像文件大小错误 \endif */
        ERR_OTHER = -6, /**< \if English other errors \else 其他错误 \endif */
        ERR_DDR = -7, /**< \if English DDR access error \else DDR访问错误 \endif */
        ERR_TIMEOUT = -8  /**< \if English timeout error \else 超时错误 \endif */
    }

    /**
    * \if English
    * @brief file transfer status
    * \else
    * @brief 文件传输状态
    * \endif
    */
    public enum FileTranState
    {
        FILE_TRAN_STAT_TRANSFER = 2,  /**< \if English file transfer \else 文件传输中 \endif */
        FILE_TRAN_STAT_DONE = 1,  /**< \if English file transfer succeeded\else 文件传输成功 \endif */
        FILE_TRAN_STAT_PREPAR = 0,  /**< \if English preparing \else 准备中 \endif */
        FILE_TRAN_ERR_DDR = -1, /**< \if English DDR access failed \else DDR访问失败 \endif */
        FILE_TRAN_ERR_NOT_ENOUGH_SPACE = -2, /**< \if English Insufficient target space error \else 目标空间不足错误 \endif */
        FILE_TRAN_ERR_PATH_NOT_WRITABLE = -3, /**< \if English Destination path is not writable \else 目标路径不可写 \endif */
        FILE_TRAN_ERR_MD5_ERROR = -4, /**< \if English MD5 checksum error \else MD5校验错误 \endif */
        FILE_TRAN_ERR_WRITE_FLASH_ERROR = -5, /**< \if English write flash error \else 写Flash错误 \endif */
        FILE_TRAN_ERR_TIMEOUT = -6  /**< \if English timeout error \else 超时错误 \endif */

    }

    /**
    * \if English
    * @brief data transfer status
    * \else
    * @brief 数据传输状态
    * \endif
    */
    public enum DataTranState
    {
        DATA_TRAN_STAT_STOPPED = 3,  /**< \if English data transfer stoped \else 数据传输终止 \endif */
        DATA_TRAN_STAT_DONE = 2,  /**< \if English data transfer completed \else 数据传输完成 \endif */
        DATA_TRAN_STAT_VERIFYING = 1,  /**< \if English data verifying \else 数据校验中 \endif */
        DATA_TRAN_STAT_TRANSFERRING = 0,  /**< \if English data transferring \else 数据传输中 \endif */
        DATA_TRAN_ERR_BUSY = -1, /**< \if English Transmission is busy \else 传输忙 \endif */
        DATA_TRAN_ERR_UNSUPPORTED = -2, /**< \if English not support \else 不支持 \endif */
        DATA_TRAN_ERR_TRAN_FAILED = -3, /**< \if English transfer failed \else 传输失败 \endif */
        DATA_TRAN_ERR_VERIFY_FAILED = -4, /**< \if English Test failed \else 检验失败 \endif */
        DATA_TRAN_ERR_OTHER = -5  /**< \if English other errors \else 其他错误\endif */
    }

    /**
    * \if English
    * @brief Data block structure for data block transmission
    * \else
    * @brief 数据块结构体，用于数据分块传输
    * \endif
    */
    [StructLayout(LayoutKind.Sequential)]
    public struct DataChunk
    {
        IntPtr data;    ///< \if English current block data pointer \else 当前块数据指针 \endif
        UInt32 size;    ///< \if English Current block data length \else 当前块数据长度 \endif
        UInt32 offset;  ///< \if English The offset of the current data block relative to the complete data \else 当前数据块相对完整数据的偏移 \endif
        UInt32 fullDataSize;  ///< \if English full data size \else 完整数据大小 \endif
    }

    /**
    * \if English
    * @brief Int range structure
    * \else
    * @brief 整形范围的结构体
    * \endif
    */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct IntPropertyRange
    {
        public Int32 cur;   ///< \if English current value \else 当前值 \endif
        public Int32 max;   ///< \if English maximum value \else 最大值 \endif
        public Int32 min;   ///< \if English minimum value \else 最小值 \endif
        public Int32 step;  ///< \if English step value \else 步进值 \endif
        public Int32 def;   ///< \if English Default \else 默认值 \endif
    }

    /**
    * \if English
    * @brief Float range structure
    * \else
    * @brief 浮点型范围的结构体
    * \endif
    */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct FloatPropertyRange
    {
        public float cur;   ///< \if English current value \else 当前值 \endif
        public float max;   ///< \if English maximum value \else 最大值 \endif
        public float min;   ///< \if English minimum value \else 最小值 \endif
        public float step;  ///< \if English step value \else 步进值 \endif
        public float def;   ///< \if English default \else 默认值 \endif
    }

    /**
     * \if English
     * @brief Boolean-scoped structure
     * \else
     * @brief 布尔型范围的结构体
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct BoolPropertyRange
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool cur;    ///< \if English current value \else 当前值 \endif
        [MarshalAs(UnmanagedType.I1)]
        public bool max;    ///< \if English maximum value \else 最大值 \endif
        [MarshalAs(UnmanagedType.I1)]
        public bool min;    ///< \if English minimum value \else 最小值 \endif
        [MarshalAs(UnmanagedType.I1)]
        public bool step;   ///< \if English step value \else 步进值 \endif
        [MarshalAs(UnmanagedType.I1)]
        public bool def;    ///< \if English default \else 默认值 \endif
    }

    /**
     * \if English
     * @brief Camera internal parameters
     * \else
     * @brief 相机内参
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct CameraIntrinsic
    {
        public float fx;      ///< \if English focal length in x direction \else x方向焦距 \endif
        public float fy;      ///< \if English focal length in y direction \else y方向焦距 \endif
        public float cx;      ///< \if English Optical center abscissa \else 光心横坐标 \endif
        public float cy;      ///< \if English Optical center ordinate \else 光心纵坐标 \endif
        public Int16 width;   ///< \if English image width \else 图像宽度 \endif
        public Int16 height;  ///< \if English image height \else 图像高度 \endif
    }

    /**
     * \if English
     * @brief Distortion Parameters
     * \else
     * @brief 畸变参数
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct CameraDistortion
    {
        public float k1;    ///< \if English Radial distortion factor 1 \else 径向畸变系数1 \endif
        public float k2;    ///< \if English Radial distortion factor 2 \else 径向畸变系数2 \endif
        public float k3;    ///< \if English Radial distortion factor 3 \else 径向畸变系数3 \endif
        public float k4;    ///< \if English Radial distortion factor 4 \else 径向畸变系数4 \endif
        public float k5;    ///< \if English Radial distortion factor 5 \else 径向畸变系数5 \endif
        public float k6;    ///< \if English Radial distortion factor 6 \else 径向畸变系数6 \endif
        public float p1;    ///< \if English Tangential distortion factor 1 \else 切向畸变系数1 \endif
        public float p2;    ///< \if English Tangential distortion factor 2 \else 切向畸变系数2 \endif
    }

    /**
     * \if English
     * @brief Rotation/Transformation
     * \else
     * @brief 旋转/变换矩阵
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct D2CTransform
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public float[] rot;  ///< \if English Rotation matrix \else 旋转矩阵，行优先\endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] trans;   ///< \if English transformation matrix \else 变化矩阵 \endif
    }

    /**
     * \if English
     * @brief Camera parameters
     * \else
     * @brief 相机参数
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct CameraParam
    {
        public CameraIntrinsic depthIntrinsic;   ///< \if English Depth camera internal parameters \else 深度相机内参 \endif    
        public CameraIntrinsic rgbIntrinsic;     ///< \if English Color camera internal parameters \else 彩色相机内参 \endif    
        public CameraDistortion depthDistortion;  ///< \if English Depth camera distortion parameters \else 深度相机畸变参数 \endif   
        public CameraDistortion rgbDistortion;    ///< \if English Color camera distortion parameters 1 \else 彩色相机畸变参数 \endif   
        public D2CTransform transform;        ///< \if English rotation/transformation matrix \else 旋转/变换矩阵 \endif   
        public bool isMirrored;       ///< \if English Whether the image frame corresponding to this group of parameters is mirrored \else 本组参数对应的图像帧是否被镜像 \endif   
    }

    /**
     * \if English
     * @brief alignment mode
     * \else
     * @brief 对齐模式
     * \endif
     */
    public enum AlignMode
    {
        ALIGN_DISABLE = 0,     /**< \if English turn off alignment \else 关闭对齐 \endif */
        ALIGN_D2C_HW_MODE = 1, /**< \if English Hardware D2C alignment mode \else 硬件D2C对齐模式 \endif */
        ALIGN_D2C_SW_MODE = 2, /**< \if English Software D2C alignment mode \else 软件D2C对齐模式 \endif */
    }

    /**
     * \if English
     * @brief rectangle
     * \else
     * @brief 矩形
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct Rect
    {
        public UInt32 x;       ///< \if English origin coordinate x \else 原点坐标x \endif
        public UInt32 y;       ///< \if English origin coordinate y \else 原点坐标y \endif
        public UInt32 width;   ///< \if English rectangle width \else 矩形宽度 \endif
        public UInt32 height;  ///< \if English rectangle height \else 矩形高度 \endif
    }

    /**
     * \if English
     * @brief format conversion type enumeration
     * \else
     * @brief 格式转换类型枚举
     * \endif
     */
    public enum ConvertFormat
    {
        FORMAT_YUYV_TO_RGB888 = 0, /**< \if English YUYV to RGB888 \else YUYV转换为RGB888 \endif */
        FORMAT_I420_TO_RGB888,     /**< \if English I420 to RGB888 \else I420转换为RGB888 \endif */
        FORMAT_NV21_TO_RGB888,     /**< \if English NV21 to RGB888 \else NV21转换为RGB888 \endif */
        FORMAT_NV12_TO_RGB888,     /**< \if English NV12 to RGB888 \else NV12转换为RGB888 \endif */
        FORMAT_MJPEG_TO_I420,      /**< \if English MJPG to I420 \else MJPG转换为I420\endif */
        FORMAT_RGB888_TO_BGR,      /**< \if English RGB888 to BGR \else RGB888转换为BGR \endif */
        FORMAT_MJPEG_TO_NV21,      /**< \if English MJPG to NV21 \else MJPG转换为NV21 \endif */
        FORMAT_MJPEG_TO_RGB888,    /**< \if English MJPG to RGB888 \else MJPG转换为RGB888 \endif */
    }

    /**
     * \if English
     * @brief Enumeration of IMU sample rate values ​​(gyroscope)
     * \else
     * @brief IMU采样率值的枚举(陀螺仪)
     * \endif
     */
    public enum GyroSampleRate
    {
        OB_SAMPLE_RATE_1_5625_HZ = 1, /**< 1.5625Hz */
        OB_SAMPLE_RATE_3_125_HZ,      /**< 3.125Hz */
        OB_SAMPLE_RATE_6_25_HZ,       /**< 6.25Hz */
        OB_SAMPLE_RATE_12_5_HZ,       /**< 12.5Hz */
        OB_SAMPLE_RATE_25_HZ,         /**< 25Hz */
        OB_SAMPLE_RATE_50_HZ,         /**< 50Hz */
        OB_SAMPLE_RATE_100_HZ,        /**< 100Hz */
        OB_SAMPLE_RATE_200_HZ,        /**< 200Hz */
        OB_SAMPLE_RATE_500_HZ,        /**< 500Hz */
        OB_SAMPLE_RATE_1_KHZ,         /**< 1KHz */
        OB_SAMPLE_RATE_2_KHZ,         /**< 2KHz */
        OB_SAMPLE_RATE_4_KHZ,         /**< 4KHz */
        OB_SAMPLE_RATE_8_KHZ,         /**< 8KHz */
        OB_SAMPLE_RATE_16_KHZ,        /**< 16KHz */
        OB_SAMPLE_RATE_32_KHZ,        /**< 32Hz */
    }

    /**
     * \if English
     * @brief Enumeration of IMU sample rate values ​​(accelerometer)
     * \else
     * @brief IMU采样率值的枚举(加速度计)
     * \endif
     */
    public enum AccelSampleRate
    {
        OB_SAMPLE_RATE_1_5625_HZ = 1, /**< 1.5625Hz */
        OB_SAMPLE_RATE_3_125_HZ,      /**< 3.125Hz */
        OB_SAMPLE_RATE_6_25_HZ,       /**< 6.25Hz */
        OB_SAMPLE_RATE_12_5_HZ,       /**< 12.5Hz */
        OB_SAMPLE_RATE_25_HZ,         /**< 25Hz */
        OB_SAMPLE_RATE_50_HZ,         /**< 50Hz */
        OB_SAMPLE_RATE_100_HZ,        /**< 100Hz */
        OB_SAMPLE_RATE_200_HZ,        /**< 200Hz */
        OB_SAMPLE_RATE_500_HZ,        /**< 500Hz */
        OB_SAMPLE_RATE_1_KHZ,         /**< 1KHz */
        OB_SAMPLE_RATE_2_KHZ,         /**< 2KHz */
        OB_SAMPLE_RATE_4_KHZ,         /**< 4KHz */
        OB_SAMPLE_RATE_8_KHZ,         /**< 8KHz */
        OB_SAMPLE_RATE_16_KHZ,        /**< 16KHz */
        OB_SAMPLE_RATE_32_KHZ,        /**< 32Hz */
    }

    /**
     * \if English
     * @brief Enumeration of gyroscope ranges
     * \else
     * @brief 陀螺仪量程的枚举
     * \endif
     */
    public enum GyroFullScaleRange
    {
        OB_GYRO_FS_16dps = 1, /**< \if English 16 degrees per second \else 16度每秒 \endif */
        OB_GYRO_FS_31dps,     /**< \if English 31 degrees per second \else 31度每秒 \endif */
        OB_GYRO_FS_62dps,     /**< \if English 62 degrees per second \else 62度每秒 \endif */
        OB_GYRO_FS_125dps,    /**< \if English 125 degrees per second \else 125度每秒 \endif */
        OB_GYRO_FS_245dps,    /**< \if English 245 degrees per second \else 245度每秒 \endif */
        OB_GYRO_FS_250dps,    /**< \if English 250 degrees per second \else 250度每秒 \endif */
        OB_GYRO_FS_500dps,    /**< \if English 500 degrees per second \else 500度每秒 \endif */
        OB_GYRO_FS_1000dps,   /**< \if English 1000 degrees per second \else 1000度每秒 \endif */
        OB_GYRO_FS_2000dps,   /**< \if English 2000 degrees per second \else 2000度每秒 \endif */
    }

    /**
     * \if English
     * @brief Accelerometer range enumeration
     * \else
     * @brief 加速度计量程枚举
     * \endif
     */
    public enum AccelFullScaleRange
    {
        OB_ACCEL_FS_2g = 1, /**< \if English 1x the acceleration of gravity \else 1倍重力加速度 \endif */
        OB_ACCEL_FS_4g,     /**< \if English 4x the acceleration of gravity \else 4倍重力加速度 \endif */
        OB_ACCEL_FS_8g,     /**< \if English 8x the acceleration of gravity \else 8倍重力加速度 \endif */
        OB_ACCEL_FS_16g,    /**< \if English 16x the acceleration of gravity \else 16倍重力加速度\endif */
    }

    /**
     * \if English
     * @brief Data structures for accelerometers
     *\else
     * @brief 加速度计的数据结构体
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct AccelValue
    {
        public float x; ///< \if English x-direction component \else x方向分量 \endif
        public float y; ///< \if English y-direction component \else y方向分量 \endif
        public float z; ///< \if English z-direction component \else z方向分量 \endif
    }

    /**
     * \if English
     * @brief Data structures for gyroscopes
     *\else
     * @brief 陀螺仪的数据结构体
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct GyroValue
    {
        public float x; ///< \if English x-direction component \else x方向分量 \endif
        public float y; ///< \if English y-direction component \else y方向分量 \endif
        public float z; ///< \if English z-direction component \else z方向分量 \endif
    }

    /**
     * \if English
     * @brief Get the temperature parameters of the device (unit: Celsius)
     * \else
     * @brief 获取设备的温度参数（单位：摄氏度）
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct DeviceTemperature
    {
        public float cpuTemp;        ///< \if English CPU temperature \else cpu温度 \endif
        public float irTemp;         ///< \if English IR temperature \else IR温度 \endif
        public float ldmTemp;        ///< \if English laser temperature \else 激光温度 \endif
        public float mainBoardTemp;  ///< \if English motherboard temperature \else 主板温度 \endif
        public float tecTemp;        ///< \if English TEC temperature \else TEC温度 \endif
        public float imuTemp;        ///< \if English IMU temperature \else IMU温度 \endif
        public float rgbTemp;        ///< \if English RGB temperature \else RGB温度 \endif
    }

    /**
     * \if English
     * @brief Depth crop mode enumeration
     * \else
     * @brief 深度裁切模式枚举
     * \endif
     */
    public enum DepthCroppingMode
    {
        DEPTH_CROPPING_MODE_AUTO = 0, /**< \if English automatic mode \else 自动模式 \endif */
        DEPTH_CROPPING_MODE_CLOSE = 1, /**< \if English close crop \else 关闭裁切 \endif */
        DEPTH_CROPPING_MODE_OPEN = 2, /**< \if English open crop \else 打开裁切 \endif */
    }

    /**
     * \if English
     * @brief device type enumeration
     * \else
     * @brief 设备类型枚举
     * \endif
     */
    public enum DeviceType
    {
        OB_STRUCTURED_LIGHT_MONOCULAR_CAMERA = 0, /**< \if English Monocular structured light camera \else 单目结构光相机  \endif */
        OB_STRUCTURED_LIGHT_BINOCULAR_CAMERA,     /**< \if English Binocular structured light camera \else 双目结构光相机  \endif */
        OB_TOF_CAMERA,                            /**< \if English TOF camera \else TOF相机  \endif */
    }

    /**
     * \if English
     * @brief record playback of the type of interest
     * \else
     * @brief 录制回放感兴趣数据类型
     * \endif
     */
    public enum MediaType
    {
        OB_MEDIA_COLOR_STREAM = 1,   /**< \if English color stream \else 彩色流\endif */
        OB_MEDIA_DEPTH_STREAM = 2,   /**< \if English depth stream \else 深度流 \endif */
        OB_MEDIA_IR_STREAM = 4,   /**< \if English IR stream \else 红外流 \endif */
        OB_MEDIA_GYRO_STREAM = 8,   /**< \if English gyro stream \else 陀螺仪数据流 \endif */
        OB_MEDIA_ACCEL_STREAM = 16,  /**< \if English accel stream \else 加速度计数据流 \endif */
        OB_MEDIA_CAMERA_PARAM = 32,  /**< \if English camera parameter \else 相机参数 \endif */
        OB_MEDIA_DEVICE_INFO = 64,  /**< \if English device information \else 设备信息  \endif */
        OB_MEDIA_STREAM_INFO = 128, /**< \if English stream information \else 流信息 \endif */

        OB_MEDIA_ALL = OB_MEDIA_COLOR_STREAM | OB_MEDIA_DEPTH_STREAM | OB_MEDIA_IR_STREAM | OB_MEDIA_GYRO_STREAM | OB_MEDIA_ACCEL_STREAM | OB_MEDIA_CAMERA_PARAM
                       | OB_MEDIA_DEVICE_INFO | OB_MEDIA_STREAM_INFO, /**< \if English All media data types \else 所有媒体数据类型 \endif */
    }

    /**
     * \if English
     * @brief Record playback status
     * \else
     * @brief 录制回放状态
     * \endif
     */
    public enum MediaState
    {
        OB_MEDIA_BEGIN = 0, /**< \if English begin \else 开始 \endif */
        OB_MEDIA_PAUSE,     /**< \if English pause \else 暂停 \endif */
        OB_MEDIA_RESUME,    /**< \if English resume \else 继续 \endif */
        OB_MEDIA_END,       /**< \if English end \else 终止  \endif */
    }

    /**
     * \if English
     * @brief depth accuracy class
     * @attention The depth accuracy level does not completely determine the depth unit and real accuracy, and the influence of the data packaging format needs to
     * be considered. The specific unit can be obtained through getValueScale() of DepthFrame \else
     * @brief 深度精度等级
     * @attention 深度精度等级并不完全决定深度的单位和真实精度，需要考虑数据打包格式的影响，
     * 具体单位可通过DepthFrame的getValueScale()获取
     * \endif
     */
    public enum DepthPrecisionLevel
    {
        OB_PRECISION_1MM,  /**< 1mm */
        OB_PRECISION_0MM8, /**< 0.8mm */
        OB_PRECISION_0MM4, /**< 0.4mm */
        OB_PRECISION_0MM1, /**< 0.1mm */
        OB_PRECISION_COUNT,
    }

    /**
    * \if English
    * @brief tof filter scene range
    * \else
    * @brief tof滤波场景范围
    * \endif
    */
    public enum TofFilterRange
    {
        OB_TOF_FILTER_RANGE_CLOSE = 0,   /**< \if English close range \else 近距离范围 \endif */
        OB_TOF_FILTER_RANGE_MIDDLE = 1,   /**< \if English middle range \else 中距离范围  \endif */
        OB_TOF_FILTER_RANGE_LONG = 2,   /**< \if English long range \else 远距离范围 \endif */
        OB_TOF_FILTER_RANGE_DEBUG = 100, /**< \if English debug range \else Debug模式  \endif */
    }

    /**
     * \if English
     * @brief 3D point structure in SDK
     * \else
     * @brief SDK中3D点结构体
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct Point
    {
        public float x;  ///< \if English x coordinate \else X坐标 \endif     
        public float y;  ///< \if English y coordinate \else Y坐标 \endif     
        public float z;  ///< \if English z coordinate \else Z坐标 \endif     
    }

    /**
     * \if English
     * @brief 3D point structure with color information
     * \else
     * @brief 带有颜色信息的3D点结构体
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct ColorPoint
    {
        public float x;  ///< \if English x coordinate \else X坐标 \endif    
        public float y;  ///< \if English y coordinate \else Y坐标 \endif   
        public float z;  ///< \if English z coordinate \else Z坐标 \endif    
        public float r;  ///< \if English red channel component \else 红色通道分量 \endif    
        public float g;  ///< \if English green channel component \else 绿色通道分量 \endif    
        public float b;  ///< \if English blue channel component\else 蓝色通道分量 \endif    
    }

    /**
     * \if English
     * @brief Multi-device sync mode
     * \else
     * @brief 多设备同步模式
     * \endif
     */
    public enum SyncMode
    {
        OB_SYNC_STOP = 0x00,  /**< \if English turn off sync \else 关闭同步 \endif */
        OB_SYNC_SINGLE_MODE = 0x01,  /**< \if English single device mode \else single device模式 \endif */
        OB_SYNC_ONLINE_HOST_MODE = 0x02,  /**< \if English The single device mode is also the host mode, which is dominated by ir \else single
                                         device模式，也是host模式，是ir作主的  \endif */
        OB_SYNC_ONLINE_SLAVE_MODE = 0x03, /**< \if English slave mode (ext_in --> rgb、tof、ext_out) \else slave模式 (ext_in --> rgb、tof、ext_out)  \endif */
        OB_SYNC_ONLY_MCU_MODE = 0x04, /**< \if English MCU as host mode \else mcu作host的模式 \endif */
        OB_SYNC_ONLY_IR_MODE = 0x05, /**< \if English IR as host mode\else ir作host的模式  \endif */
    }

    /**
     * \if English
     * @brief TOF Exposure Threshold
     * \else
     * @brief TOF曝光阈值
     *\endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct TofExposureThresholdControl
    {
        public Int32 upper;  ///< \if English Upper threshold, unit: ms \else 阈值上限， 单位：ms \endif
        public Int32 lower;  ///< \if English Lower threshold, unit: ms \else 阈值下限， 单位：ms \endif
    }

    /**
     * \if English
     * @brief Multi-device synchronization configuration
     * \else
     * @brief 多设备同步配置
     * \endif
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable()]
    public struct MultiDeviceSyncConfig
    {
        public SyncMode syncMode;       ///< \if English Sync mode \else 同步模式 \endif
        public UInt16 tofPhaseDelay;  ///< \if English Tof trigger signal delay us. \else Tof触发信号延时 us \endif
        public UInt16 rgbPhaseDelay;  ///< \if English rgb trigger signal delay us \else rgb触发信号延时 us \endif
        public UInt16 outPhaseDelay;  ///< \if English Output signal delay us \else 输出信号延时 us \endif
        public UInt16 outOCPolarity;  ///< \if English 0: Positive pulse, 1: Negative pulse \else 0:正向脉冲, 1: 负向脉冲 \endif
        public UInt16 mcuHostFps;     ///< \if English Trigger frame rate in mcu master mode \else mcu主模式时的触发帧率 \endif
        public UInt16 curDevId;       ///< \if English Current device number \else 当前设备编号 \endif
    }

    /**
     * @brief 网络设备的IP地址配置（ipv4）
     *
     */
    public struct DeviceIpAddrConfig
    {
        public UInt16 dhcp;        ///< dhcp 动态ip配置开关; 0:关; 1: 开
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] address;  ///< ip地址(大端模式, 如192.168.1.1，则address[0]==192)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] mask;     ///< 子网掩码(大端模式)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] gateway;  ///< 网关(大端模式)
    }

    /**
     * @brief 设备通信模式
     *
     */
    public enum CommunicationType
    {
        OB_COMM_USB = 0x00, /**< USB */
        OB_COMM_NET = 0x01, /**< 网络 */
    }
}