cmake_minimum_required(VERSION 3.8.2)

project(ob_csharp VERSION 0.1.0 LANGUAGES CSharp)
set(CMAKE_INSTALL_PREFIX ${CMAKE_BINARY_DIR}/install)
include(CSharpUtilities)

add_library(${PROJECT_NAME} SHARED 
    src/Config.cs
    src/Context.cs
    src/Device.cs
    src/DeviceInfo.cs
    src/DeviceList.cs
    src/CameraParamList.cs
    src/DepthWorkModeList.cs
    src/Error.cs
    src/Filter.cs
    src/Frame.cs
    src/NativeHandle.cs
    src/NativeException.cs
    src/obNative.cs
    src/Pipeline.cs
    src/Sensor.cs
    src/SensorList.cs
    src/StreamProfile.cs
    src/StreamProfileList.cs
    src/Recorder.cs
    src/Playback.cs
    src/Types.cs
    src/Property.cs
    src/Version.cs
)

set_property(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION 4.7.2)


add_subdirectory(samples)

file(GLOB OB_SDK_LIB_FILES ${CMAKE_CURRENT_LIST_DIR}/OrbbecSDK/lib/*)
file(GLOB OB_SDK_CONFIG_FILES ${CMAKE_CURRENT_LIST_DIR}/OrbbecSDK/config/OrbbecSDKConfig.*)


install (FILES ${OB_SDK_LIB_FILES} DESTINATION ${CMAKE_INSTALL_PREFIX}/bin)
install (FILES ${OB_SDK_CONFIG_FILES} DESTINATION ${CMAKE_INSTALL_PREFIX}/bin)

install(TARGETS ${PROJECT_NAME}
    RUNTIME DESTINATION ${CMAKE_INSTALL_PREFIX}/bin
)
