using System;
using System.Collections.Generic;

namespace Thalins.PMDnD
{
    internal class Program
    {
        private static void Main()
        {
            /*if (!Config.Load() || !Sheets.Initialize(Config.SpreadsheetID))
            {
                return;
            }
            /*CharacterSheet Template = new CharacterSheet("Template");
            CharacterSheet Template2 = new CharacterSheet("Template 2");
            Battle.Start(new List<CharacterSheet> { Template, Template2 });*/

            UI ui = new UI();
            ui.Run();
        }

        //static void OldMain(string[] args)
        //{
        //    Sheets.Initialize("1vQzjjcZCzo_ZGyZrU_q1jcIBu1K96a0jPgG8Rb9V4hU");

        //    UserCredential credential;

        //    using (var stream =
        //        new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        //    {
        //        string credPath = "token.json";
        //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //            GoogleClientSecrets.Load(stream).Secrets,
        //            Scopes,
        //            "user",
        //            CancellationToken.None,
        //            new FileDataStore(credPath, true)).Result;
        //        Console.WriteLine("Credential file saved to: " + credPath);
        //    }

        //    var service = new SheetsService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = ApplicationName,
        //    });

        //    string spreadsheetId = "1vQzjjcZCzo_ZGyZrU_q1jcIBu1K96a0jPgG8Rb9V4hU";
        //    string range = "NAME";
        //    SpreadsheetsResource.ValuesResource.GetRequest request =
        //        service.Spreadsheets.Values.Get(spreadsheetId, range);

        //    ValueRange response = request.Execute();
        //    IList<IList<object>> values = response.Values;
        //    if (values != null && values.Count > 0)
        //    {
        //        Console.WriteLine("Name, Species, Base HP");
        //        foreach (var row in values)
        //        {
        //                Console.WriteLine("{0}", row[0]);
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("No data found.");
        //    }
        //    Console.Read();
        //}
    }
}
