# Build Scripts
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent $scriptDir
$orbbecSdkPath = Join-Path -Path $projectRoot -ChildPath "OrbbecSDK"
$libPath = Join-Path -Path $orbbecSdkPath -ChildPath "lib"
$buildPath = Join-Path -Path $projectRoot -ChildPath "build"
$installPath = Join-Path -Path $buildPath -ChildPath "install/CSharpWrapper"

if (!(Test-Path $orbbecSdkPath)) {
    Write-Host "OrbbecSDK not found!" -ForegroundColor Red
    exit 1
}

if (!(Test-Path $libPath)) {
    Write-Host "lib not found!" -ForegroundColor Red
    exit 1
}

if (!(Test-Path $buildPath)) {
    Write-Host "build not found, create it!" -ForegroundColor Yellow
    New-Item -Path $buildPath -ItemType Directory | Out-Null
}

if (!(Test-Path $installPath)) {
    Write-Host "install not found, create it!" -ForegroundColor Yellow
    New-Item -Path $installPath -ItemType Directory | Out-Null
}

Write-Host "Please enter the Visual Studio version to use (e.g., '2019' or '2022'). Default is '2019':" -ForegroundColor Cyan
$userInput = Read-Host "Visual Studio Version"

if ([string]::IsNullOrWhiteSpace($userInput)) {
    $userInput = "2019"
}

switch ($userInput) {
    "2019" { $cmakeGenerator = "Visual Studio 16 2019" }
    "2022" { $cmakeGenerator = "Visual Studio 17 2022" }
    default {
        Write-Host "Invalid input! Please enter '2019' or '2022'." -ForegroundColor Red
        exit 1
    }
}
Write-Host "Using CMake Generator: $cmakeGenerator" -ForegroundColor Green

Write-Host "Running cmake to generate the solution..." -ForegroundColor Yellow
Push-Location $buildPath
try {
    cmake .. -G "$cmakeGenerator" -DCMAKE_BUILD_TYPE=Release -DCMAKE_INSTALL_PREFIX="$installPath"
    cmake --build . --config Release --target install
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: cmake failed to generate the solution!" -ForegroundColor Red
        throw "CMake generation failed."
    }
} catch {
    Write-Host "An error occurred during CMake generation." -ForegroundColor Red
    Write-Host "Deleting build directory to clean up..." -ForegroundColor Yellow
    Remove-Item -Path $buildPath -Recurse -Force
    exit 1
} finally {
    Pop-Location
}

Write-Host "Compilation completed!" -ForegroundColor Cyan