using System;
using Newtonsoft.Json;

namespace JsonSerializer
{
    public class NewtonsoftSerializer : IJsonSerializer
    {
        private JsonSerializerSettings _settings;

        public NewtonsoftSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };   
        }

        public T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public string SerializeToString<T>(T jsonData)
        {
            return JsonConvert.SerializeObject(jsonData, _settings);
        }

        public byte[] SerializeToByteArray(object obj)
        {
            var stringObj = JsonConvert.SerializeObject(obj);

            byte[] bytes = new byte[stringObj.Length * sizeof(char)];
            Buffer.BlockCopy(stringObj.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
   
}
