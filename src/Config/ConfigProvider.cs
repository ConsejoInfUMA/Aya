using System.IO;
using Newtonsoft.Json;

namespace Aya.Config
{
    public class ConfigProvider : IConfigProvider
    {
        private string _file = "config.json";

        public BotConfig ReadConfig()
        {
            if (!File.Exists(_file))
            {
                var json = JsonConvert.SerializeObject(new BotConfig(), Formatting.Indented);
                File.WriteAllText(_file, json);
                return null;
            }
            else
            {
                var json = System.IO.File.ReadAllText(_file);
                return JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }
    }
}

