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
        public static class Ranges
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

            public static void Initialize(IList<IList<Object>> values)
            {
                Console.WriteLine("Initializing range constants...");
                List<string> ranges = new List<string>();
                for (var i = 0; i < values.Count; i++)
                {
                    ranges.Add(values[i][0].ToString());
                    Console.WriteLine("{0} - {1}%", ranges[i], (float)i / values.Count * 100);
                }
                var index = 0;
                Gender = ranges[index++];
                Image = ranges[index++];
                Player = ranges[index++];
                Status = ranges[index++];
                HP = ranges[index++];
                MaxHP = ranges[index++];
                HPBar = ranges[index++];
                Name = ranges[index++];
                Species = ranges[index++];

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
                Service.Spreadsheets.Values.Get(SpreadsheetId, "RANGE_CONSTANTS");
            ValueRange initResponse = initRequest.Execute();

            Ranges.Initialize(initResponse.Values);
        }

        public static IList<IList<object>> GetFromSheet(string range, bool dmSheet = false)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                Service.Spreadsheets.Values.Get(dmSheet ? DmSpreadsheetId : SpreadsheetId, range);
            ValueRange response = request.Execute();
            return response.Values;
        }

        public static IList<object> GetFromSheet(List<string> ranges)
        {
            SpreadsheetsResource.ValuesResource.BatchGetRequest request =
                Service.Spreadsheets.Values.BatchGet(SpreadsheetId);
            request.Ranges = ranges;
            var response = request.Execute();
            foreach ()
            return response.Values;
        }
    }
}
