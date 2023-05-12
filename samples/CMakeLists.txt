project(Samples LANGUAGES CSharp)

include(CSharpUtilities)

add_executable(HelloOrbbec HelloOrbbec.cs)
add_dependencies(HelloOrbbec ob_csharp)

add_executable(PipelineSample PipelineSample.cs)
add_dependencies(PipelineSample ob_csharp)

add_executable(PipelineSample_Wait PipelineSample_Wait.cs)
add_dependencies(PipelineSample_Wait ob_csharp)

