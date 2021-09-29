using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;

namespace XMLReading_WPF
{
    class LoadLocalFile : ILoadFile
    {
        public void Load(Dictionary<string, string> args, out FileStream fsSource, out FileStream fsNew)
        {
            string pathSource;
            if (args.ContainsKey("path") == true)
            {
                pathSource = args["path"];
            }
            else
            {
                Console.WriteLine("Введите полный путь до файла: ");
                pathSource = Console.ReadLine();
            }

            string pathNew = Path.GetDirectoryName(pathSource) + "\\Save.XML";

            try
            {
                fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                MessageBox.Show($"Не удалось открыть файл \"{pathSource}\"");
                fsSource = null;
            }

            try
            {
                fsNew = new FileStream(pathNew, FileMode.Create, FileAccess.Write);
            }
            catch
            {
                MessageBox.Show($"Не удалось создать файл \"{pathNew}\"");
                fsNew = null;
            }


            if (fsSource != null && fsNew != null)
                Console.WriteLine($"\nЗагрузили файл \"{Path.GetFileName(pathSource)}\"\n");
        }

        public string Variant() => "указать файл на диске";
    }
}
