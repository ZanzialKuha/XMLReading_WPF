using System;
using System.IO;
using System.Text;
using static System.TimeSpan;

namespace XMLReading_WPF
{
    class Process
    {
        public LoadObj fsSource { get; set; }
        public SaveObj fsNew { get; set; }
        public string condition { get; set; }

        private static int sizeOfBlock = 1000;       // размер блока для поиска
        private string bufferText = "";              // остаток от прочитанной строки кинем сюда, чтобы обработать со следующей строкой

        public Process(LoadObj _fsSource, SaveObj _fsNew, string _condition)
        {
            fsSource = _fsSource;
            fsNew = _fsNew;
            condition = _condition;
        }
        public void XMLReading(IProgress<string> progress)
        {
            long length = fsSource.Length;
            long numBytesRead = 0;             // общий прогресс

            while (numBytesRead < length)
            {
                // Read возвращает что угодно от 0 до bufferSize.
                int n = fsSource.Read();

                // Херня случилась - выйдем тут
                if (n == 0)
                    break;

                numBytesRead += n;

                SearchCondition();

                // прогресс
                if (numBytesRead % 100000 == 0)
                {
                    Console.WriteLine("Прочитано {0:n} из {1:n} ({2:P})", numBytesRead, length, 1.0 * numBytesRead / length);
                    progress.Report($"Прочитано {numBytesRead:n} из {length:n} ({1.0 * numBytesRead / length:P})");
                    fsNew.Save(); // промежуточное сохранение
                }
            }

            fsNew.Finish();
            fsSource.Close();
            Console.WriteLine($"Всего прочитано {numBytesRead:n}");
        }

        private void SearchCondition()
        {
            // проверим что мы получили из файла
            string text = bufferText + fsSource.Text;
            int ixText = 1;
            int posSearch = 0;
            while (posSearch < text.Length && ixText > 0)
            {
                // проверяем блочно по 1к символов (средняя длина ~500, взял с запасом)
                ixText = text.IndexOf("/>", posSearch, text.Length > posSearch + sizeOfBlock ? sizeOfBlock : text.Length - posSearch);
                if (ixText > 0)
                    // проверим заданное условие поиска
                    if (text.IndexOf(condition, posSearch, ixText - posSearch + 2) > 0)
                        fsNew.Add(text.Substring(posSearch, ixText - posSearch + 2));

                // двигаемся по тексту
                posSearch = ixText > 0 ? ixText + 2 : posSearch;
            }

            // остатки скинем на следующую проверку, чтобы не потерять данные
            bufferText = text.Substring(posSearch, text.Length - posSearch);
        }
    }
}
