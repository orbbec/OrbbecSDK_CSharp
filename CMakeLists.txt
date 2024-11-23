cmake_minimum_required(VERSION 3.8.2)

project(ob_csharp VERSION 0.1.0 LANGUAGES CSharp)

include(CSharpUtilities)

file(GLOB_RECURSE SOURCES src/*.cs)

# add_library(${PROJECT_NAME} SHARED
#     src/CameraParamList.cs
#     src/Config.cs
#     src/Context.cs
#     src/DepthWorkModeList.cs
#     src/Device.cs
#     src/DeviceInfo.cs
#     src/DeviceList.cs
#     src/CameraParamList.cs
#     src/Error.cs
#     src/Filter.cs
#     src/FilterConfigSchemaList.cs
#     src/Frame.cs
#     src/NativeException.cs
#     src/NativeHandle.cs
#     src/obNative.cs
#     src/Pipeline.cs
#     src/Sensor.cs
#     src/SensorList.cs
#     src/StreamProfile.cs
#     src/StreamProfileList.cs
#     src/Recorder.cs
#     src/Playback.cs
#     src/Types.cs
#     src/Version.cs
# )

add_library(${PROJECT_NAME} SHARED ${SOURCES})

set_property(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION 4.7.2)

set(OB_LIB_DIR ${CMAKE_CURRENT_LIST_DIR}/OrbbecSDK/lib)
message("OB_LIB_DIR: ${OB_LIB_DIR}")

add_subdirectory(samples)

file(GLOB OB_SDK_LIB_FILES ${CMAKE_CURRENT_LIST_DIR}/OrbbecSDK/lib/OrbbecSDK.dll)
file(GLOB OB_SDK_EXT_DIR ${CMAKE_CURRENT_LIST_DIR}/OrbbecSDK/lib/extensions)
file(GLOB OB_SDK_CONFIG_FILES ${CMAKE_CURRENT_LIST_DIR}/OrbbecSDK/config/OrbbecSDKConfig.*)

install (FILES ${OB_SDK_LIB_FILES} DESTINATION bin)
install (DIRECTORY ${OB_SDK_EXT_DIR} DESTINATION bin)
install (FILES ${OB_SDK_CONFIG_FILES} DESTINATION bin)

install (FILES ${OB_SDK_LIB_FILES} DESTINATION lib)
install (DIRECTORY ${OB_SDK_EXT_DIR} DESTINATION lib)
install (FILES ${OB_SDK_CONFIG_FILES} DESTINATION lib)

install(TARGETS ${PROJECT_NAME}
    RUNTIME DESTINATION bin
    LIBRARY DESTINATION lib
    ARCHIVE DESTINATION lib
)