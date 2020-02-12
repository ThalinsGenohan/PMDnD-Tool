using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Thalins.PMDnD
{
    static class Config
    {
        private static ConfigObject _cfg;

        public static string SpreadsheetID { get => _cfg?.SpreadsheetID; }

        public static void Load()
        {
            _cfg = JsonConvert.DeserializeObject<ConfigObject>(File.ReadAllText("config.json"));
        }
    }

    class ConfigObject
    {
        public string SpreadsheetID = "";
    }
}
