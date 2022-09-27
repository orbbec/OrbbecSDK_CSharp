using System;
using System.Runtime.InteropServices;

namespace Orbbec
{
    internal class obNative
    {
#if ORBBEC_DEBUG
        public const string obsdk = "OrbbecSDK_d";
#else
        public const string obsdk = "OrbbecSDK";
#endif
        #region Context
        //ob_context *ob_create_context(ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_create_context")]
        public static extern IntPtr ob_create_context([Out] out IntPtr error);

        //ob_context *ob_create_context_with_config(const char *config_path, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_create_context_with_config")]
        public static extern IntPtr ob_create_context_with_config(String configPath, [Out] out IntPtr error);

        //void ob_delete_context(ob_context *context, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_delete_context")]
        public static extern void ob_delete_context(IntPtr context, [Out] out IntPtr error);

        //ob_device_list *ob_query_device_list(ob_context *context, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_query_device_list")]
        public static extern IntPtr ob_query_device_list(IntPtr context, [Out] out IntPtr error);

        //ob_device *ob_create_net_device(ob_context *context, const char *address, uint16_t port, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_create_net_device")]
        public static extern IntPtr ob_create_net_device(IntPtr context, String address, UInt16 port, [Out] out IntPtr error);

        //void ob_set_device_changed_callback(ob_context *context, ob_device_changed_callback callback, void *user_data, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_set_device_changed_callback")]
        public static extern void ob_set_device_changed_callback(IntPtr context, [MarshalAs(UnmanagedType.FunctionPtr)] DeviceChangedCallbackInternal callback, IntPtr userData, [Out] out IntPtr error);

        //void ob_enable_multi_device_sync(ob_context *context, uint64_t repeatInterval, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_enable_multi_device_sync")]
        public static extern void ob_enable_multi_device_sync(IntPtr context, UInt64 repeatInterval, [Out] out IntPtr error);

        //void ob_set_logger_severity(ob_log_severity severity, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_set_logger_serverity")]
        public static extern void ob_set_logger_serverity(LogSeverity logSeverity, [Out] out IntPtr error);

        //void ob_set_logger_to_file(ob_log_severity severity, const char *directory, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_set_logger_to_file")]
        public static extern void ob_set_logger_to_file(LogSeverity logSeverity, String fileName, [Out] out IntPtr error);

        //void ob_set_logger_to_console(ob_log_severity severity, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_set_logger_to_console")]
        public static extern void ob_set_logger_to_console(LogSeverity logSeverity, [Out] out IntPtr error);
        #endregion

        #region Device
        //uint32_t ob_device_list_device_count(ob_device_list *list, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_list_device_count")]
        public static extern UInt32 ob_device_list_count(IntPtr deviceList, [Out] out IntPtr error);

