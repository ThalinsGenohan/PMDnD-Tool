using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Thalins.PMDnD
{
    static class Config
    {
        const string configFile = "config.json";

        private static ConfigObject _cfg;

        public static string SpreadsheetID { get => _cfg?.SpreadsheetID; }

        public static void Load()
        {
            if (!File.Exists(configFile))
            {
                Console.WriteLine("ERROR! Config file \"{0}\" not found! A new one will be generated for you. Please input the ID of the Google Sheet you are accessing before relaunching.", configFile);
                File.WriteAllText(configFile, JsonConvert.SerializeObject(_cfg));
            }

            _cfg = JsonConvert.DeserializeObject<ConfigObject>(File.ReadAllText(configFile));
        }
    }

    class ConfigObject
    {
        public string SpreadsheetID = "";
    }
}
