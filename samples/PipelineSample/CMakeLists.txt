project(Pipeline_cs LANGUAGES CSharp)

include(CSharpUtilities)

add_executable(${PROJECT_NAME} PipelineSample.cs)

add_dependencies(${PROJECT_NAME} ob_csharp)
