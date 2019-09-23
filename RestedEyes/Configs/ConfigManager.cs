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
                timeRest = 15,
                timeRestSign = "m",
                timeWork = 1,
                timeWorkSign = "h"
            },
            new Config() {
                message = "Разомнитесь",
                timeRest = 15,
                timeRestSign = "m",
                timeWork = 2,
                timeWorkSign = "h"
            },
             new Config() {
                message = "Передохните",
                timeRest = 2,
                timeRestSign = "m",
                timeWork = 30,
                timeWorkSign = "m"
             },
              new Config() {
                message = "Передохните-test",
                timeRest = 10,
                timeRestSign = "s",
                timeWork = 10,
                timeWorkSign = "s"
             }
            };
        }

        public static string PathConfigDefault() => 
            Path.Combine(Directory.GetCurrentDirectory(), "ConfigTime.json");
    }
}