        //const char *ob_device_list_get_device_name(ob_device_list *list, uint32_t index, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_name")]
        public static extern IntPtr ob_device_list_get_device_name(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        //int ob_device_list_get_device_pid(ob_device_list *list, uint32_t index, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_pid")]
        public static extern int ob_device_list_get_device_pid(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        //int ob_device_list_get_device_vid(ob_device_list *list, uint32_t index, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_vid")]
        public static extern int ob_device_list_get_device_vid(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        //const char *ob_device_list_get_device_uid(ob_device_list *list, uint32_t index, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_uid")]
        public static extern IntPtr ob_device_list_get_device_uid(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        //const char *ob_device_list_get_device_serial_number(ob_device_list *list, uint32_t index, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_serial_number")]
        public static extern IntPtr ob_device_list_get_device_serial_number(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        //ob_device *ob_device_list_get_device(ob_device_list *list, uint32_t index, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device")]
        public static extern IntPtr ob_device_list_get_device(IntPtr deviceList, UInt32 index, [Out] out IntPtr error);

        //ob_device *ob_device_list_get_device_by_serial_number(ob_device_list *list, const char *serial_number, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_by_serial_number")]
        public static extern IntPtr ob_device_list_get_device_by_serial_number(IntPtr deviceList, String serialNumber, [Out] out IntPtr error);

        //ob_device *ob_device_list_get_device_by_uid(ob_device_list *list, const char *uid, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_list_get_device_by_uid")]
        public static extern IntPtr ob_device_list_get_device_by_uid(IntPtr deviceList, String uid, [Out] out IntPtr error);

        //void ob_delete_device(ob_device *device, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_delete_device")]
        public static extern void ob_delete_device(IntPtr device, [Out] out IntPtr error);

        //void ob_delete_device_info(ob_device_info *info, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_delete_device_info")]
        public static extern void ob_delete_device_info(IntPtr deviceInfo, [Out] out IntPtr error);

        //void ob_delete_device_list(ob_device_list *list, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_delete_device_list")]
        public static extern void ob_delete_device_list(IntPtr deviceList, [Out] out IntPtr error);

        //ob_device_info *ob_device_get_device_info(ob_device *device, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_device_info")]
        public static extern IntPtr ob_device_get_device_info(IntPtr device, [Out] out IntPtr error);

        //ob_sensor_list *ob_device_get_sensor_list(ob_device *device, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_sensor_list")]
        public static extern IntPtr ob_device_get_sensor_list(IntPtr device, [Out] out IntPtr error);

        //ob_sensor *ob_device_get_sensor(ob_device *device, ob_sensor_type type, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_sensor")]
        public static extern IntPtr ob_device_get_sensor(IntPtr device, SensorType sensorType, [Out] out IntPtr error);

        //void ob_device_set_int_property(ob_device *device, ob_property_id property_id, int32_t property, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_set_int_property")]
        public static extern void ob_device_set_int_property(IntPtr device, PropertyId propertyId, Int32 property, [Out] out IntPtr error);

        //int32_t ob_device_get_int_property(ob_device *device, ob_property_id property_id, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_int_property")]
        public static extern Int32 ob_device_get_int_property(IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //void ob_device_set_float_property(ob_device *device, ob_property_id property_id, float property, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_set_float_property")]
        public static extern void ob_device_set_float_property(IntPtr device, PropertyId propertyId, float property, [Out] out IntPtr error);

        //float ob_device_get_float_property(ob_device *device, ob_property_id property_id, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_float_property")]
        public static extern float ob_device_get_float_property(IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //void ob_device_set_bool_property(ob_device *device, ob_property_id property_id, bool property, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_set_bool_property")]
        public static extern void ob_device_set_bool_property(IntPtr device, PropertyId propertyId, bool property, [Out] out IntPtr error);

        //bool ob_device_get_bool_property(ob_device *device, ob_property_id property_id, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_bool_property")]
        public static extern bool ob_device_get_bool_property(IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //void ob_device_set_structured_data( ob_device* device, ob_global_unified_property property_id, const void* data, uint32_t data_size, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_set_structured_data")]
        public static extern void ob_device_set_structured_data(IntPtr device, PropertyId propertyId, IntPtr data, UInt32 dataSize, [Out] out IntPtr error);

        //void ob_device_get_structured_data( ob_device* device, ob_global_unified_property property_id, void* data, uint32_t* data_size, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_structured_data")]
        public static extern void ob_device_get_structured_data(IntPtr device, PropertyId propertyId, IntPtr data, ref UInt32 dataSize, [Out] out IntPtr error);

        //void ob_device_set_raw_data(ob_device *device, ob_property_id property_id, void *data, uint32_t data_size, ob_set_data_callback cb, bool async, void *user_data, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_set_raw_data")]
        public static extern void ob_device_set_raw_data(IntPtr device, PropertyId propertyId, IntPtr data, UInt32 dataSize, [MarshalAs(UnmanagedType.FunctionPtr)] SetDataCallbackInternal callback, bool async, IntPtr userData, [Out] out IntPtr error);

        //void ob_device_get_raw_data(ob_device *device, ob_property_id property_id, ob_get_data_callback cb, bool async, void *user_data, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_raw_data")]
        public static extern void ob_device_get_raw_data(IntPtr device, PropertyId propertyId, [MarshalAs(UnmanagedType.FunctionPtr)] GetDataCallback callback, bool async, IntPtr userData, [Out] out IntPtr error);

        //uint32_t ob_device_get_supported_property_count(ob_device *device, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_supported_property_count")]
        public static extern UInt32 ob_device_get_supported_property_count(IntPtr device, [Out] out IntPtr error);

        //ob_property_item ob_device_get_supported_property(ob_device *device, uint32_t index, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_supported_property")]
        public static extern void ob_device_get_supported_property(out PropertyItem item, IntPtr device, UInt32 index, [Out] out IntPtr error);

        //bool ob_device_is_property_supported(ob_device *device, ob_property_id property_id, ob_permission_type permission, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_is_property_supported")]
        public static extern bool ob_device_is_property_supported(IntPtr device, PropertyId propertyId, PermissionType permissionType, [Out] out IntPtr error);

        //ob_int_property_range ob_device_get_int_property_range( ob_device* device, ob_property_id property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_int_property_range")]
        public static extern void ob_device_get_int_property_range(out IntPropertyRange range, IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //ob_float_property_range ob_device_get_float_property_range( ob_device* device, ob_property_id property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_float_property_range")]
        public static extern void ob_device_get_float_property_range(out FloatPropertyRange range, IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //ob_bool_property_range ob_device_get_bool_property_range( ob_device* device, ob_property_id property_id, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_bool_property_range")]
        public static extern void ob_device_get_bool_property_range(out BoolPropertyRange range, IntPtr device, PropertyId propertyId, [Out] out IntPtr error);

        //void ob_device_write_ahb(ob_device *device, uint32_t reg, uint32_t mask, uint32_t value, ob_error **error);
        //void ob_device_read_ahb(ob_device *device, uint32_t reg, uint32_t mask, uint32_t *value, ob_error **error);
        //void ob_device_write_i2c(ob_device *device, uint32_t module_id, uint32_t reg, uint32_t mask, uint32_t value, ob_error **error);
        //void ob_device_read_i2c(ob_device *device, uint32_t module_id, uint32_t reg, uint32_t mask, uint32_t *value, ob_error **error);
        //void ob_device_write_flash(ob_device *device, uint32_t offset, const void *data, uint32_t data_size, ob_set_data_callback cb, bool async, void *user_data, ob_error **error);
        //void ob_device_read_flash(ob_device *device, uint32_t offset, uint32_t data_size, ob_get_data_callback cb, bool async, void *user_data, ob_error **error);
        //uint64_t ob_device_sync_device_time(ob_device *device, ob_error **error);
        //void ob_device_upgrade(ob_device *device, const char *path, ob_device_upgrade_callback callback, bool async, void *user_data, ob_error **error);

        //ob_device_state ob_device_get_device_state( ob_device* device, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_get_device_state")]
        public static extern UInt64 ob_device_get_device_state(IntPtr device, [Out] out IntPtr error);

        //void ob_device_state_changed( ob_device* device, ob_device_state_callback callback, void* user_data, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_state_changed")]
        public static extern void ob_device_state_changed(IntPtr device, [MarshalAs(UnmanagedType.FunctionPtr)] DeviceStateCallbackInternal callback, IntPtr userData, [Out] out IntPtr error);

        //void ob_device_send_file_to_destination(ob_device *device, const char *file_path, const char *dst_path, ob_file_send_callback callback, bool async, void *user_data, ob_error **error);

        //bool ob_device_activate_authorization( ob_device* device, const char* auth_code, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_activate_authorization")]
        public static extern bool ob_device_activate_authorization(IntPtr device, String authCode, [Out] out IntPtr error);

        //void ob_device_write_authorization_code( ob_device* device, const char* auth_code, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_device_write_authorization_code")]
        public static extern void ob_device_write_authorization_code(IntPtr device, String autoCode, [Out] out IntPtr error);

        //ob_camera_param_list *ob_device_get_calibration_camera_param_list(ob_device *device, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_device_get_calibration_camera_param_list")]
        public static extern IntPtr ob_device_get_calibration_camera_param_list(IntPtr device, [Out] out IntPtr error);

        //void ob_device_reboot(ob_device *device, ob_error **error);

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

        //const char *ob_device_info_connection_type(ob_device_info *info, ob_error **error);
        //const char *ob_device_info_hardware_version(ob_device_info *info, ob_error **error);
        //const char *ob_device_info_supported_min_sdk_version(ob_device_info *info, ob_error **error);
        //const char *ob_device_info_asicName(ob_device_info *info, ob_error **error);
        //ob_device_type ob_device_info_device_type(ob_device_info *info, ob_error **error);
        //uint32_t ob_camera_param_list_count(ob_camera_param_list *param_list, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_camera_param_list_count")]
        public static extern UInt32 ob_camera_param_list_count(IntPtr paramList, [Out] out IntPtr error);

        //ob_camera_param ob_camera_param_list_get_param(ob_camera_param_list *param_list, uint32_t index, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_camera_param_list_get_param")]
        public static extern void ob_camera_param_list_get_param(out CameraParam cameraParam, IntPtr paramList, UInt32 index, [Out] out IntPtr error);

        //void ob_delete_camera_param_list(ob_camera_param_list *param_list, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_delete_camera_param_list")]
        public static extern void ob_delete_camera_param_list(IntPtr paramList, [Out] out IntPtr error);

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

        #region Filter
        //ob_filter *ob_create_pointcloud_filter(ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_create_pointcloud_filter")]
        public static extern IntPtr ob_create_pointcloud_filter([Out] out IntPtr error);

        //void ob_pointcloud_filter_set_camera_param(ob_filter *filter, ob_camera_param param, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_pointcloud_filter_set_camera_param")]
        public static extern void ob_pointcloud_filter_set_camera_param(IntPtr filter, CameraParam param, [Out] out IntPtr error);

        //void ob_pointcloud_filter_set_point_format(ob_filter *filter, ob_format type, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_pointcloud_filter_set_point_format")]
        public static extern void ob_pointcloud_filter_set_point_format(IntPtr filter, Format format, [Out] out IntPtr error);

        //void ob_pointcloud_filter_set_frame_align_state(ob_filter *filter, bool state, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_pointcloud_filter_set_frame_align_state")]
        public static extern void ob_pointcloud_filter_set_frame_align_state(IntPtr filter, bool state, [Out] out IntPtr error);

        //ob_filter *ob_create_format_convert_filter(ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_create_format_convert_filter")]
        public static extern IntPtr ob_create_format_convert_filter([Out] out IntPtr error);

        //void ob_format_convert_filter_set_format(ob_filter *filter, ob_convert_format type, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_format_convert_filter_set_format")]
        public static extern void ob_format_convert_filter_set_format(IntPtr filter, ConvertFormat format, [Out] out IntPtr error);

        //void ob_filter_reset( ob_filter* filter, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_reset")]
        public static extern void ob_filter_reset(IntPtr filter, [Out] out IntPtr error);

        //ob_frame* ob_filter_process( ob_filter* filter, ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_process")]
        public static extern IntPtr ob_filter_process(IntPtr filter, IntPtr frame, [Out] out IntPtr error);

        //void ob_filter_set_callback( ob_filter* filter, ob_filter_callback callback, void* user_data, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_set_callback")]
        public static extern void ob_filter_set_callback(IntPtr filter, [MarshalAs(UnmanagedType.FunctionPtr)] FilterCallbackInternal callback, IntPtr userData, [Out] out IntPtr error);

        //void ob_filter_push_frame( ob_filter* filter, ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_filter_push_frame")]
        public static extern void ob_filter_push_frame(IntPtr filter, IntPtr frame, [Out] out IntPtr error);
        
        //void ob_delete_filter(ob_filter *filter, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_delete_filter")]
        public static extern void ob_delete_filter(IntPtr filter, [Out] out IntPtr error);
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

        //uint64_t ob_frame_time_stamp_us(ob_frame *frame, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_frame_time_stamp_us")]
        public static extern UInt64 ob_frame_time_stamp_us(IntPtr frame, [Out] out IntPtr error);

        //uint64_t ob_frame_system_time_stamp( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_system_time_stamp")]
        public static extern UInt64 ob_frame_system_time_stamp(IntPtr frame, [Out] out IntPtr error);

        //void* ob_frame_data( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_data")]
        public static extern IntPtr ob_frame_data(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_frame_data_size( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frame_data_size")]
        public static extern UInt32 ob_frame_data_size(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_video_frame_width( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_video_frame_width")]
        public static extern UInt32 ob_video_frame_width(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_video_frame_height( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_video_frame_height")]
        public static extern UInt32 ob_video_frame_height(IntPtr frame, [Out] out IntPtr error);

        //void* ob_video_frame_metadata( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_video_frame_metadata")]
        public static extern IntPtr ob_video_frame_metadata(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_video_frame_metadata_size( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_video_frame_metadata_size")]
        public static extern UInt32 ob_video_frame_metadata_size(IntPtr frame, [Out] out IntPtr error);

        //uint8_t ob_video_frame_pixel_available_bit_size(ob_frame *frame, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_video_frame_pixel_available_bit_size")]
        public static extern uint ob_video_frame_pixel_available_bit_size(IntPtr frame, [Out] out IntPtr error);

        //float ob_depth_frame_get_value_scale( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_depth_frame_get_value_scale")]
        public static extern float ob_depth_frame_get_value_scale(IntPtr frame, [Out] out IntPtr error);

        //void ob_delete_frame( ob_frame* frame, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_frame")]
        public static extern void ob_delete_frame(IntPtr frame, [Out] out IntPtr error);

        //uint32_t ob_frameset_frame_count( ob_frame* frameset, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frameset_frame_count")]
        public static extern UInt32 ob_frameset_frame_count(IntPtr frameset, [Out] out IntPtr error);

        //ob_frame* ob_frameset_depth_frame( ob_frame* frame_set, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frameset_depth_frame")]
        public static extern IntPtr ob_frameset_depth_frame(IntPtr frameset, [Out] out IntPtr error);

        //ob_frame* ob_frameset_color_frame( ob_frame* frame_set, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frameset_color_frame")]
        public static extern IntPtr ob_frameset_color_frame(IntPtr frameset, [Out] out IntPtr error);

        //ob_frame* ob_frameset_ir_frame( ob_frame* frame_set, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frameset_ir_frame")]
        public static extern IntPtr ob_frameset_ir_frame(IntPtr frameset, [Out] out IntPtr error);

        //ob_frame* ob_frameset_points_frame( ob_frame* frame_set, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_frameset_points_frame")]
        public static extern IntPtr ob_frameset_points_frame(IntPtr frameset, [Out] out IntPtr error);

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

        //ob_pipeline *ob_create_pipeline_with_playback_file(const char *file_name, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_create_pipeline_with_playback_file")]
        public static extern IntPtr ob_create_pipeline_with_playback_file(String fileName, [Out] out IntPtr error);

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

        //ob_frame* ob_pipeline_wait_for_frameset( ob_pipeline* pipeline, uint32_t timeout_ms, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_wait_for_frameset")]
        public static extern IntPtr ob_pipeline_wait_for_frameset(IntPtr pipeline, UInt32 timeoutMs, [Out] out IntPtr error);

        //ob_device* ob_pipeline_get_device( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_get_device")]
        public static extern IntPtr ob_pipeline_get_device(IntPtr pipeline, [Out] out IntPtr error);

        //ob_playback *ob_pipeline_get_playback(ob_pipeline *pipeline, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_pipeline_get_playback")]
        public static extern IntPtr ob_pipeline_get_playback(IntPtr pipeline, [Out] out IntPtr error);

        //ob_stream_profile_list* ob_pipeline_get_stream_profile_list( ob_pipeline* pipeline, ob_sensor_type sensor_type, uint32_t* profile_count, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_get_stream_profile_list")]
        public static extern IntPtr ob_pipeline_get_stream_profile_list(IntPtr pipeline, SensorType sensorType, [Out] out IntPtr error);

        //void ob_pipeline_enable_frame_sync( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_enable_frame_sync")]
        public static extern void ob_pipeline_enable_frame_sync(IntPtr pipeline, [Out] out IntPtr error);

        //void ob_pipeline_disable_frame_sync( ob_pipeline* pipeline, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_pipeline_disable_frame_sync")]
        public static extern void ob_pipeline_disable_frame_sync(IntPtr pipeline, [Out] out IntPtr error);

        //void ob_pipeline_switch_config(ob_pipeline *pipeline, ob_config *config, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_pipeline_switch_config")]
        public static extern void ob_pipeline_switch_config(IntPtr pipeline, IntPtr config, [Out] out IntPtr error);

        //ob_camera_param ob_pipeline_get_camera_param(ob_pipeline *pipeline, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_pipeline_get_camera_param")]
        public static extern void ob_pipeline_get_camera_param(out CameraParam cameraParam, IntPtr pipeline, [Out] out IntPtr error);
        
        //ob_stream_profile_list *ob_get_d2c_depth_profile_list(ob_pipeline *pipeline, ob_stream_profile *color_profile,ob_align_mode align_mode, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_get_d2c_depth_profile_list")]
        public static extern IntPtr ob_get_d2c_depth_profile_list(IntPtr pipeline, IntPtr colorProfile, AlignMode alignMode, [Out] out IntPtr error);
        
        //ob_rect ob_get_d2c_range_valid_area(ob_pipeline *pipeline, uint32_t minimum_distance,uint32_t maximum_distance, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_get_d2c_range_valid_area")]
        public static extern void ob_get_d2c_range_valid_area(out Rect rect, IntPtr pipeline, UInt32 minDistance, UInt32 maxDistance, [Out] out IntPtr error);
        
        //void ob_pipeline_start_record(ob_pipeline *pipeline, const char *file_name, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_pipeline_start_record")]
        public static extern void ob_pipeline_start_record(IntPtr pipeline, String fileName, [Out] out IntPtr error);
        
        //void ob_pipeline_stop_record(ob_pipeline *pipeline, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_pipeline_stop_record")]
        public static extern void ob_pipeline_stop_record(IntPtr pipeline, [Out] out IntPtr error);

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

        //void ob_config_set_align_mode(ob_config *config, ob_align_mode mode, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_config_set_align_mode")]
        public static extern void ob_config_set_align_mode(IntPtr config, AlignMode mode, [Out] out IntPtr error);
        
        //void ob_config_set_depth_scale_require(ob_config *config,bool enable,ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_config_set_depth_scale_require")]
        public static extern void ob_config_set_depth_scale_require(IntPtr config, bool enable, [Out] out IntPtr error);

        //void ob_config_set_d2c_target_resolution(ob_config *config,uint32_t d2c_target_width,uint32_t d2c_target_height,ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_config_set_d2c_target_resolution")]
        public static extern void ob_config_set_d2c_target_resolution(IntPtr config, UInt32 width, UInt32 height, [Out] out IntPtr error);
        #endregion

        #region Record
        //ob_recorder *ob_create_recorder(ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_create_recorder")]
        public static extern IntPtr ob_create_recorder([Out] out IntPtr error);

        //ob_recorder *ob_create_recorder_with_device(ob_device *dev, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_create_recorder_with_device")]
        public static extern IntPtr ob_create_recorder_with_device(IntPtr dev, [Out] out IntPtr error);

        //void ob_delete_recorder(ob_recorder *recorder, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_delete_recorder")]
        public static extern void ob_delete_recorder(IntPtr recorder, [Out] out IntPtr error);
        
        //void ob_recorder_start(ob_recorder *recorder, const char *filename, bool async, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_recorder_start")]
        public static extern void ob_recorder_start(IntPtr recorder, String fileName, bool async, [Out] out IntPtr error);

        //void ob_recorder_stop(ob_recorder *recorder, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_recorder_stop")]
        public static extern void ob_recorder_stop(IntPtr recorder, [Out] out IntPtr error);

        //void ob_recorder_write_frame(ob_recorder *recorder, ob_frame *frame, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_recorder_write_frame")]
        public static extern void ob_recorder_write_frame(IntPtr recorder, IntPtr frame, [Out] out IntPtr error);
        #endregion

        #region Playback
        //ob_playback *ob_create_playback(const char *filename, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_create_playback")]
        public static extern IntPtr ob_create_playback(String fileName, [Out] out IntPtr error);

        //void ob_delete_playback(ob_playback *playback, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_delete_playback")]
        public static extern void ob_delete_playback(IntPtr playback, [Out] out IntPtr error);

        //void ob_playback_start(ob_playback *playback, ob_playback_callback callback, void *user_data, ob_media_type type, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_playback_start")]
        public static extern void ob_playback_start(IntPtr playback, [MarshalAs(UnmanagedType.FunctionPtr)] PlaybackCallbackInternal callback, IntPtr userData, MediaType mediaType, [Out] out IntPtr error);

        //void ob_playback_stop(ob_playback *playback, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_playback_stop")]
        public static extern void ob_playback_stop(IntPtr playback, [Out] out IntPtr error);

        //void ob_set_playback_state_callback(ob_playback *playback, ob_media_state_callback callback, void *user_data, ob_error **error);
        
        //ob_device_info *ob_playback_get_device_info(ob_playback *playback, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_playback_get_device_info")]
        public static extern IntPtr ob_playback_get_device_info(IntPtr playback, [Out] out IntPtr error);

        //ob_camera_param ob_playback_get_camera_param(ob_playback *playback, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_playback_get_camera_param")]
        public static extern void ob_playback_get_camera_param(out CameraParam cameraParam, IntPtr playback, [Out] out IntPtr error);
        #endregion

        #region Sensor
        //ob_sensor_type ob_sensor_get_type( ob_sensor* sensor, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_type")]
        public static extern SensorType ob_sensor_get_type(IntPtr sensor, [Out] out IntPtr error);

        //ob_stream_profile_list* ob_sensor_get_stream_profile_list( ob_sensor* sensor, uint32_t* count, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_get_stream_profile_list")]
        public static extern IntPtr ob_sensor_get_stream_profile_list(IntPtr sensor, [Out] out IntPtr error);

        //void ob_sensor_start( ob_sensor* sensor, ob_stream_profile* profile, ob_frame_callback callback, void* user_data, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_start")]
        public static extern void ob_sensor_start(IntPtr sensor, IntPtr profile, [MarshalAs(UnmanagedType.FunctionPtr)] FrameCallbackInternal callback, IntPtr userData, [Out] out IntPtr error);
        
        //void ob_sensor_stop( ob_sensor* sensor, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_stop")]
        public static extern void ob_sensor_stop(IntPtr sensor, [Out] out IntPtr error);

        //void ob_sensor_switch_profile(ob_sensor *sensor, ob_stream_profile *profile, ob_error **error);

        //void ob_delete_sensor_list( ob_sensor_list* sensors, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_sensor_list")]
        public static extern void ob_delete_sensor_list(IntPtr sensors, [Out] out IntPtr error);

        //uint32_t ob_sensor_list_get_sensor_count( ob_sensor_list* sensors, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_list_get_sensor_count")]
        public static extern UInt32 ob_sensor_list_get_sensor_count(IntPtr sensors, [Out] out IntPtr error);

        //ob_sensor_type ob_sensor_list_get_sensor_type( ob_sensor_list* sensors, uint32_t index, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_list_get_sensor_type")]
        public static extern SensorType ob_sensor_list_get_sensor_type(IntPtr sensors, UInt32 index, [Out] out IntPtr error);

        //ob_sensor* ob_sensor_list_get_sensor_by_type( ob_sensor_list* sensors, ob_sensor_type sensorType, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_list_get_sensor_by_type")]
        public static extern IntPtr ob_sensor_list_get_sensor_by_type(IntPtr sensors, SensorType sensorType, [Out] out IntPtr error);

        //ob_sensor* ob_sensor_list_get_sensor( ob_sensor_list* sensors, uint32_t index, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_sensor_list_get_sensor")]
        public static extern IntPtr ob_sensor_list_get_sensor(IntPtr sensors, UInt32 index, [Out] out IntPtr error);

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

        //ob_stream_profile *ob_stream_profile_list_get_video_stream_profile(ob_stream_profile_list *profile_list, int width, int height, ob_format format, int fps, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_stream_profile_list_get_video_stream_profile")]
        public static extern IntPtr ob_stream_profile_list_get_video_stream_profile(IntPtr profileList, int width, int height, Format format, int fps, [Out] out IntPtr error);
        
        //ob_stream_profile *ob_stream_profile_list_get_profile(ob_stream_profile_list *profile_list, int index, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_stream_profile_list_get_profile")]
        public static extern IntPtr ob_stream_profile_list_get_profile(IntPtr profileList, int index, [Out] out IntPtr error);
        
        //uint32_t ob_stream_profile_list_count(ob_stream_profile_list *profile_list, ob_error **error);
        [DllImport(obsdk, EntryPoint = "ob_stream_profile_list_count")]
        public static extern UInt32 ob_stream_profile_list_count(IntPtr profileList, [Out] out IntPtr error);

        //void ob_delete_stream_profile_list( ob_stream_profile** profiles, uint32_t count, ob_error** error );
        [DllImport(obsdk, EntryPoint = "ob_delete_stream_profile_list")]
        public static extern void ob_delete_stream_profile_list(IntPtr profiles, [Out] out IntPtr error);

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