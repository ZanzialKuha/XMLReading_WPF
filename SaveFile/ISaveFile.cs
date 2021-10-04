using System.Collections.Generic;

namespace XMLReading_WPF
{
    interface ISaveFile
    {
        void Save(Dictionary<string, string> args, out SaveObj fsNew);
    }
}
