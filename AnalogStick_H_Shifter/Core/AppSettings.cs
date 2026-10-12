using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace AnalogStick_H_Shifter
{
    public enum InputMode { XInput, DirectInput }
    public enum StickSide { Right, Left }
    public enum AxisPair { XY, RxRy, ZRz }

    /// <summary>Everything the app remembers between sessions (stored as settings.json next to the exe).</summary>
    [DataContract]
    public sealed class AppSettings
    {
        [DataMember] public InputMode InputMode { get; set; }
        [DataMember] public StickSide XInputStick { get; set; }
        [DataMember] public string DirectInputDevice { get; set; }
        [DataMember] public AxisPair DirectInputAxes { get; set; }
        [DataMember] public bool InvertY { get; set; }
        [DataMember] public string LayoutName { get; set; }
        [DataMember] public ShifterLayout Layout { get; set; }
        [DataMember] public Dictionary<string, int> ShiftCounts { get; set; }
        [DataMember] public int WindowWidth { get; set; }
        [DataMember] public int WindowHeight { get; set; }
        [DataMember] public bool WindowMaximized { get; set; }

        public int TotalShifts => ShiftCounts.Values.Sum();

        public int GetCount(string gear) => ShiftCounts.TryGetValue(gear, out int c) ? c : 0;

        public void Increment(string gear) => ShiftCounts[gear] = GetCount(gear) + 1;

        public static AppSettings Load(string path, string legacyCounterPath)
        {
            AppSettings settings = null;
            try
            {
                if (File.Exists(path)) settings = Json.Read<AppSettings>(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not read settings: " + ex.Message);
            }

            settings = settings ?? new AppSettings();
            if (settings.ShiftCounts == null)
            {
                settings.ShiftCounts = new Dictionary<string, int>();
                ImportLegacyCounters(settings, legacyCounterPath);
            }
            return settings;
        }

        public void Save(string path) => Json.Write(path, this);

        /// <summary>
        /// Version 1.x stored the counters as a BinaryFormatter-serialized int[7] (gears 1-6, R).
        /// The record is parsed by hand so no BinaryFormatter is needed.
        /// </summary>
        static void ImportLegacyCounters(AppSettings settings, string path)
        {
            try
            {
                if (!File.Exists(path)) return;

                byte[] data = File.ReadAllBytes(path);
                // Header (17 bytes), then ArraySinglePrimitive record: 0x0F, id, length, type 0x08 (Int32), values.
                const int recordStart = 17;
                if (data.Length < recordStart + 10 || data[recordStart] != 0x0F || data[recordStart + 9] != 0x08) return;

                int length = BitConverter.ToInt32(data, recordStart + 5);
                int valuesStart = recordStart + 10;
                if (length <= 0 || length > 64 || data.Length < valuesStart + length * 4) return;

                string[] names = { "1", "2", "3", "4", "5", "6", "R" };
                for (int i = 0; i < Math.Min(length, names.Length); i++)
                {
                    int value = BitConverter.ToInt32(data, valuesStart + i * 4);
                    if (value > 0) settings.ShiftCounts[names[i]] = value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not import legacy shift counters: " + ex.Message);
            }
        }
    }
}
