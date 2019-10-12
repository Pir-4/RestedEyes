﻿using System;
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
        public static bool Exist(string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = ConfigManager.PathDefault;
            return File.Exists(path);
        }

        public static IEnumerable<Config> Read(string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                var _serializer = new DataContractJsonSerializer(typeof(IEnumerable<Config>));
                return (IEnumerable<Config>)_serializer.ReadObject(stream);
            }
        }

        public static void Write(string path, Config[] configs)
        {

            using (var stream = File.Create(path))
            {
                var _serializer = new DataContractJsonSerializer(typeof(IEnumerable<Config>));
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
             /* new Config() {
                message = "Передохните-test",
                Rest = new TimeInfo() { Number=10, Sign="s"},
                Work = new TimeInfo() { Number=10, Sign="s" },
             },
              new Config() {
                message = "Передохните-test2",
                Rest = new TimeInfo() { Number=5, Sign="s"},
                Work = new TimeInfo() { Number=5, Sign="s" },
             }*/
            };
        }

        public static string PathDefault
        {  
            get { return Path.Combine(Directory.GetCurrentDirectory(), "ConfigTime.json"); }
        }
    }
}