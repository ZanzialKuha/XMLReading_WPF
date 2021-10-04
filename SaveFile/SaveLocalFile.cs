using System.Collections.Generic;

namespace XMLReading_WPF
{
    class SaveLocalFile : ISaveFile
    {
        public void Save(Dictionary<string, string> args, out SaveObj fsNew)
        {
            string pathNew;
            if (args.ContainsKey("pathNew") == true)
            {
                pathNew = args["pathNew"];
            }
            else
            {
                pathNew = "Save.xml";
            }

            fsNew = new SaveObj_File(pathNew);
        }
    }
}
