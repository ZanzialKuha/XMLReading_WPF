using System;
using System.IO;
using System.Collections.Generic;

namespace XMLReading_WPF
{
    interface ILoadFile
    {
        void Load(Dictionary<string, string> args, out FileStream fsSource, out FileStream fsNew);
        string Variant();
    }
}
