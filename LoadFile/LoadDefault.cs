using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;

namespace XMLReading_WPF
{
    // считываем файл Load.xml из корня программы
    // и сохраняем в корень Save.xml с результатом
    class LoadDefault : ILoadFile
    {
        public void Load(Dictionary<string, string> args, out FileStream fsSource, out FileStream fsNew)
        {
            string pathSource = "Load.xml";
            string pathNew = "Save.xml";

            try
            {
                fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                MessageBox.Show("Нет файла \"Load.xml\" в корневом каталоге");
                fsSource = null;
            }

            try
            {
                fsNew = new FileStream(pathNew, FileMode.Create, FileAccess.Write);
            }
            catch
            {
                MessageBox.Show("Не удалось создать файл \"Save.xml\" в корневом каталоге");
                fsNew = null;
            }


            if (fsSource != null && fsNew != null)
                MessageBox.Show("\nЗагрузили файл \"Load.xml\" из корневого каталога\n");
        }
        public string Variant() => "по умолчанию (из корневого каталога)";
    }
}
