using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    class DirectInputController
    {
        DirectInput directInput = new DirectInput();
        public bool connected = false;
        Guid joystickGuid;

        //public string recognizedGear = "";

        public Joystick joystick { get; set; }
        public Point joystickPosition { get; set; }
        public Point previousJoystickPosition { get; set; }

        public DirectInputController(ComboBox joyStickListBox, bool selectBoxChanged)
        {
            // Find all joysticks connected to the system
            IList<DeviceInstance> connectedJoysticks = new List<DeviceInstance>();
            IList<string> connectedJoysticksString = new List<string>();

            // - look for joysticks
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            {
                connectedJoysticks.Add(deviceInstance);
                connectedJoysticksString.Add(deviceInstance.InstanceName);
            }

            if (!selectBoxChanged)
            {
                joyStickListBox.DataSource = connectedJoysticksString;
            }

            //Console.WriteLine(joyStickListBox.SelectedIndex);

            if (connectedJoysticks.Count > 0)
                joystickGuid = connectedJoysticks[joyStickListBox.SelectedIndex].InstanceGuid;

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No directinput joystick/Gamepad found.");
            }
            else
            {
                connected = true;
                // Instantiate the joystick
                joystick = new Joystick(directInput, joystickGuid);
                Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

                // Set BufferSize in order to use buffered data.
                joystick.Properties.BufferSize = 32;

                // Acquire the joystick
                joystick.Acquire();
            }
        }

        public void Update()
        {
            if (!connected)
                return;

            int x = -5000, y = -5000;

            joystick.Poll();
            var datas = joystick.GetBufferedData();
            foreach (var state in datas)
            {
                if (state.Offset == JoystickOffset.X)
                {
                    x = state.Value;
                }

                if (state.Offset == JoystickOffset.Y)
                {
                    y = state.Value;
                }

                if (x != -5000 && y != -5000)
                {
                    joystickPosition = new Point(x, y);
                }
            }
        }
    }
}
