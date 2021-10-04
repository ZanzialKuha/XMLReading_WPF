using System.IO;
using System.Collections.Generic;
using System.Windows;

namespace XMLReading_WPF
{
    class LoadLocalFile : ILoadFile
    {
        public void Load(Dictionary<string, string> args, out LoadObj fsSource)
        {
            string pathSource;
            if (args.ContainsKey("pathSource") == true)
            {
                pathSource = args["pathSource"];
            }
            else
            {
                pathSource = "Load.xml";
            }

            fsSource = new LoadObj_File(pathSource);

            if (fsSource != null)
                MessageBox.Show($"\nЗагрузили файл \"{Path.GetFileName(pathSource)}\"\n");
        }

        public string Variant() => "указать файл на диске";
    }
}