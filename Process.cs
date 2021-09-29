using System;
using System.IO;
using System.Text;
using static System.TimeSpan;

namespace XMLReading_WPF
{
    class Process
    {
        public FileStream fsSource { get; set; }
        public FileStream fsNew { get; set; }
        public string condition { get; set; }

        private static int sizeOfBlock = 1000;       // размер блока для поиска
        private static int bufferSize = 10000000;    // количество байт для поиска
        private string bufferText = "";              // остаток от прочитанной строки кинем сюда, чтобы обработать со следующей строкой
        private byte[] bytes = new byte[bufferSize]; // промежуточный массив для чтения потока для поиска закрывающегося тега
        public Process(FileStream _fsSource, FileStream _fsNew, string _condition)
        {
            fsSource = _fsSource;
            fsNew = _fsNew;
            condition = _condition;
        }
        public void XMLReading(IProgress<string> progress)
        {
            long length = fsSource.Length;
            // будем читать файл потоком и обрабатывать
            int posBytes = 0;                 // текущая позиция в массиве
            long numBytesRead = 0;             // общий прогресс

            // сформируем XML-файл
            fsNew.Write(Encoding.Default.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Houses>"));

            while (numBytesRead < length)
            {
                // Read возвращает что угодно от 0 до bufferSize.
                int n = fsSource.Read(bytes, posBytes, bufferSize);

                // Херня случилась - выйдем тут
                if (n == 0)
                    break;

                numBytesRead += n;
                posBytes += n;

                SearchCondition();

                // зачистим память, чтобы не загружать все 35Гб в память
                Array.Clear(bytes, 0, posBytes);
                posBytes = 0;

                // прогресс
                if (numBytesRead % 100000 == 0)
                {
                    Console.WriteLine("Прочитано {0:n} из {1:n} ({2:P})", numBytesRead, length, 1.0 * numBytesRead / length);
                    //progress.Report((1.0 * numBytesRead / length));
                    progress.Report($"Прочитано {numBytesRead:n} из {length:n} ({1.0 * numBytesRead / length:P})");
                    //Console.SetCursorPosition(0, Console.CursorTop - 1);
                    fsNew.Flush(); // сбросим что есть из буффера в файл
                }
            }

            fsNew.Write(bytes, 0, posBytes);
            fsNew.Write(Encoding.Default.GetBytes("</Houses>"));
            fsNew.Close();
            fsSource.Close();
            Console.WriteLine($"Всего прочитано {numBytesRead:n}");
        }

        private void SearchCondition()
        {
            // проверим что мы получили из файла
            string text = bufferText + Encoding.Default.GetString(bytes);
            int ixText = 1;
            int posSearch = 0;
            while (posSearch < text.Length && ixText > 0)
            {
                // проверяем блочно по 1к символов (средняя длина ~500, взял с запасом)
                ixText = text.IndexOf("/>", posSearch, (text.Length > posSearch + sizeOfBlock ? sizeOfBlock : text.Length - posSearch));
                if (ixText > 0)
                    // проверим заданное условие поиска
                    if (text.IndexOf(condition, posSearch, ixText - posSearch + 2) > 0)
                        fsNew.Write(Encoding.Default.GetBytes(text.Substring(posSearch, ixText - posSearch + 2)));

                // двигаемся по тексту
                posSearch = (ixText > 0 ? ixText + 2 : posSearch);
            }

            // остатки скинем на следующую проверку, чтобы не потерять данные
            bufferText = text.Substring(posSearch, text.Length - posSearch);
        }
    }
}
