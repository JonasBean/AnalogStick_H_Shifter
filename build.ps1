<#
build.ps1 - Simple build script for AnalogStick_H_Shifter

Usage examples:
  .\build.ps1                    # Debug build using MSBuild if available
  .\build.ps1 -Configuration Release
  .\build.ps1 -MsBuildPath 'C:\Path\To\MSBuild.exe'

What it does:
 - Ensures nuget.exe is available (downloads to repo root if missing)
 - Runs nuget restore on the solution
 - Attempts to find MSBuild.exe (Visual Studio Build Tools) and use it to build the solution
 - Falls back to `dotnet build` if MSBuild is not found
#>

param(
    [string]$Configuration = "Debug",
    [string]$MsBuildPath = $null,
    [switch]$NoNuGetDownload
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $repoRoot

Write-Host "Repository root: $repoRoot"
Write-Host "Configuration: $Configuration"

$solution = Join-Path $repoRoot 'AnalogStick_H_Shifter.sln'
if (-not (Test-Path $solution)) {
    Write-Error "Solution file not found: $solution"
    exit 2
}

# Ensure nuget.exe
$nugetExe = Join-Path $repoRoot 'nuget.exe'
if (-not (Get-Command nuget -ErrorAction SilentlyContinue) -and -not (Test-Path $nugetExe)) {
    if ($NoNuGetDownload) {
        Write-Error "nuget.exe not found in PATH and downloading disabled. Provide nuget.exe or install NuGet CLI."
        exit 3
    }
    Write-Host "nuget.exe not found; downloading latest nuget.exe to $nugetExe"
    try {
        Invoke-WebRequest -Uri 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile $nugetExe -UseBasicParsing
    } catch {
        Write-Error "Failed to download nuget.exe: $_"
        exit 4
    }
}

# Run restore
if (Get-Command nuget -ErrorAction SilentlyContinue) {
    Write-Host "Running: nuget restore $solution"
    nuget restore $solution
} else {
    Write-Host "Running: $nugetExe restore $solution"
    & $nugetExe restore $solution
}

# Find MSBuild.exe if not provided
if ($MsBuildPath) {
    $msbuild = $MsBuildPath
} else {
    $candidates = @(
        'C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe'
    )
    $msbuild = $candidates | Where-Object { Test-Path $_ } | Select-Object -First 1

    # If not found, try using vswhere (if available) to locate installed Visual Studio and MSBuild
    if (-not $msbuild) {
        $vswhere = "$env:ProgramFiles(x86)\Microsoft Visual Studio\Installer\vswhere.exe"
        if (Test-Path $vswhere) {
            try {
                $vswhereArgs = @('-latest','-products','*','-requires','Microsoft.Component.MSBuild','-property','installationPath')
                $installPath = & $vswhere $vswhereArgs 2>$null | Select-Object -First 1
                if ($installPath) {
                    $candidate = Join-Path $installPath 'MSBuild\Current\Bin\MSBuild.exe'
                    if (Test-Path $candidate) { $msbuild = $candidate }
                }
            } catch {
                # ignore vswhere failures
            }
        }
    }
}

if ($msbuild) {
    Write-Host "Using MSBuild: $msbuild"
    $msbuildArgs = @(
        $solution,
        '/m',
        "/p:Configuration=$Configuration"
    )
    $p = Start-Process -FilePath $msbuild -ArgumentList $msbuildArgs -NoNewWindow -Wait -PassThru
    if ($p.ExitCode -ne 0) {
        Write-Error "MSBuild failed with exit code $($p.ExitCode)"
        exit $p.ExitCode
    }
    Write-Host "MSBuild completed successfully."
    exit 0
} else {
    Write-Warning "MSBuild.exe not found. Falling back to 'dotnet build' (may fail for classic .NET Framework projects)."
    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        Write-Error "dotnet CLI not found. Install .NET SDK or provide MSBuild path."
        exit 5
    }
    dotnet restore
    dotnet build $solution -c $Configuration
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    Write-Host "dotnet build completed."
    exit 0
}
