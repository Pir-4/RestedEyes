using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace RestedEyes.Configs
{
    public class ConfigManager
    {
        readonly DataContractJsonSerializer _serializer;

        public ConfigManager()
        {
            _serializer = new DataContractJsonSerializer(typeof(IEnumerable<Config>));
        }

        public bool Exist(string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = ConfigManager.PathConfigDefault();
            return File.Exists(path);
        }

        public IEnumerable<Config> Raad(string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                return (IEnumerable<Config>)_serializer.ReadObject(stream);
            }
        }

        public void Write(string path, Config[] configs)
        {
            using (var stream = File.Create(path))
            {
                _serializer.WriteObject(stream, configs);
            }
        }

        public static IEnumerable<Config> ConfigsDefault()
        {
            return new List<Config> {
                new Config()
            {
                message = "Сделайте гимнастику для глаз",
                Rest = new TimeInfo() { Number=15, Sign="m"},
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
                Work = new TimeInfo() { Number=30, Sign="m"},
             },
              new Config() {
                message = "Передохните-test",
                Rest = new TimeInfo() { Number=10, Sign="s"},
                Work = new TimeInfo() { Number=10, Sign="s" },
             },
              new Config() {
                message = "Передохните-test2",
                Rest = new TimeInfo() { Number=5, Sign="s"},
                Work = new TimeInfo() { Number=5, Sign="s" },
             }
            };
        }

        public static string PathConfigDefault() => 
            Path.Combine(Directory.GetCurrentDirectory(), "ConfigTime.json");
    }
}
