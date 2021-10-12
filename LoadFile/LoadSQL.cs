using System.Collections.Generic;

namespace XMLReading_WPF
{
    class LoadSQL : ILoadFile
    {
        public void Load(Dictionary<string, string> args, out LoadObj fsSource)
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

            fsSource = new LoadObj_SQL(connectionString);
        }

        public string Variant() => "с сервера SQL";
    }
}
