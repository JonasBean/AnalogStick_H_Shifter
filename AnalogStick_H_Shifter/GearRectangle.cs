using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalogStick_H_Shifter
{
    class GearRectangle
    {
        private Rectangle rect;
        private string gear;

        public int Width { get => rect.Width; set => rect.Width = value; }
        public int Height { get => rect.Height; set => rect.Height = value; }
        public int XPosition { get => rect.X; set => rect.X = value; }
        public int YPosition { get => rect.Y; set => rect.Y = value; }
        public string Gear { get => gear; set => gear = value; }
        public Rectangle Rect { get => rect; set => rect = value; }
    }
}
