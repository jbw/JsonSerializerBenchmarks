using Jil;
using System.IO;

namespace JsonSerializer
{
    public class JilSerializer : IJsonSerializer
    {
        private readonly Options _jilOptions;

        public JilSerializer()
        {
            _jilOptions = new Options(false, false, false, DateTimeFormat.ISO8601, true);
        }

        public T Deserialize<T>(string jsonString)
        {
            return JSON.Deserialize<T>(jsonString);
        }

        public string SerializeToString<T>(T jsonData)
        {
            using (var output = new StringWriter())
            {
                JSON.Serialize(jsonData, output, _jilOptions);

                return output.ToString();
            }
        }

        public byte[] SerializeToByteArray(object obj)
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {
                    JSON.Serialize(obj, sw, _jilOptions);
                }
                return ms.ToArray();
            }
        }
    }
}
