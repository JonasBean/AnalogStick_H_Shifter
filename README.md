AnalogStick_H_Shifter
=====================

What this program does
------------------------------------------
AnalogStick_H_Shifter is a small Windows Forms utility that lets you use a gamepad's analog stick as an "H‑shifter" for applications or games that accept keyboard gear inputs. The program reads the right analog stick (X/Y) from either an XInput-compatible controller (Xbox style) or a DirectInput joystick/gamepad, displays a visual axis, and maps stick positions into configurable rectangular "gear" regions. When the stick enters a region the app simulates keyboard scancodes (keys 1–6 for gears and 9 for reverse) using the Windows SendInput API so the target program receives the events as normal key presses.

Main features
-------------
- Support for XInput and DirectInput controllers (SharpDX used for device access).
- Visual editor: draw and move rectangular gear zones on the axis image and save/load layouts to the `Layouts/` folder.
- Live overlay showing the current stick position and the active gear (visual colours and large gear indicator).
- Persisted shift counters (written to a small `shiftCount` file) and per‑gear counters shown in the UI.
- Optionally compute and publish releases using the provided `build.ps1` and `deploy-release.ps1` helper scripts (local packaging and GitHub release support).

How to use (quickstart)
------------------------
1. Build the project with Visual Studio or MSBuild (the project targets .NET Framework 4.7.2). See the build instructions above.
2. Run `AnalogStick_H_Shifter.exe` from the `bin\Release` (or `bin\Debug`) folder.
3. Choose controller type in the UI (XInput or DirectInput). If using DirectInput, select the device from the dropdown.
4. Press "Activate" to start reading the stick. The right stick position will be shown on the white axis image.
5. The default layout contains seven regions (gears 1–6 and R). You can move/resize these rectangles with the mouse or create and save new layouts to `Layouts/`.
6. When the stick moves into a region the app sends the corresponding keyboard scancode (1..6 or 9). Use this to control the target application as if you pressed those keys on your keyboard.

Safety and notes
----------------
- The app uses low‑level SendInput to generate keyboard events — these are global to the Windows session and will affect whichever application has input focus. Close or minimize target programs or ensure focus is correct before activating.
- Classic .NET projects like this one require MSBuild/Visual Studio Build Tools for reliable resource generation; `dotnet build` may fail for some resource tasks.
- Controller access requires appropriate drivers and permissions (SharpDX handles XInput/DirectInput). If no controller is found, switch the controller type or reconnect the device.

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

