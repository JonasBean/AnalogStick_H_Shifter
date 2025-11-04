AnalogStick_H_Shifter
=====================

Summary
-------
This project was migrated from packages.config to PackageReference so NuGet packages (SharpDX, InputSimulator) are resolved using the modern NuGet/MSBuild workflow.

Build environment notes
-----------------------
- The project is a classic .NET Framework application (TargetFramework v4.7.2).
- Recommended build tools: Visual Studio or Visual Studio Build Tools (MSBuild). `dotnet build` can fail on classical projects with errors like MSB4216 (GenerateResource host).

Quick build instructions (PowerShell)
-----------------------------------
1) Download nuget.exe (if not available globally):
```powershell
cd C:\Repos\AnalogStick_H_Shifter
Invoke-WebRequest -Uri 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile 'nuget.exe'
```

2) Restore packages:
```powershell
.\nuget.exe restore .\AnalogStick_H_Shifter.sln
```

3) Build using Visual Studio Build Tools (recommended):
```powershell
& 'C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe' .\AnalogStick_H_Shifter.sln /m
```

Alternative (may not always work):
```powershell
dotnet restore
dotnet build
```
Note: `dotnet build` can still produce GenerateResource (MSB4216) errors for this project; use MSBuild if that occurs.

CI note (GitHub Actions) — minimal
---------------------------------
- Use a Windows runner with MSBuild/Visual Studio available. A simple workflow can be configured to restore NuGet and build with MSBuild.

What changed
------------
- `AnalogStick_H_Shifter.csproj` now contains PackageReference entries for SharpDX (Core, DirectInput, XInput) and InputSimulator.
- `packages.config` has been removed.
- Legacy HintPath-style references were removed.
- `<RuntimeIdentifiers>win</RuntimeIdentifiers>` was added so modern NuGet/MSBuild workflows can resolve runtime-specific assets.

If you want
----------
- I can add a GitHub Actions workflow file (`.github/workflows/build.yml`) to the repo.
- Or I can prepare a CI configuration for Azure DevOps/TeamCity/Jenkins — tell me which system you use.
