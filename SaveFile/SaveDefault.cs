using System.Collections.Generic;

namespace XMLReading_WPF
{
    class SaveDefault : ISaveFile
    {
        public void Save(Dictionary<string, string> args, out SaveObj fsNew)
        {
            fsNew = new SaveObj_File("Save.xml");
        }
    }
}
