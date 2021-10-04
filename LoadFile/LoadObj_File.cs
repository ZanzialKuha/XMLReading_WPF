using System;
using System.IO;
using System.Windows;
using System.Text;

namespace XMLReading_WPF
{
    class LoadObj_File : LoadObj
    {
        public string pathSource { get; set; } = "Load.xml";
        public FileStream fsSource { get; private set; }

        private static int bufferSize = 10000000;    // количество байт для поиска
        private byte[] bytes = new byte[bufferSize]; // промежуточный массив для чтения потока для поиска закрывающегося тега
        public LoadObj_File(string pathSource)
        {
            try
            {
                fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);
                Length = fsSource.Length;
            }
            catch
            {
                MessageBox.Show($"Не удалось открыть файл \"{pathSource}\"");
                fsSource = null;
            }
        }
        public override int Read(int readSize = 10000000)
        {
            int n = fsSource.Read(bytes, 0, readSize);

            Text = Encoding.Default.GetString(bytes, 0, readSize);

            // зачистим память, чтобы не загружать все 35Гб в память
            Array.Clear(bytes, 0, n);

            return n;
        }

        public override void Close()
        {
            fsSource.Close();
        }
    }
}