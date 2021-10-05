using System.Collections.Generic;

namespace XMLReading_WPF
{
    class SaveSQL : ISaveFile
    {
        public void Save(Dictionary<string, string> args, out SaveObj fsNew)
        {
            string connectionString;
            if (args.ContainsKey("ServerSettings") == true)
            {
                string[] settings = args["ServerSettings"].Split(';');
                connectionString = @"Data Source=" + settings[0] + ";Initial Catalog=" + settings[1] + ";User ID=" + settings[2] + ";Password=" + settings[3];
            }
            else
            {
                connectionString = "";
            }

            fsNew = new SaveObj_SQL(connectionString);
        }
    }
}
