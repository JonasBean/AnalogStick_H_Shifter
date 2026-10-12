AnalogStick_H_Shifter
=====================

What this program does
------------------------------------------
AnalogStick_H_Shifter is a small Windows utility that lets you use a gamepad's analog stick as an "H‑shifter" for games that accept keyboard gear inputs. It reads a stick from an XInput controller (Xbox style) or a DirectInput device (joysticks, wheels, gamepads), shows the live stick position and maps it into configurable rectangular "gear zones". When the stick enters a zone the app holds that gear's key using the Windows SendInput API (hardware scan codes), so the game receives normal key presses.

Main features
-------------
- Modern dark UI with a resizable window and a live, scalable shift-pattern editor.
- **1 to 12 forward gears + reverse** – change the gear count with the **− / +** buttons. Zones are arranged as a classic H-pattern (odd gears on top, even gears below; reverse bottom right for odd counts, top left for even counts).
- Drag zones to move them, drag any corner to resize them, or nudge the selected zone with the arrow keys (Ctrl + arrows resizes, Shift = bigger steps).
- **Free key binding per gear**: click a gear's key in the "Gears & keys" list and press the new key. Duplicate bindings are highlighted.
- XInput: choose left or right stick; the first connected controller is picked up automatically (also when plugged in later).
- DirectInput: choose device, axis pair (X/Y, Rx/Ry, Z/Rz) and optionally invert the Y axis.
- Live gear indicator, stick trail, per-gear and total shift counters.
- Save/load named layouts (`Layouts/*.json`). Layout files and shift counters from version 1.x are imported automatically.
- All settings, the current layout and the window size are remembered in `settings.json` next to the exe.

How to use (quickstart)
------------------------
1. Build the project with Visual Studio or MSBuild (.NET Framework 4.7.2, see below) or download a release.
2. Run `AnalogStick_H_Shifter.exe`.
3. Choose the input (XInput or DirectInput) in the "Controller" card. The status badge at the top shows whether a controller is connected; the stick position appears on the pattern.
4. Pick the number of forward gears with − / +, adjust the zones with the mouse and rebind keys if needed (defaults: gears 1–8 → keys 1–8, gear 9 → 0, reverse → 9).
5. Press **Activate** to start sending keys. Press it again to pause the output.
6. Save the layout under a name with "Save as…" to reuse it later.

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

