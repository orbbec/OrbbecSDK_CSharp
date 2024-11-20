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

add_subdirectory(samples)