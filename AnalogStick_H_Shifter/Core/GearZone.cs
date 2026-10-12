using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>
    /// A rectangular region of the stick range that engages one gear.
    /// Coordinates are normalized (0..1) so the layout is independent of the canvas size:
    /// x = 0 is full left, y = 0 is full up.
    /// </summary>
    [DataContract]
    public sealed class GearZone
    {
        [DataMember(Order = 0)] public string Name { get; set; }
        [DataMember(Order = 1)] public bool IsReverse { get; set; }
        [DataMember(Order = 2)] public Keys Key { get; set; }
        [DataMember(Order = 3)] public float X { get; set; }
        [DataMember(Order = 4)] public float Y { get; set; }
        [DataMember(Order = 5)] public float Width { get; set; }
        [DataMember(Order = 6)] public float Height { get; set; }

        public RectangleF Bounds
        {
            get => new RectangleF(X, Y, Width, Height);
            set { X = value.X; Y = value.Y; Width = value.Width; Height = value.Height; }
        }

        /// <summary>1-based forward gear number, 0 for reverse.</summary>
        public int Number => IsReverse ? 0 : (int.TryParse(Name, out int n) ? n : 0);

        public bool Contains(PointF p) => p.X >= X && p.X <= X + Width && p.Y >= Y && p.Y <= Y + Height;

        public GearZone Clone() => (GearZone)MemberwiseClone();

        public static Keys DefaultKeyFor(int forwardGear)
        {
            if (forwardGear >= 1 && forwardGear <= 8) return Keys.D0 + forwardGear;
            switch (forwardGear)
            {
                case 9: return Keys.D0;
                case 10: return Keys.OemMinus;
                case 11: return Keys.Oemplus;
                default: return Keys.NumPad0 + (forwardGear - 12) % 10;
            }
        }

        public static Keys DefaultReverseKey => Keys.D9;
    }
}
