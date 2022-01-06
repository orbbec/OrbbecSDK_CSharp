using System;
using System.Runtime.InteropServices;

namespace Orbbec
{
    internal class obNative
    {
        public const string obsdk = "OrbbecSDK";

        #region Context
        [DllImport(obsdk, EntryPoint = "ob_create_context")]
        public static extern IntPtr ob_create_context([Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_create_context_with_config")]
        public static extern IntPtr ob_create_context_with_config(String configPath, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_delete_context")]
        public static extern void ob_delete_context(IntPtr context, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_query_device_list")]
        public static extern IntPtr ob_query_device_list(IntPtr context, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_set_device_changed_callback")]
        public static extern void ob_set_device_changed_callback(IntPtr context, [MarshalAs(UnmanagedType.FunctionPtr)] DeviceChangedCallbackInternal callback, IntPtr userData, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_set_logger_serverity")]
        public static extern void ob_set_logger_serverity(IntPtr context, LogServerity logServerity, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_set_logger_to_file")]
        public static extern void ob_set_logger_to_file(IntPtr context, LogServerity logServerity, String fileName, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_set_logger_to_console")]
        public static extern void ob_set_logger_to_console(IntPtr context, LogServerity logServerity, [Out] out IntPtr error);
        #endregion

        #region Device
        [DllImport(obsdk, EntryPoint = "ob_device_list_device_count")]
        public static extern UInt32 ob_device_list_count(IntPtr deviceList, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_info")]
        public static extern IntPtr ob_device_list_get_device_info(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_name")]
        public static extern IntPtr ob_device_list_get_device_name(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_pid")]
        public static extern int ob_device_list_get_device_pid(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_vid")]
        public static extern int ob_device_list_get_device_vid(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_uid")]
        public static extern IntPtr ob_device_list_get_device_uid(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_get_device")]
        public static extern IntPtr ob_get_device(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_delete_device")]
        public static extern void ob_delete_device(IntPtr device, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_delete_device_info")]
        public static extern void ob_delete_device_info(IntPtr deviceInfo, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_delete_device_list")]
        public static extern void ob_delete_device_list(IntPtr deviceList, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_get_device_info")]
        public static extern IntPtr ob_device_get_device_info(IntPtr device, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_get_sensor_list")]
        public static extern IntPtr ob_device_get_sensor_list(IntPtr device, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_get_sensor")]
        public static extern IntPtr ob_device_get_sensor(IntPtr device, SensorType sensorType, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_set_int_property")]
        public static extern void ob_device_set_int_property(IntPtr device, PropertyId propertyId, Int32 property, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_get_int_property")]
        public static extern Int32 ob_device_get_int_property(IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_set_float_property")]
        public static extern void ob_device_set_float_property(IntPtr device, PropertyId propertyId, float property, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_get_float_property")]
        public static extern float ob_device_get_float_property(IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_set_bool_property")]
        public static extern void ob_device_set_bool_property(IntPtr device, PropertyId propertyId, bool property, [Out] out IntPtr error);

        [DllImport(obsdk, EntryPoint = "ob_device_get_bool_property")]
        public static extern bool ob_device_get_bool_property(IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //void ob_device_set_structured_data( ob_device* device, ob_global_unified_property property_id, const void* data, uint32_t data_size, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_set_structured_data")]
        public static extern void ob_device_set_structured_data(IntPtr device, PropertyId propertyId, IntPtr data, UInt32 dataSize, [Out] out IntPtr error);

        //void ob_device_get_structured_data( ob_device* device, ob_global_unified_property property_id, void* data, uint32_t* data_size, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_structured_data")]
        public static extern void ob_device_get_structured_data(IntPtr device, PropertyId propertyId, IntPtr data, ref UInt32 dataSize, [Out] out IntPtr error);

        //bool ob_device_is_property_supported( ob_device* device, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_is_property_supported")]
        public static extern bool ob_device_is_property_supported(IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //ob_int_property_range ob_device_get_int_property_range( ob_device* device, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_int_property_range")]
        public static extern void ob_device_get_int_property_range(out IntPropertyRange range, IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //ob_float_property_range ob_device_get_float_property_range( ob_device* device, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_float_property_range")]
        public static extern void ob_device_get_float_property_range(out FloatPropertyRange range, IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //ob_bool_property_range ob_device_get_bool_property_range( ob_device* device, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_bool_property_range")]
        public static extern void ob_device_get_bool_property_range(out BoolPropertyRange range, IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //ob_device_state ob_device_get_device_state( ob_device* device, ob_error** error );
        // [DllImport(obsdk, EntryPoint = "ob_device_get_device_state")]
        // public static extern void ob_device_get_device_state(out DeviceState state, IntPtr device, [Out] out IntPtr error);

        //void ob_device_state_changed( ob_device* device, ob_device_state_callback callback, void* user_data, ob_error** error );
        // [DllImport(obsdk, EntryPoint = "ob_device_state_changed")]
        // public static extern void ob_device_state_changed(IntPtr device, [MarshalAs(UnmanagedType.FunctionPtr)] DeviceStateCallback callback, IntPtr userData, [Out] out IntPtr error);

        //bool ob_device_activate_authorization( ob_device* device, const char* auth_code, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_activate_authorization")]
        public static extern bool ob_device_activate_authorization(IntPtr device, String authCode, [Out] out IntPtr error);

        //void ob_device_write_authorization_code( ob_device* device, const char* auth_code, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_write_authorization_code")]
        public static extern void ob_device_write_authorization_code(IntPtr device, String autoCode, [Out] out IntPtr error);

        //ob_camera_intrinsic ob_device_get_camera_intrinsic( ob_device* device, ob_sensor_type sensor_type, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_camera_intrinsic")]
        public static extern void ob_device_get_camera_intrinsic(out CameraIntrinsic intrinsic, IntPtr device, SensorType sensorType, [Out] out IntPtr error);

        //ob_camera_distortion ob_device_get_camera_distortion( ob_device* device, ob_sensor_type sensor_type, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_camera_distortion")]
        public static extern void ob_device_get_camera_distortion(out CameraDistortion distortion, IntPtr device, SensorType sensorType, [Out] out IntPtr error);

        //ob_d2c_transform ob_device_get_d2c_transform( ob_device* device, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_d2c_transform")]
        public static extern void ob_device_get_d2c_transform(out D2CTransform d2CTransform , IntPtr device, [Out] out IntPtr error);

        //const char* ob_device_info_name( ob_device_info* info, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_info_name")]
        public static extern IntPtr ob_device_info_name(IntPtr deviceInfo, [Out] out IntPtr error);

        //int ob_device_info_pid( ob_device_info* info, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_info_pid")]
        public static extern int ob_device_info_pid(IntPtr deviceInfo, [Out] out IntPtr error);

        //int ob_device_info_vid( ob_device_info* info, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_info_vid")]
        public static extern int ob_device_info_vid(IntPtr deviceInfo, [Out] out IntPtr error);

        //const char* ob_device_info_uid( ob_device_info* info, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_info_uid")]
        public static extern IntPtr ob_device_info_uid(IntPtr deviceInfo, [Out] out IntPtr error);

        //const char* ob_device_info_serial_number( ob_device_info* info, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_info_serial_number")]
        public static extern IntPtr ob_device_info_serial_number(IntPtr deviceInfo, [Out] out IntPtr error);

        //const char* ob_device_info_firmware_version( ob_device_info* info, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_info_firmware_version")]
        public static extern IntPtr ob_device_info_firmware_version(IntPtr deviceInfo, [Out] out IntPtr error);

        //const char* ob_device_info_usb_type( ob_device_info* info, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_info_usb_type")]
        public static extern IntPtr ob_device_info_usb_type(IntPtr deviceInfo, [Out] out IntPtr error);
        #endregion

        #region Error
        //ob_status ob_error_status( ob_error* error );
        [DllImport(obsdk, EntryPoint = "ob_error_status")]
        public static extern Status ob_error_status(IntPtr error);

        //const char* ob_error_message( ob_error* error );
        [DllImport(obsdk, EntryPoint = "ob_error_message")]
        public static extern IntPtr ob_error_message(IntPtr error);

        //const char* ob_error_function( ob_error* error );
        [DllImport(obsdk, EntryPoint = "ob_error_function")]
        public static extern IntPtr ob_error_function(IntPtr error);

        //const char* ob_error_args( ob_error* error );
        [DllImport(obsdk, EntryPoint = "ob_error_args")]
        public static extern IntPtr ob_error_args(IntPtr error);

        //ob_exception_type ob_error_exception_type( ob_error* error );
        [DllImport(obsdk, EntryPoint = "ob_error_exception_type")]
        public static extern ExceptionType ob_error_exception_type(IntPtr error);

        //void ob_delete_error( ob_error* error );
        [DllImport(obsdk, EntryPoint = "ob_delete_error")]
        public static extern void ob_delete_error(IntPtr error);
        #endregion

        #region Frame
        //uint64_t ob_frame_index( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_index")]
        public static extern UInt64 ob_frame_index(IntPtr frame, [Out] out IntPtr error);

        //ob_format ob_frame_format( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_format")]
        public static extern Format ob_frame_format(IntPtr frame, [Out] out IntPtr error);

        //ob_frame_type ob_frame_get_type( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_get_type")]
        public static extern FrameType ob_frame_get_type(IntPtr frame, [Out] out IntPtr error);

        //uint64_t ob_frame_time_stamp( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_time_stamp")]
        public static extern UInt64 ob_frame_time_stamp(IntPtr frame, [Out] out IntPtr error);

        //uint64_t ob_frame_system_time_stamp( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_system_time_stamp")]
        public static extern UInt64 ob_frame_system_time_stamp(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_video_frame_width( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_video_frame_width")]
        public static extern UInt32 ob_video_frame_width(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_video_frame_height( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_video_frame_height")]
        public static extern UInt32 ob_video_frame_height(IntPtr frame, [Out] out IntPtr error);

        //float ob_depth_frame_get_value_scale( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_depth_frame_get_value_scale")]
        public static extern float ob_depth_frame_get_value_scale(IntPtr frame, [Out] out IntPtr error);

        //void* ob_frame_data( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_data")]
        public static extern IntPtr ob_frame_data(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_frame_data_size( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_data_size")]
        public static extern UInt32 ob_frame_data_size(IntPtr frame, [Out] out IntPtr error);

        //void* ob_frame_metadata( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_metadata")]
        public static extern IntPtr ob_frame_metadata(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_frame_metadata_size( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_metadata_size")]
        public static extern UInt32 ob_frame_metadata_size(IntPtr frame, [Out] out IntPtr error);

        //ob_stream_type ob_frame_get_stream_type( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_get_stream_type")]
        public static extern StreamType ob_frame_get_stream_type(IntPtr frame, [Out] out IntPtr error);

        //void ob_delete_frame( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_frame")]
        public static extern void ob_delete_frame(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_frame_set_frame_count( ob_frame* frame_set, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_set_frame_count")]
        public static extern UInt32 ob_frame_set_frame_count(IntPtr frame, [Out] out IntPtr error);

        //ob_frame* ob_frame_set_depth_frame( ob_frame* frame_set, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_set_depth_frame")]
        public static extern IntPtr ob_frame_set_depth_frame(IntPtr frameset, [Out] out IntPtr error);

        //ob_frame* ob_frame_set_color_frame( ob_frame* frame_set, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_set_color_frame")]
        public static extern IntPtr ob_frame_set_color_frame(IntPtr frameset, [Out] out IntPtr error);

        //ob_frame* ob_frame_set_ir_frame( ob_frame* frame_set, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_set_ir_frame")]
        public static extern IntPtr ob_frame_set_ir_frame(IntPtr frameset, [Out] out IntPtr error);

        //ob_frame* ob_frame_set_points_frame( ob_frame* frame_set, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_set_points_frame")]
        public static extern IntPtr ob_frame_set_points_frame(IntPtr frameset, [Out] out IntPtr error);

        //ob_accel_value ob_accel_frame_value( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_accel_frame_value")]
        public static extern void ob_accel_frame_value(out AccelValue accelValue, IntPtr frame, [Out] out IntPtr error);

        //float ob_accel_frame_temperature( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_accel_frame_temperature")]
        public static extern float ob_accel_frame_temperature(IntPtr frame, [Out] out IntPtr error);

        //ob_gyro_value ob_gyro_frame_value( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_gyro_frame_value")]
        public static extern void ob_gyro_frame_value(out GyroValue gyroValue, IntPtr frame, [Out] out IntPtr error);

        //float ob_gyro_frame_temperature( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_gyro_frame_temperature")]
        public static extern float ob_gyro_frame_temperature(IntPtr frame, [Out] out IntPtr error);
        #endregion

        #region Pipeline

        //ob_pipeline* ob_create_pipeline( ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_create_pipeline")]
        public static extern IntPtr ob_create_pipeline([Out] out IntPtr error);

        //ob_pipeline* ob_create_pipeline_with_device( ob_device* dev, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_create_pipeline_with_device")]
        public static extern IntPtr ob_create_pipeline_with_device(IntPtr device, [Out] out IntPtr error);

        //void ob_delete_pipeline( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_pipeline")]
        public static extern void ob_delete_pipeline(IntPtr pipeline, [Out] out IntPtr error);

        //void ob_pipeline_start( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_start")]
        public static extern void ob_pipeline_start(IntPtr pipeline, [Out] out IntPtr error);

        //void ob_pipeline_start_with_config( ob_pipeline* pipeline, ob_config* config, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_start_with_config")]
        public static extern void ob_pipeline_start_with_config(IntPtr pipeline, IntPtr config, [Out] out IntPtr error);

        //void ob_pipeline_start_with_callback( ob_pipeline* pipeline, ob_config* config, ob_frame_set_callback callback, void* user_data, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_start_with_callback")]
        public static extern void ob_pipeline_start_with_callback(IntPtr pipeline, IntPtr config, [MarshalAs(UnmanagedType.FunctionPtr)] FramesetCallbackInternal callback, IntPtr userData, [Out] out IntPtr error);

        //void ob_pipeline_stop( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_stop")]
        public static extern void ob_pipeline_stop(IntPtr pipeline, [Out] out IntPtr error);

        //ob_config* ob_pipeline_get_config( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_get_config")]
        public static extern IntPtr ob_pipeline_get_config(IntPtr pipeline, [Out] out IntPtr error);

        //ob_frame_set* ob_pipeline_wait_for_frames( ob_pipeline* pipeline, uint32_t timeout_ms, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_wait_for_frames")]
        public static extern IntPtr ob_pipeline_wait_for_frames(IntPtr pipeline, UInt32 timeoutMs, [Out] out IntPtr error);

        //ob_device* ob_pipeline_get_device( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_get_device")]
        public static extern IntPtr ob_pipeline_get_device(IntPtr pipeline, [Out] out IntPtr error);

        //ob_stream_profile** ob_pipeline_get_stream_profiles( ob_pipeline* pipeline, ob_sensor_type sensor_type, uint32_t* profile_count, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_get_stream_profiles")]
        public static extern IntPtr ob_pipeline_get_stream_profiles(IntPtr pipeline, SensorType sensorType, ref UInt32 profileCount, [Out] out IntPtr error);

        //ob_stream_profile** ob_pipeline_get_all_stream_profiles( ob_pipeline* pipeline, uint32_t* profile_count, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_get_all_stream_profiles")]
        public static extern IntPtr ob_pipeline_get_all_stream_profiles(IntPtr pipeline, ref UInt32 profileCount, [Out] out IntPtr error);

        //void ob_pipeline_enable_frame_sync( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_enable_frame_sync")]
        public static extern void ob_pipeline_enable_frame_sync(IntPtr pipeline, [Out] out IntPtr error);

        //void ob_pipeline_disable_frame_sync( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_disable_frame_sync")]
        public static extern void ob_pipeline_disable_frame_sync(IntPtr pipeline, [Out] out IntPtr error);

        //ob_config* ob_create_config( ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_create_config")]
        public static extern IntPtr ob_create_config([Out] out IntPtr error);

        //void ob_delete_config( ob_config* config, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_config")]
        public static extern void ob_delete_config(IntPtr config, [Out] out IntPtr error);
        
        //void ob_config_enable_stream( ob_config* config, ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_config_enable_stream")]
        public static extern void ob_config_enable_stream(IntPtr config, IntPtr profile, [Out] out IntPtr error);

        //void ob_config_enable_all_stream( ob_config* config, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_config_enable_all_stream")]
        public static extern void ob_config_enable_all_stream(IntPtr config, [Out] out IntPtr error);

        //void ob_config_disable_stream( ob_config* config, ob_stream_type type, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_config_disable_stream")]
        public static extern void ob_config_disable_stream(IntPtr config, StreamType streamType, [Out] out IntPtr error);

        //void ob_config_disable_all_stream( ob_config* config, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_config_disable_all_stream")]
        public static extern void ob_config_disable_all_stream(IntPtr config, [Out] out IntPtr error);

        #if filter
        //ob_filter* ob_pipeline_create_pointcloud_filter( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_create_pointcloud_filter")]
        public static extern IntPtr ob_pipeline_create_pointcloud_filter(IntPtr pipeline, [Out] out IntPtr error);

        //void ob_pipeline_delete_pointcloud_filter( ob_pipeline* pipeline, ob_filter* filter, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_delete_pointcloud_filter")]
        public static extern void ob_pipeline_delete_pointcloud_filter(IntPtr pipeline, IntPtr filter, [Out] out IntPtr error);

        //void ob_pointcloud_filter_set_camera_parameter( ob_filter* filter, CAMERA_PARA param, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pointcloud_filter_set_camera_parameter")]
        public static extern void ob_pointcloud_filter_set_camera_parameter(IntPtr filter, CameraParam param, [Out] out IntPtr error);

        //void ob_pointcloud_filter_set_point_format( ob_filter* filter, ob_format type, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pointcloud_filter_set_point_format")]
        public static extern void ob_pointcloud_filter_set_point_format(IntPtr filter, Format format, [Out] out IntPtr error);

        //void ob_pointcloud_filter_enable_aligned_mode( ob_filter* filter, bool status, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pointcloud_filter_enable_aligned_mode")]
        public static extern void ob_pointcloud_filter_enable_aligned_mode(IntPtr filter, bool status, [Out] out IntPtr error);

        //ob_filter* ob_pipeline_create_format_convert_filter( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_create_format_convert_filter")]
        public static extern IntPtr ob_pipeline_create_format_convert_filter(IntPtr pipeline, [Out] out IntPtr error);

        //void ob_format_convert_filter_set_format( ob_filter* filter, ob_convert_format type, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_format_convert_filter_set_format")]
        public static extern void ob_format_convert_filter_set_format(IntPtr filter, [Out] out IntPtr error);

        //void ob_pipeline_delete_format_convert_filter( ob_pipeline* pipeline, ob_filter* filter, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_delete_format_convert_filter")]
        public static extern void ob_pipeline_delete_format_convert_filter(IntPtr pipeline, IntPtr filter, [Out] out IntPtr error);

        //void ob_filter_reset( ob_filter* filter, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_reset")]
        public static extern void ob_filter_reset(IntPtr filter, [Out] out IntPtr error);

        //ob_frame* ob_filter_process( ob_filter* filter, ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_process")]
        public static extern IntPtr ob_filter_process(IntPtr filter, IntPtr frame, [Out] out IntPtr error);

        //bool ob_filter_start( ob_filter* filter, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_start")]
        public static extern bool ob_filter_start(IntPtr filter, [Out] out IntPtr error);

        //void ob_filter_stop( ob_filter* filter, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_stop")]
        public static extern void ob_filter_stop(IntPtr filter, [Out] out IntPtr error);

        //void ob_filter_set_callback( ob_filter* filter, ob_filter_callback callback, void* user_data, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_set_callback")]
        public static extern void ob_filter_set_callback(IntPtr filter, [MarshalAs(UnmanagedType.FunctionPtr)] FilterCallback callback, IntPtr userData, [Out] out IntPtr error);

        //void ob_filter_push_frame( ob_filter* filter, ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_push_frame")]
        public static extern void ob_filter_push_frame(IntPtr filter, IntPtr frame, [Out] out IntPtr error);
        #endif
        #endregion

        #region  Sensor
        //ob_sensor_type ob_sensor_get_type( ob_sensor* sensor, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_type")]
        public static extern SensorType ob_sensor_get_type(IntPtr sensor, [Out] out IntPtr error);

        //void ob_sensor_set_bool_property( ob_sensor* sensor, ob_global_unified_property property_id, bool property, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_set_bool_property")]
        public static extern void ob_sensor_set_bool_property(IntPtr sensor, PropertyId propertyId, bool property, [Out] out IntPtr error);

        //bool ob_sensor_get_bool_property( ob_sensor* sensor, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_bool_property")]
        public static extern bool ob_sensor_get_bool_property(IntPtr sensor, PropertyId propertyId, [Out] out IntPtr error);

        //void ob_sensor_set_int_property( ob_sensor* sensor, ob_global_unified_property property_id, int32_t property, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_set_int_property")]
        public static extern void ob_sensor_set_int_property(IntPtr sensor, PropertyId propertyId, Int32 property, [Out] out IntPtr error);

        //int32_t ob_sensor_get_int_property( ob_sensor* sensor, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_int_property")]
        public static extern Int32 ob_sensor_get_int_property(IntPtr sensor, PropertyId propertyId, [Out] out IntPtr error);

        //void ob_sensor_set_float_property( ob_sensor* sensor, ob_global_unified_property property_id, float property, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_set_float_property")]
        public static extern void ob_sensor_set_float_property(IntPtr sensor, PropertyId propertyId, float property, [Out] out IntPtr error);

        //float ob_sensor_get_float_property( ob_sensor* sensor, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_float_property")]
        public static extern float ob_sensor_get_float_property(IntPtr sensor, PropertyId propertyId, [Out] out IntPtr error);

        //void ob_sensor_set_structured_data( ob_sensor* sensor, ob_global_unified_property property_id, const void* data, uint32_t data_size, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_set_structured_data")]
        public static extern void ob_sensor_set_structured_data(IntPtr sensor, PropertyId propertyId, IntPtr data, UInt32 dataSize, [Out] out IntPtr error);

        //void ob_sensor_get_structured_data( ob_sensor* sensor, ob_global_unified_property property_id, void* data, uint32_t* data_size, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_structured_data")]
        public static extern void ob_sensor_get_structured_data(IntPtr sensor, PropertyId propertyId, IntPtr data, ref UInt32 dataSize, [Out] out IntPtr error);

        //ob_int_property_range ob_sensor_get_int_property_range( ob_sensor* sensor, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_int_property_range")]
        public static extern void ob_sensor_get_int_property_range(out IntPropertyRange range, IntPtr sensor, PropertyId propertyId, [Out] out IntPtr error);

        //ob_float_property_range ob_sensor_get_float_property_range( ob_sensor* sensor, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_float_property_range")]
        public static extern void ob_sensor_get_float_property_range(out FloatPropertyRange range, IntPtr sensor, PropertyId propertyId, [Out] out IntPtr error);

        //ob_bool_property_range ob_sensor_get_bool_property_range( ob_sensor* sensor, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_bool_property_range")]
        public static extern void ob_sensor_get_bool_property_range(out BoolPropertyRange range, IntPtr sensor, PropertyId propertyId, [Out] out IntPtr error);

        //uint32_t ob_sensor_get_supported_property_count( ob_sensor* sensor, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_supported_property_count")]
        public static extern UInt32 ob_sensor_get_supported_property_count(IntPtr sensor, [Out] out IntPtr error);

        //ob_global_unified_property_item ob_sensor_get_supported_property( ob_sensor* sensor, uint32_t index, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_supported_property")]
        public static extern PropertyId ob_sensor_get_supported_property(IntPtr sensor, UInt32 index, [Out] out IntPtr error);

        //bool ob_sensor_is_property_supported( ob_sensor* sensor, ob_global_unified_property property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_is_property_supported")]
        public static extern bool ob_sensor_is_property_supported(IntPtr sensor, PropertyId propertyId, [Out] out IntPtr error);

        //ob_stream_profile** ob_sensor_get_stream_profiles( ob_sensor* sensor, uint32_t* count, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_stream_profiles")]
        public static extern IntPtr ob_sensor_get_stream_profiles(IntPtr sensor, ref UInt32 count, [Out] out IntPtr error);

        //void ob_sensor_start( ob_sensor* sensor, ob_stream_profile* profile, ob_frame_callback callback, void* user_data, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_start")]
        public static extern void ob_sensor_start(IntPtr sensor, IntPtr profile, [MarshalAs(UnmanagedType.FunctionPtr)] FrameCallbackInternal callback, IntPtr userData, [Out] out IntPtr error);
        
        //void ob_sensor_stop( ob_sensor* sensor, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_stop")]
        public static extern void ob_sensor_stop(IntPtr sensor, [Out] out IntPtr error);

        //void ob_delete_sensor_list( ob_sensor_list* sensors, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_sensor_list")]
        public static extern void ob_delete_sensor_list(IntPtr sensors, [Out] out IntPtr error);

        //uint32_t ob_sensor_list_get_sensor_count( ob_sensor_list* sensors, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_list_get_sensor_count")]
        public static extern UInt32 ob_sensor_list_get_sensor_count(IntPtr sensors, [Out] out IntPtr error);

        //ob_sensor_type ob_sensor_list_get_sensor_type( ob_sensor_list* sensors, uint32_t index, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_list_get_sensor_type")]
        public static extern SensorType ob_sensor_list_get_sensor_type(IntPtr sensors, UInt32 index, [Out] out IntPtr error);

        //ob_sensor* ob_get_sensor_by_type( ob_sensor_list* sensors, ob_sensor_type sensorType, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_get_sensor_by_type")]
        public static extern IntPtr ob_get_sensor_by_type(IntPtr sensors, SensorType sensorType, [Out] out IntPtr error);

        //ob_sensor* ob_get_sensor( ob_sensor_list* sensors, uint32_t index, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_get_sensor")]
        public static extern IntPtr ob_get_sensor(IntPtr sensors, UInt32 index, [Out] out IntPtr error);

        //void ob_delete_sensor( ob_sensor* sensor, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_sensor")]
        public static extern void ob_delete_sensor(IntPtr sensor, [Out] out IntPtr error);
        #endregion

        #region StreamProfile
        //ob_format ob_stream_profile_format( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_stream_profile_format")]
        public static extern Format ob_stream_profile_format(IntPtr profile, [Out] out IntPtr error);

        //ob_stream_type ob_stream_profile_type( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_stream_profile_type")]
        public static extern StreamType ob_stream_profile_type(IntPtr profile, [Out] out IntPtr error);

        //uint32_t ob_video_stream_profile_fps( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_video_stream_profile_fps")]
        public static extern UInt32 ob_video_stream_profile_fps(IntPtr profile, [Out] out IntPtr error);

        //uint32_t ob_video_stream_profile_width( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_video_stream_profile_width")]
        public static extern UInt32 ob_video_stream_profile_width(IntPtr profile, [Out] out IntPtr error);
        
        //uint32_t ob_video_stream_profile_height( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_video_stream_profile_height")]
        public static extern UInt32 ob_video_stream_profile_height(IntPtr profile, [Out] out IntPtr error);

        //ob_accel_full_scale_range ob_accel_stream_profile_full_scale_range( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_accel_stream_profile_full_scale_range")]
        public static extern AccelFullScaleRange ob_accel_stream_profile_full_scale_range(IntPtr profile, [Out] out IntPtr error);

        //ob_accel_sample_rate ob_accel_stream_profile_sample_rate( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_accel_stream_profile_sample_rate")]
        public static extern AccelSampleRate ob_accel_stream_profile_sample_rate(IntPtr profile, [Out] out IntPtr error);

        //ob_gyro_full_scale_range ob_gyro_stream_profile_full_scale_range( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_gyro_stream_profile_full_scale_range")]
        public static extern GyroFullScaleRange ob_gyro_stream_profile_full_scale_range(IntPtr profile, [Out] out IntPtr error);

        //ob_gyro_sample_rate ob_gyro_stream_profile_sample_rate( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_gyro_stream_profile_sample_rate")]
        public static extern GyroSampleRate ob_gyro_stream_profile_sample_rate(IntPtr profile, [Out] out IntPtr error);

        //void ob_delete_stream_profiles( ob_stream_profile** profiles, uint32_t count, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_stream_profiles")]
        public static extern void ob_delete_stream_profiles(IntPtr profiles, UInt32 count, [Out] out IntPtr error);

        //void ob_delete_stream_profile( ob_stream_profile* profile, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_stream_profile")]
        public static extern void ob_delete_stream_profile(IntPtr profile, [Out] out IntPtr error);
        #endregion

        #region Version
        //int ob_get_version();
        [DllImport(obsdk, EntryPoint = "ob_get_version")]
        public static extern int ob_get_version();

        //int ob_get_major_version();
        [DllImport(obsdk, EntryPoint = "ob_get_major_version")]
        public static extern int ob_get_major_version();

        //int ob_get_minor_version();
        [DllImport(obsdk, EntryPoint = "ob_get_minor_version")]
        public static extern int ob_get_minor_version();

        //int ob_get_patch_version();
        [DllImport(obsdk, EntryPoint = "ob_get_patch_version")]
        public static extern int ob_get_patch_version();
        #endregion
    }
}