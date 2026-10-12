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
- Controller access requires appropriate drivers and permissions (SharpDX handles XInput/DirectInput). If no controller is found, switch the controller type or reconnect the device.

Building
--------
The project is a classic .NET Framework 4.7.2 WinForms application. It needs MSBuild from Visual Studio or the Visual Studio Build Tools; `dotnet build` fails on the resource generation of classic .NET Framework projects.

The easiest way is the build script. It downloads `nuget.exe` if needed, restores the packages and locates MSBuild via `vswhere`:

```powershell
.\build.ps1 -Configuration Release
```

Manual build:

```powershell
.\nuget.exe restore .\AnalogStick_H_Shifter.sln
& 'C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe' .\AnalogStick_H_Shifter.sln /m /p:Configuration=Release
```

The output is written to `AnalogStick_H_Shifter\bin\Release`.

Releases
--------
1. Update the version with `.\update-version.ps1 -Version 2.0.0.0` and commit.
2. Push a tag starting with `v` (for example `v2.0.0.0`).
3. The GitHub Actions workflow `.github/workflows/release.yml` builds the project on Windows and publishes a GitHub release with a ZIP of the program files.
