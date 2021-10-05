using System.Text;
using System.Windows;
using System.IO;

namespace XMLReading_WPF
{
    class SaveObj_File : SaveObj
    {
        public string pathNew { get; set; } = "Save.xml";
        private FileStream fsNew { get; set; }
        public SaveObj_File(string pathNew)
        {
            try
            {
                fsNew = new FileStream(pathNew, FileMode.Create, FileAccess.Write);
                fsNew.Write(Encoding.Default.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Houses>"));
                IsReady = true;
            }
            catch
            {
                MessageBox.Show("Не удалось создать файл \"Save.xml\" в корневом каталоге");
                IsReady = false;
            }
        }
        public override void Add(string text)
        {
            fsNew.Write(Encoding.Default.GetBytes(text));
        }
        public override void Save()
        {
            fsNew.Flush();
        }
        public override void Close()
        {
            fsNew.Write(Encoding.Default.GetBytes("</Houses>"));
            fsNew.Close();
        }
    }
}
