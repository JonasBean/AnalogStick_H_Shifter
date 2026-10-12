using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>A complete shift pattern: all forward gears plus reverse.</summary>
    [DataContract]
    public sealed class ShifterLayout
    {
        public const int MinForwardGears = 1;
        public const int MaxForwardGears = 12;
        public const string FileExtension = ".json";

        /// <summary>Stick range used by the 1.x binary layout files.</summary>
        const float LegacyAxisSize = 680f;

        [DataMember(Order = 0)] public int Version { get; set; } = 2;
        [DataMember(Order = 1)] public List<GearZone> Gears { get; set; } = new List<GearZone>();

        public int ForwardCount => Gears.Count(g => !g.IsReverse);
        public GearZone Reverse => Gears.FirstOrDefault(g => g.IsReverse);

        public ShifterLayout Clone() => new ShifterLayout { Gears = Gears.Select(g => g.Clone()).ToList() };

        /// <summary>
        /// Builds a classic H-pattern: odd gears on top, even gears below.
        /// With an odd gear count reverse takes the free slot bottom right (like a 5-speed),
        /// with an even count it gets its own column top left (like a 6-speed).
        /// </summary>
        public static ShifterLayout CreateDefault(int forwardGears, IDictionary<string, Keys> keepKeys = null)
        {
            forwardGears = Math.Max(MinForwardGears, Math.Min(MaxForwardGears, forwardGears));

            bool reverseOwnColumn = forwardGears % 2 == 0;
            int columns = (forwardGears + 1) / 2 + (reverseOwnColumn ? 1 : 0);
            int firstForwardColumn = reverseOwnColumn ? 1 : 0;

            const float margin = 0.04f, gap = 0.035f;
            const float topY = 0.04f, bottomY = 0.62f, rowHeight = 0.34f;
            float pitch = (1f - 2 * margin) / columns;

            RectangleF Slot(int column, int row) => new RectangleF(
                margin + column * pitch + gap / 2, row == 0 ? topY : bottomY, pitch - gap, rowHeight);

            Keys KeyFor(string name, Keys fallback) =>
                keepKeys != null && keepKeys.TryGetValue(name, out var k) ? k : fallback;

            var layout = new ShifterLayout();
            for (int gear = 1; gear <= forwardGears; gear++)
            {
                string name = gear.ToString();
                layout.Gears.Add(new GearZone
                {
                    Name = name,
                    Key = KeyFor(name, GearZone.DefaultKeyFor(gear)),
                    Bounds = Slot(firstForwardColumn + (gear - 1) / 2, (gear - 1) % 2),
                });
            }

            layout.Gears.Add(new GearZone
            {
                Name = "R",
                IsReverse = true,
                Key = KeyFor("R", GearZone.DefaultReverseKey),
                Bounds = reverseOwnColumn ? Slot(0, 0) : Slot(columns - 1, 1),
            });

            return layout;
        }

        /// <summary>Re-arranges into the default pattern for <paramref name="forwardGears"/>, keeping key bindings.</summary>
        public ShifterLayout WithForwardGears(int forwardGears)
        {
            var keys = Gears.GroupBy(g => g.Name).ToDictionary(g => g.Key, g => g.First().Key);
            return CreateDefault(forwardGears, keys);
        }

        /// <summary>Repairs out-of-range values and restores the gear order (forward gears ascending, reverse last).</summary>
        public void Normalize()
        {
            if (Gears == null) Gears = new List<GearZone>();
            Gears.RemoveAll(g => g == null);

            foreach (var g in Gears)
            {
                g.Width = Clamp(g.Width, 0.02f, 1f);
                g.Height = Clamp(g.Height, 0.02f, 1f);
                g.X = Clamp(g.X, 0f, 1f - g.Width);
                g.Y = Clamp(g.Y, 0f, 1f - g.Height);
                if (string.IsNullOrEmpty(g.Name)) g.Name = g.IsReverse ? "R" : "?";
            }

            // Forward gears first (in order), reverse last – the UI relies on this order.
            Gears = Gears.OrderBy(g => g.IsReverse).ThenBy(g => g.Number).ToList();
        }

        static float Clamp(float v, float min, float max) => v < min ? min : (v > max ? max : v);

        // ------------------------------------------------------------------ persistence

        public void Save(string path) => Json.Write(path, this);

        public static ShifterLayout Load(string path)
        {
            var layout = Path.GetExtension(path).Equals(FileExtension, StringComparison.OrdinalIgnoreCase)
                ? Json.Read<ShifterLayout>(path)
                : LoadLegacy(path);

            layout.Normalize();
            if (layout.Gears.Count == 0)
                throw new InvalidDataException("The layout does not contain any gears.");
            return layout;
        }

        /// <summary>Reads the binary layout format of version 1.x (name + 4×int32 per gear, 680 px stick range).</summary>
        static ShifterLayout LoadLegacy(string path)
        {
            var layout = new ShifterLayout();

            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    string name = reader.ReadString();
                    int x = reader.ReadInt32(), y = reader.ReadInt32(), w = reader.ReadInt32(), h = reader.ReadInt32();

                    bool reverse = name == "R";
                    int.TryParse(name, out int number);

                    layout.Gears.Add(new GearZone
                    {
                        Name = name,
                        IsReverse = reverse,
                        Key = reverse ? GearZone.DefaultReverseKey : GearZone.DefaultKeyFor(number),
                        Bounds = new RectangleF(x / LegacyAxisSize, y / LegacyAxisSize, w / LegacyAxisSize, h / LegacyAxisSize),
                    });
                }
            }

            return layout;
        }

        /// <summary>Lists saved layouts by display name. JSON files win over legacy files with the same name.</summary>
        public static SortedDictionary<string, string> ListSaved(string folder)
        {
            var result = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (!Directory.Exists(folder)) return result;

            foreach (var file in Directory.GetFiles(folder).OrderBy(f => Path.GetExtension(f) == FileExtension))
            {
                string ext = Path.GetExtension(file);
                if (ext == ".tmp") continue;
                string name = ext.Equals(FileExtension, StringComparison.OrdinalIgnoreCase)
                    ? Path.GetFileNameWithoutExtension(file)
                    : Path.GetFileName(file);
                result[name] = file;
            }

            return result;
        }
    }
}
