project(HelloOrbbec_cs LANGUAGES CSharp)

include(CSharpUtilities)

add_executable(${PROJECT_NAME} HelloOrbbec.cs)

add_dependencies(${PROJECT_NAME} ob_csharp)
