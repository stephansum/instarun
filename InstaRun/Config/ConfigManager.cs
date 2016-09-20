﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace InstaRun
{
    public static class ConfigManager
    {

        public static Config CreateSampleConfig()
        {
            var settings = new Settings();
            var items = new List<Item>()
            {
                new Executable("Self", @"InstaRun.exe"),
                new Executable("VSCode", @"code", true, "."),
                new Executable("PowerShell", @"%SystemRoot%\system32\WindowsPowerShell\v1.0\powershell.exe"),
                new Executable("UserProfile", @"%USERPROFILE%"),
                new Separator(),
                new Container("Browsers", new List<Item>() {
                    new Executable("Internet Explorer (Google)", @"iexplore", true, "www.google.de"),
                    new Executable("Chrome", @"chrome", true),
                    new Container("Test Browsers", new List<Item>() {
                        new Executable("Firefox", @"firefox", true),
                    }),
                }),
            };
            var config = new Config(settings, items);
            return config;
        }


        public static Config GetConfig()
        {
            return Deserialize<Config>(App.PathToConfig);
        }


        public static void CreateSampleConfigXml()
        {
            if (!File.Exists(App.PathToSampleConfig))
            {
                var objectToSerialize = CreateSampleConfig();
                Serialize<Config>(objectToSerialize, App.PathToSampleConfig);
            }
        }


        public static T Deserialize<T>(string path)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                T result = default(T);

                using (var reader = new StreamReader(path))
                {
                    result = (T)xs.Deserialize(reader);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static void Serialize<T>(T objectToSerialize, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            var writerSettings = new XmlWriterSettings
            {
                Encoding = Encoding.Unicode,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
            };

            using (var writer = XmlWriter.Create(path, writerSettings))
            {
                xs.Serialize(writer, objectToSerialize);
            }
        }

    }
}
