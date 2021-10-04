using System.Collections.Generic;

namespace XMLReading_WPF
{
    interface ILoadFile
    {
        void Load(Dictionary<string, string> args, out LoadObj fsSource);
        string Variant();
    }
}
