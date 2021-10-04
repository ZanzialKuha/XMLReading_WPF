using System.Collections.Generic;

namespace XMLReading_WPF
{
    // считываем файл Load.xml из корня программы
    // и сохраняем в корень Save.xml с результатом
    class LoadDefault : ILoadFile
    {
        public void Load(Dictionary<string, string> args, out LoadObj fsSource)
        {
            fsSource = new LoadObj_File("Load.xml");
        }
        public string Variant() => "по умолчанию (из корневого каталога)";
    }
}
