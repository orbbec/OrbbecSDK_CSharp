project(HelloOrbbec LANGUAGES CSharp)

include(CSharpUtilities)

add_executable(${PROJECT_NAME} Program.cs)

add_dependencies(${PROJECT_NAME} ob_csharp)

set_property(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION v4.7.2)

set_property(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_REFERENCES
    "System"
)

set_target_properties (${PROJECT_NAME} PROPERTIES
	FOLDER samples
)

install(TARGETS HelloOrbbec RUNTIME DESTINATION ${CMAKE_INSTALL_PREFIX}/bin)