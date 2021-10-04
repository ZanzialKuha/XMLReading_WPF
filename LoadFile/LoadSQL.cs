using System.Collections.Generic;

namespace XMLReading_WPF
{
    class LoadSQL : ILoadFile
    {
        public void Load(Dictionary<string, string> args, out LoadObj fsSource)
        {
            fsSource = new LoadObj_SQL();
        }

        public string Variant() => "с сервера SQL";
    }
}
