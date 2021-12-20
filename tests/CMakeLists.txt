project(UnitTest_cs LANGUAGES CSharp)

include(CSharpUtilities)

add_library(${PROJECT_NAME} SHARED 
                            Context_Test.cs
                            Device_Test.cs
                            Sensor_Test.cs
                            Pipeline_Test.cs
                            )

add_dependencies(${PROJECT_NAME} ob_csharp)

set(NUnitFramework "${CMAKE_CURRENT_SOURCE_DIR}/NUnit/nunit.framework.dll")
# set(NUnitFrameworkTests "${CMAKE_CURRENT_SOURCE_DIR}/NUnit/nunit.framework.tests.dll")

set_property(TARGET ${PROJECT_NAME} PROPERTY 
    VS_DOTNET_REFERENCE_nunit.framework ${NUnitFramework}
)

# set_property(TARGET ${PROJECT_NAME} PROPERTY 
#     VS_DOTNET_REFERENCE_nunit.framework.tests ${NUnitFrameworkTests}
# )