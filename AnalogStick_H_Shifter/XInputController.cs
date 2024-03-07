using SharpDX.XInput;

namespace AnalogStick_H_Shifter
{
    class XInputController
    {
        Controller controller;
        public bool connected = false;
        public int deadband = 2500;

        //public string recognizedGear = "";
        public Gamepad gamepad { get; set; }

        public XInputController()
        {
            controller = new Controller(UserIndex.One);

            connected = controller.IsConnected;
        }

        // Call this method to update all class values
        public void Update()
        {
            if (!connected)
                return;

            gamepad = controller.GetState().Gamepad;
            //Console.WriteLine("X: " + gamepad.RightThumbX + "\t Y: " + gamepad.RightThumbY + " " + recognizedGear);
        }
    }
}
