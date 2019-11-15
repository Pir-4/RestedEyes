using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;

namespace RestedEyes.Configs
{
    public class ConfigManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static bool Exist(string path = null)
        {
            Logger.Info($"Check file is exist '{path}'");
            if (string.IsNullOrWhiteSpace(path))
                path = ConfigManager.PathDefault;
            return File.Exists(path);
        }

        public static IEnumerable<Config> Read(string path)
        {
            Logger.Info($"Read from file '{path}'");
            using (var stream = File.Open(path, FileMode.Open))
            {
                var _serializer = new DataContractJsonSerializer(typeof(IEnumerable<Config>));
                return (IEnumerable<Config>)_serializer.ReadObject(stream);
            }
        }

        public static void Write(string path, Config[] configs)
        {
            Logger.Info($"Write to file '{path}'");
            using (var stream = File.Create(path))
            {
                var _serializer = new DataContractJsonSerializer(typeof(IEnumerable<Config>));
                _serializer.WriteObject(stream, configs);
            }
        }

        public static IEnumerable<Config> ConfigsDefault()
        {
            Logger.Info($"Use default config values");
            return new List<Config> {
                new Config()
            {
                message = "Сделайте гимнастику для глаз",
                Rest = new TimeInfo() { Number=5, Sign="m"},
                Work = new TimeInfo() { Number=1, Sign="h"},
            },
            new Config() {
                message = "Разомнитесь",
                Rest = new TimeInfo() { Number=15, Sign="m"},
                Work = new TimeInfo() { Number=2, Sign="h"},
            },
             new Config() {
                message = "Передохните",
                Rest = new TimeInfo() { Number=2, Sign="m"},
                Work = new TimeInfo() { Number=45, Sign="m"},
             },              
            };
        }

        public static string PathDefault
        {  
            get { return Path.Combine(Directory.GetCurrentDirectory(), "ConfigTime.json"); }
        }
    }
}
