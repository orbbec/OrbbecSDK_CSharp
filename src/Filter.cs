using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Orbbec
{
    public delegate void FilterCallback(Frame frame);

    public class Filter : IDisposable
    {
        protected NativeHandle _handle;
        // private FilterCallback _callback;
        private static Dictionary<IntPtr, FilterCallback> _filterCallbacks = new Dictionary<IntPtr, FilterCallback>();
        private NativeFilterCallback _nativeCallback;

#if ORBBEC_UNITY
        [AOT.MonoPInvokeCallback(typeof(FilterCallback))]
#endif
        private static void OnFilter(IntPtr framePtr, IntPtr userData)
        {
            Frame frame = new Frame(framePtr);
            _filterCallbacks.TryGetValue(userData, out FilterCallback callback);
            if (callback != null)
            {
                callback(frame);
            }
            else
            {
                frame.Dispose();
            }
        }

        internal Filter()
        {
            _nativeCallback = new NativeFilterCallback(OnFilter);
        }

        internal Filter(IntPtr handle)
        {
            _handle = new NativeHandle(handle, Delete);
            _nativeCallback = new NativeFilterCallback(OnFilter);
        }

        /**
        * \if English
        * @brief filter reset, free the internal cache, stop the processing thread and clear the pending buffer frame when asynchronous processing
        * \else
        * @brief filter重置，释放内部缓存，异步处理时停止处理线程并清空待处理的缓存帧
        * \endif
        */
        public void Reset()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_filter_reset(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Processing frames (synchronous interface)
        *
        * @param frame frame to be processed
        * @return Frame processed frame
        * \else
        * @brief 处理帧（同步接口）
        *
        * @param frame 需要处理的frame
        * @return Frame 处理后的frame
        * \endif
        */
        public Frame Process(Frame frame)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_filter_process(_handle.Ptr, frame.GetNativeHandle().Ptr, ref error);
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            return new Frame(handle);
        }

        /**
        * \if English
        * @brief Set the callback function (asynchronous callback interface)
        *
        * @param callback Processing result callback
        * \else
        * @brief 设置回调函数（异步回调接口）
        *
        * @param callback 处理结果回调
        * \endif
        */
        public void SetCallback(FilterCallback callback)
        {
            _filterCallbacks[_handle.Ptr] = callback;
            IntPtr error = IntPtr.Zero;
            obNative.ob_filter_set_callback(_handle.Ptr, _nativeCallback, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Push the pending frame into the cache (asynchronous callback interface)
        *
        * @param frame The pending frame processing result is returned by the callback function
        * \else
        * @brief 压入待处理frame到缓存（异步回调接口）
        *
        * @param frame 待处理的frame处理结果通过回调函数返回
        * \endif
        */
        public void PushFrame(Frame frame)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_filter_push_frame(_handle.Ptr, frame.GetNativeHandle().Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public string GetName()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr namePtr = obNative.ob_get_filter_name(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }

            // TODO: check charset
            return Marshal.PtrToStringAnsi(namePtr);
        }

        internal void Delete(IntPtr handle)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_delete_filter(handle, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public void Dispose()
        {
            _handle.Dispose();
        }

        public bool Enabled
        {
            get
            {
                return IsEnabled();
            }
            set
            {
                Enable(value);
            }
        }

        internal void Enable(bool enabled)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_filter_enable(_handle.Ptr, enabled, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        internal bool IsEnabled()
        {
            IntPtr error = IntPtr.Zero;
            bool isEnabled = obNative.ob_filter_is_enable(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return isEnabled;
        }
    }

    public class PointCloudFilter : Filter
    {
        public PointCloudFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_pointcloud_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        /**
        * \if English
        * @brief Set camera parameters
        *
        * @param param Camera internal and external parameters
        * \else
        * @brief 设置相机参数
        *
        * @param param 相机内外参数
        * \endif
        */
        public void SetCameraParam(CameraParam cameraParam)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_pointcloud_filter_set_camera_param(_handle.Ptr, cameraParam, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief Set point cloud type parameters
        *
        * @param type Point cloud type depth point cloud or RGBD point cloud
        * \else
        * @brief 设置点云类型参数
        *
        * @param type 点云类型深度点云或RGBD点云
        * \endif
        */
        public void SetPointFormat(Format format)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_pointcloud_filter_set_point_format(_handle.Ptr, format, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief  Set the frame alignment state that will be input to generate point cloud (it needs to be enabled in D2C mode, as the basis for the algorithm to
        * select the set of camera internal parameters)
        *
        * @param state Alignment status, True: enable alignment; False: disable alignment
        * \else
        * @brief  设置将要输入的用于生成点云的帧对齐状态（D2C模式下需要开启，作为算法选用那组相机内参的依据）
        *
        * @param state 对齐状态，True：开启对齐； False：关闭对齐
        * \endif
        */
        public void SetAlignState(bool state)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_pointcloud_filter_set_frame_align_state(_handle.Ptr, state, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief  Set the point cloud coordinate data zoom factor
        *
        * @attention Calling this function to set the scale will change the point coordinate scaling factor of the output point cloud frame: posScale = posScale /
        * scale.The point coordinate scaling factor for the output point cloud frame can be obtained via @ref PointsFrame::getPositionValueScale function
        *
        * @param scale Zoom factor
        * \else
        * @brief  设置点云坐标数据缩放比例
        *
        * @attention 调用该函数设置缩放比例会改变输出点云帧的点坐标缩放系数：posScale = posScale / scale;
        *  输出点云帧的点坐标缩放系数可通过 @ref PointsFrame::getPositionValueScale 函数获取
        *
        * @param scale 缩放比例
        * \endif
        */
        public void SetPositionDataScaled(float scale)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_pointcloud_filter_set_position_data_scale(_handle.Ptr, scale, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief  Set point cloud color data normalization
        *
        * @param state Whether normalization is required
        * \else
        * @brief  设置点云颜色数据归一化
        *
        * @param state 是否需要归一化
        * \endif
        */
        public void SetColorDataNormalization(bool state)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_pointcloud_filter_set_color_data_normalization(_handle.Ptr, state, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        /**
        * \if English
        * @brief  Set point cloud coordinate system
        *
        * @param type coordinate system type
        * \else
        * @brief  设置点云坐标系
        *
        * @param type 坐标系类型
        * \endif
        */
        public void SetCoordinateSystem(CoordinateSystemType type)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_pointcloud_filter_set_coordinate_system(_handle.Ptr, type, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class FormatConvertFilter : Filter
    {
        public FormatConvertFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_format_convert_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        /**
        * \if English
        * @brief Set format conversion type
        *
        * @param type Format conversion type
        * \else
        * @brief 设置格式转化类型
        *
        * @param format 格式转化类型
        * \endif
        */
        public void SetConvertFormat(ConvertFormat format)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_format_convert_filter_set_format(_handle.Ptr, format, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class CompressionFilter : Filter
    {
        public CompressionFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_compression_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        /**
        * \if English
        * @brief Set compression params
        *
        * @param mode Compression mode OB_COMPRESSION_LOSSLESS or OB_COMPRESSION_LOSSY
        * @param params Compression params, when mode is OB_COMPRESSION_LOSSLESS, params is NULL
        * \else
        * @brief 设置压缩参数
        *
        * @param mode 压缩模式 OB_COMPRESSION_LOSSLESS or OB_COMPRESSION_LOSSY
        * @param params 压缩参数, 当mode为OB_COMPRESSION_LOSSLESS时，params为NULL
        * \endif
        */
        public void SetCompressionParams(CompressionMode mode, IntPtr param)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_compression_filter_set_compression_params(_handle.Ptr, mode, param, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class DecompressionFilter : Filter
    {
        public DecompressionFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_decompression_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }
    }

    public class HoleFillingFilter : Filter
    {
        public HoleFillingFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_holefilling_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public HoleFillingMode GetMode()
        {
            IntPtr error = IntPtr.Zero;
            var mode = obNative.ob_holefilling_filter_get_mode(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return mode;
        }

        public void SetMode(HoleFillingMode holeFillingMode)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_holefilling_filter_set_mode(_handle.Ptr, holeFillingMode, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class TemporalFilter : Filter
    {
        public TemporalFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_temporal_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public FloatPropertyRange GetDiffscaleRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_temporal_filter_get_diffscale_range(out FloatPropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public void SetDiffscaleValue(float value)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_temporal_filter_set_diffscale_value(_handle.Ptr, value, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public FloatPropertyRange GetWeightRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_temporal_filter_get_weight_range(out FloatPropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public void SetWeightValue(float value)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_temporal_filter_set_weight_value(_handle.Ptr, value, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class SpatialAdvancedFilter : Filter
    {
        public SpatialAdvancedFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_spatial_advanced_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public FloatPropertyRange GetAlphaRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_spatial_advanced_filter_get_alpha_range(out FloatPropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public UInt16PropertyRange GetDispDiffRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_spatial_advanced_filter_get_disp_diff_range(out UInt16PropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public UInt16PropertyRange GetRadiusRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_spatial_advanced_filter_get_radius_range(out UInt16PropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public IntPropertyRange GetMagnitudeRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_spatial_advanced_filter_get_magnitude_range(out IntPropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public SpatialAdvancedFilterParams GetFilterParams()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_spatial_advanced_filter_get_filter_params(out SpatialAdvancedFilterParams filterParams, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return filterParams;
        }

        public void SetFilterParams(SpatialAdvancedFilterParams filterParams)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_spatial_advanced_filter_set_filter_params(_handle.Ptr, filterParams, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class NoiseRemovalFilter : Filter
    {
        public NoiseRemovalFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_noise_removal_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public UInt16PropertyRange GetDispDiffRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_noise_removal_filter_get_disp_diff_range(out UInt16PropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public IntPropertyRange GetMaxSizeRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_noise_removal_filter_get_max_size_range(out IntPropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public NoiseRemovalFilterParams GetFilterParams()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_noise_removal_filter_get_filter_params(out NoiseRemovalFilterParams filterParams, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return filterParams;
        }

        public void SetFilterParams(NoiseRemovalFilterParams filterParams)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_noise_removal_filter_set_filter_params(_handle.Ptr, filterParams, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class EdgeNoiseRemovalFilter : Filter
    {
        public EdgeNoiseRemovalFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_edge_noise_removal_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public UInt16PropertyRange GetMarginLeftThRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_edge_noise_removal_filter_get_margin_left_th_range(out UInt16PropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }
        public UInt16PropertyRange GetMarginRightThRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_edge_noise_removal_filter_get_margin_right_th_range(out UInt16PropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }
        public UInt16PropertyRange GetMarginTopThRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_edge_noise_removal_filter_get_margin_top_th_range(out UInt16PropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }
        public UInt16PropertyRange GetMarginBottomThRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_edge_noise_removal_filter_get_margin_bottom_th_range(out UInt16PropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public EdgeNoiseRemovalFilterParams GetFilterParams()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_edge_noise_removal_filter_get_filter_params(out EdgeNoiseRemovalFilterParams filterParams, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return filterParams;
        }

        public void SetFilterParams(EdgeNoiseRemovalFilterParams filterParams)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_edge_noise_removal_filter_set_filter_params(_handle.Ptr, filterParams, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class DecimationFilter : Filter
    {
        public DecimationFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_decimation_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public UInt8PropertyRange GetScaleRange()
        {
            IntPtr error = IntPtr.Zero;
            UInt8PropertyRange range = obNative.ob_decimation_filter_get_scale_range(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public void SetScaleValue(byte value)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_decimation_filter_set_scale_value(_handle.Ptr, value, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public byte GetScaleValue()
        {
            IntPtr error = IntPtr.Zero;
            byte value = obNative.ob_decimation_filter_get_scale_value(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return value;
        }
    }

    public class ThresholdFilter : Filter
    {
        public ThresholdFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_threshold_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public IntPropertyRange GetMinRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_threshold_filter_get_min_range(out IntPropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public IntPropertyRange GetMaxRange()
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_threshold_filter_get_max_range(out IntPropertyRange range, _handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return range;
        }

        public void SetScaleValue(UInt16 min, UInt16 max)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_threshold_filter_set_scale_value(_handle.Ptr, min, max, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }
    }

    public class SequenceIdFilter : Filter
    {
        public SequenceIdFilter()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_sequenceId_filter(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public void SelectSequenceId(int sequenceId)
        {
            IntPtr error = IntPtr.Zero;
            obNative.ob_sequence_id_filter_select_sequence_id(_handle.Ptr, sequenceId, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
        }

        public int GetSequenceId()
        {
            IntPtr error = IntPtr.Zero;
            int sequenceId = obNative.ob_sequence_id_filter_get_sequence_id(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return sequenceId;
        }

        // TODO: make this return (an array of?) SequenceIdItem instead
        public IntPtr GetSequenceIdList()
        {
            IntPtr error = IntPtr.Zero;
            IntPtr sequenceIdList = obNative.ob_sequence_id_filter_get_sequence_id_list(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return sequenceIdList;
        }

        public int GetSequenceIdListSize()
        {
            IntPtr error = IntPtr.Zero;
            int sequenceIdListSize = obNative.ob_sequence_id_filter_get_sequence_id_list_size(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return sequenceIdListSize;
        }
    }

    public class HdrMerge : Filter
    {
        public HdrMerge(bool depthToDisparity)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_hdr_merge(ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }
    }

    public class Align : Filter
    {
        public Align(StreamType alignToStream)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_align(ref error, alignToStream);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }

        public StreamType GetAlignToStreamType()
        {
            IntPtr error = IntPtr.Zero;
            StreamType streamType = obNative.ob_align_get_to_stream_type(_handle.Ptr, ref error);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            return streamType;
        }
    }

    public class DisparityTransform : Filter
    {
        public DisparityTransform(bool depthToDisparity)
        {
            IntPtr error = IntPtr.Zero;
            IntPtr handle = obNative.ob_create_disparity_transform(ref error, depthToDisparity);
            if (error != IntPtr.Zero)
            {
                throw new NativeException(new Error(error));
            }
            _handle = new NativeHandle(handle, Delete);
        }
    }
}