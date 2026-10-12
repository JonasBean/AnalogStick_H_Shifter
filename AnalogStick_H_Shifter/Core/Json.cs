using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace AnalogStick_H_Shifter
{
    /// <summary>Small JSON helper on top of DataContractJsonSerializer (no external dependency).</summary>
    static class Json
    {
        public static T Read<T>(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
            }
        }

        public static void Write<T>(string path, T value)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var buffer = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(buffer, Encoding.UTF8, false, true, "  "))
                {
                    new DataContractJsonSerializer(typeof(T)).WriteObject(writer, value);
                }

                // Write to a temp file first so a crash never leaves a half-written file behind.
                string tmp = path + ".tmp";
                File.WriteAllBytes(tmp, buffer.ToArray());
                if (File.Exists(path)) File.Delete(path);
                File.Move(tmp, path);
            }
        }
    }
}
