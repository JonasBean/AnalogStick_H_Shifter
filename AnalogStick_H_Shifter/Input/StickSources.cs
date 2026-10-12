using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.XInput;

namespace AnalogStick_H_Shifter
{
    /// <summary>Provides the stick position normalized to 0..1 (x: left→right, y: up→down).</summary>
    interface IStickSource : IDisposable
    {
        bool TryRead(out PointF position);
    }

    /// <summary>Xbox-style controllers. Uses the first connected controller and picks up hot-plugged ones.</summary>
    sealed class XInputStickSource : IStickSource
    {
        readonly Controller[] controllers =
        {
            new Controller(UserIndex.One), new Controller(UserIndex.Two),
            new Controller(UserIndex.Three), new Controller(UserIndex.Four),
        };
        readonly StickSide side;
        int activeIndex = -1;
        DateTime nextScan = DateTime.MinValue;

        public XInputStickSource(StickSide side) => this.side = side;

        public bool TryRead(out PointF position)
        {
            position = new PointF(0.5f, 0.5f);

            if (activeIndex >= 0 && !Read(controllers[activeIndex], ref position))
                activeIndex = -1;

            if (activeIndex < 0)
            {
                // Querying unplugged slots is slow, so only rescan every now and then.
                if (DateTime.UtcNow < nextScan) return false;
                nextScan = DateTime.UtcNow.AddSeconds(1);

                for (int i = 0; i < controllers.Length && activeIndex < 0; i++)
                    if (Read(controllers[i], ref position)) activeIndex = i;
            }

            return activeIndex >= 0;
        }

        bool Read(Controller controller, ref PointF position)
        {
            if (!controller.GetState(out State state)) return false;

            var pad = state.Gamepad;
            short x = side == StickSide.Right ? pad.RightThumbX : pad.LeftThumbX;
            short y = side == StickSide.Right ? pad.RightThumbY : pad.LeftThumbY;

            position = new PointF((x + 32768f) / 65535f, 1f - (y + 32768f) / 65535f);
            return true;
        }

        public void Dispose() { }
    }

    /// <summary>DirectInput joysticks, wheels and gamepads.</summary>
    sealed class DirectInputStickSource : IStickSource
    {
        static readonly DirectInput directInput = new DirectInput();

        readonly Guid deviceGuid;
        readonly AxisPair axes;
        readonly bool invertY;
        Joystick joystick;
        DateTime nextRetry = DateTime.MinValue;

        public DirectInputStickSource(Guid deviceGuid, AxisPair axes, bool invertY)
        {
            this.deviceGuid = deviceGuid;
            this.axes = axes;
            this.invertY = invertY;
        }

        public static IList<DeviceInstance> GetDevices()
        {
            try
            {
                return directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly)
                    .GroupBy(d => d.InstanceGuid).Select(g => g.First()).ToList();
            }
            catch (SharpDXException)
            {
                return new List<DeviceInstance>();
            }
        }

        public bool TryRead(out PointF position)
        {
            position = new PointF(0.5f, 0.5f);

            if (joystick == null && !TryOpen()) return false;

            try
            {
                joystick.Poll();
                var state = joystick.GetCurrentState();

                int x, y;
                switch (axes)
                {
                    case AxisPair.RxRy: x = state.RotationX; y = state.RotationY; break;
                    case AxisPair.ZRz: x = state.Z; y = state.RotationZ; break;
                    default: x = state.X; y = state.Y; break;
                }

                float ny = y / 65535f;
                position = new PointF(x / 65535f, invertY ? 1f - ny : ny);
                return true;
            }
            catch (SharpDXException)
            {
                // Device unplugged or focus lost – reopen on the next attempt.
                Close();
                return false;
            }
        }

        bool TryOpen()
        {
            if (deviceGuid == Guid.Empty || DateTime.UtcNow < nextRetry) return false;
            nextRetry = DateTime.UtcNow.AddSeconds(1);

            try
            {
                joystick = new Joystick(directInput, deviceGuid);
                try { joystick.Properties.Range = new InputRange(0, 65535); } catch (SharpDXException) { }
                joystick.Acquire();
                return true;
            }
            catch (SharpDXException)
            {
                Close();
                return false;
            }
        }

        void Close()
        {
            try { joystick?.Unacquire(); } catch (SharpDXException) { }
            joystick?.Dispose();
            joystick = null;
        }

        public void Dispose() => Close();
    }
}
