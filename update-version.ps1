param(
    [Parameter(Mandatory=$true)]
    [string]$Version
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Validate version format (should be like 1.0.0.0)
if ($Version -notmatch '^\d+\.\d+\.\d+\.\d+$') {
    Write-Error "Version must be in format 'Major.Minor.Build.Revision' (e.g. 1.0.0.0)"
    exit 1
}

# Update AssemblyInfo.cs
$assemblyInfoPath = Join-Path $repoRoot "AnalogStick_H_Shifter\Properties\AssemblyInfo.cs"
if (-not (Test-Path $assemblyInfoPath)) {
    Write-Error "AssemblyInfo.cs not found at: $assemblyInfoPath"
    exit 2
}

Write-Host "Updating version to $Version in AssemblyInfo.cs..."

$content = Get-Content $assemblyInfoPath -Raw
$content = $content -replace '\[assembly: AssemblyVersion\("[\d\.]+"\)\]', "[assembly: AssemblyVersion(`"$Version`")]"
$content = $content -replace '\[assembly: AssemblyFileVersion\("[\d\.]+"\)\]', "[assembly: AssemblyFileVersion(`"$Version`")]"
Set-Content $assemblyInfoPath $content

Write-Host "Version updated successfully to $Version"

# Build the project in Release configuration
Write-Host "Building project..."
.\build.ps1 -Configuration Release

Write-Host "Version update and build completed successfully!"