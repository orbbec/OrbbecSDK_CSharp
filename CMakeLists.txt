cmake_minimum_required(VERSION 3.8.2)

project(ob_csharp VERSION 0.1.0 LANGUAGES CSharp)

include(CSharpUtilities)

add_library(${PROJECT_NAME} SHARED 
    src/Config.cs
    src/Context.cs
    src/Device.cs
    src/DeviceInfo.cs
    src/DeviceList.cs
    src/Error.cs
    src/Frame.cs
    src/NativeHandle.cs
    src/NativeException.cs
    src/obNative.cs
    src/Pipeline.cs
    src/Sensor.cs
    src/SensorList.cs
    src/StreamProfile.cs
    src/Types.cs
    src/Version.cs
)

set_property(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v3.5")

add_subdirectory(samples)
add_subdirectory(tests)