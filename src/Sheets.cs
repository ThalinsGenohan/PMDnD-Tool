using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace Thalins.PMDnD
{
    static class Sheets
    {
        public class Data
        {
            public string Name { get; private set; }
            public string Position { get; private set; }
            public string Group { get; private set; }
            public string Type { get; private set; }

            public Data(string name, string position, string group = "", string type = "")
            {
                Name = name;
                Position = position;
                Group = group;
                Type = type;
            }
        }

        public static Dictionary<string, Data> Ranges;

        public static class StandardRanges
        {
            public static string Gender;
            public static string Image;
            public static string Player;
            public static string Status;
            public static string HP;
            public static string MaxHP;
            public static string HPBar;
            public static string Name;
            public static string Species;
            public static string Type1;
            public static string Type2;
            public static string TypeIcon1;
            public static string TypeIcon2;
            public static string Level;
            public static class Actions
            {
                public static string Base;
                public static string Buff;
                public static string Status;
                public static string Total;
            }
            public static string Ability;
            public static string Exp;
            public static class Strength
            {
                public static string Base;
                public static string Boost;
                public static string Buff;
                public static string Total;
            }
            public static class Special
            {
                public static string Base;
                public static string Boost;
                public static string Buff;
                public static string Total;
            }
            public static class Speed
            {
                public static string Base;
                public static string Boost;
                public static string Buff;
                public static string Total;
            }
            public static class Vitality
            {
                public static string Base;
                public static string Boost;
                public static string Buff;
                public static string Total;
            }
            public static class Accuracy
            {
                public static string Equip;
                public static string Buff;
                public static string Debuff;
                public static string Total;
            }
            public static class Evasion
            {
                public static string Equip;
                public static string Buff;
                public static string Debuff;
                public static string Total;
            }
            public static class Defense
            {
                public static string Base;
                public static string Equip;
                public static string Buff;
                public static string Total;
            }
            public static string Money;
            public static string Class;

            public static void Initialize(IList<IList<Object>> values)
            {
                Console.WriteLine("Initializing range constants...");
                List<string> ranges = new List<string>();
                for (var i = 0; i < values.Count; i++)
                {
                    ranges.Add(values[i][1].ToString());
                    Console.WriteLine("{0}: {1} - {2}", i, values[i][0], ranges[i]);
                }
                Gender =            ranges[0];
                Image =             ranges[1];
                Player =            ranges[2];
                Status =            ranges[3];
                HP =                ranges[4];
                MaxHP =             ranges[5];
                HPBar =             ranges[6];
                Name =              ranges[7];
                Species =           ranges[8];
                Type1 =             ranges[9];

                Type2 =             ranges[10];
                TypeIcon1 =         ranges[11];
                TypeIcon2 =         ranges[12];
                Level =             ranges[13];
                Actions.Base =      ranges[14];
                Actions.Buff =      ranges[15];
                Actions.Status =    ranges[16];
                Actions.Total =     ranges[17];
                Ability =           ranges[18];
                Exp =               ranges[19];

                Strength.Base =     ranges[20];
                Strength.Boost =    ranges[21];
                Strength.Buff =     ranges[22];
                Strength.Total =    ranges[23];
                Special.Base =      ranges[24];
                Special.Boost =     ranges[25];
                Special.Buff =      ranges[26];
                Special.Total =     ranges[27];
                Speed.Base =        ranges[28];
                Speed.Boost =       ranges[29];

                Speed.Buff =        ranges[30];
                Speed.Total =       ranges[31];
                Vitality.Base =     ranges[32];
                Vitality.Boost =    ranges[33];
                Vitality.Buff =     ranges[34];
                Vitality.Total =    ranges[35];
                Accuracy.Equip =    ranges[36];
                Accuracy.Buff =     ranges[37];
                Accuracy.Debuff =   ranges[38];
                Accuracy.Total =    ranges[39];

                Evasion.Equip =     ranges[40];
                Evasion.Buff =      ranges[41];
                Evasion.Debuff =    ranges[42];
                Evasion.Total =     ranges[43];
                Defense.Base =      ranges[44];
                Defense.Equip =     ranges[45];
                Defense.Buff =      ranges[46];
                Defense.Total =     ranges[47];
                Money =             ranges[48];
                Class =             ranges[49];
            }
        }

        private static UserCredential Credential;
        private static SheetsService Service;

        private static string SpreadsheetId;
        private static string DmSpreadsheetId;

        public static void Initialize(string spreadsheetId, string dmSpreadsheetId = "")
        {
            SpreadsheetId = spreadsheetId;
            DmSpreadsheetId = dmSpreadsheetId;
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                string[] scopes = { SheetsService.Scope.Spreadsheets };
                Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = "PMD&D Tool"
            });

            SpreadsheetsResource.ValuesResource.GetRequest initRequest =
                Service.Spreadsheets.Values.Get(SpreadsheetId, "DATA_CONSTANTS");
            ValueRange initResponse = initRequest.Execute();

            StandardRanges.Initialize(initResponse.Values);

            //foreach (var row in initResponse.Values)
            //{
            //    //Ranges.Add(row[0].ToString(), new Data(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString()));
            //}
        }

        public static IList<IList<object>> GetFromSheet(string range, bool dmSheet = false)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                Service.Spreadsheets.Values.Get(dmSheet ? DmSpreadsheetId : SpreadsheetId, range);
            ValueRange response = request.Execute();
            return response.Values;
        }

        public static Dictionary<string, string> GetFromSheet(List<string> ranges)
        {
            SpreadsheetsResource.ValuesResource.BatchGetRequest request =
                Service.Spreadsheets.Values.BatchGet(SpreadsheetId);
            request.Ranges = ranges;
            var response = request.Execute();

            Dictionary<string, string> values = new Dictionary<string, string>();

            int i = 0;
            foreach (var range in response.ValueRanges)
            {
                foreach (var value in range.Values)
                {
                    values.Add(ranges[i], value[0].ToString());
                    i++;
                }
            }

            return values;
        }
    }
}
