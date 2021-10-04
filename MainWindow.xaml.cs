using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace XMLReading_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // дефолтное имя файла
            dlg.DefaultExt = ".xml"; // дефолтное расширение
            dlg.Filter = "Файл формата (.xml)|*.xml"; // фильтр по расширению

            // Отображаем диалог пользователю
            Nullable<bool> result = dlg.ShowDialog();

            // результат
            if (result == true)
            {
                // открыли документ
                string filename = dlg.FileName;
                Path.Text = dlg.FileName;

                // выведем превью
                XMLContent_PreviewXML(Path.Text);
            }
        }
        private void RB1_Checked(object sender, RoutedEventArgs e)
        {
            if (Path != null)
            {
                Path.Text = "";
                Path.IsEnabled = false;
            }
            if (Bttn1 != null)
                Bttn1.IsEnabled = false;

            // выведем превью
            XMLContent_PreviewXML("Load.xml");
        }

        private void RB2_Checked(object sender, RoutedEventArgs e)
        {
            Path.IsEnabled = true;
            Bttn1.IsEnabled = true;
        }
        private void XMLContent_PreviewXML(string pathXML)
        {
            try
            {
                /*
                FileStream fsSource = new FileStream(pathXML, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[2000]; // промежуточный массив для чтения потока
                fsSource.Read(bytes, 0, 2000);
                XMLContent.Text = Encoding.Default.GetString(bytes).Replace("><", ">\n    <");
                fsSource.Close();
*/

                LoadObj_File fsSource = new LoadObj_File(pathXML);
                if (fsSource.fsSource != null)
                {

                    fsSource.Read(2000);
                    XMLContent.Text = fsSource.Text.Replace("><", ">\n    <");
                    fsSource.Close();
                }
                else
                    XMLContent.Text = "<Отсутствует файл для предпросмотра>";


            }
            catch
            {
                XMLContent.Text = "<Отсутствует файл для предпросмотра>";
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ILoadFile[] load = new ILoadFile[]
            {
                new LoadDefault(),
                new LoadLocalFile()
            };

            ISaveFile[] save = new ISaveFile[]
            {
                new SaveDefault(),
                new SaveLocalFile(),
                new SaveSQL()
            };

            int variantLoad = 0;
            if (RB1.IsChecked == true)
                variantLoad = 0;
            else if (RB2.IsChecked == true)
                variantLoad = 1;

            if (variantLoad > -1 && variantLoad < load.Length)
            {
                LoadObj fsSource;
                SaveObj fsNew;

                Dictionary<string, string> dictArgs = new Dictionary<string, string>
                {
                    ["pathSource"] = Path.Text
                };

                load[variantLoad].Load(dictArgs, out fsSource);
                save[0].Save(dictArgs, out fsNew);

                if (fsSource == null || fsNew == null)
                {
                    MessageBox.Show("Операция прервана");
                }
                else
                {
                    var progress = new Progress<string>(s => pbStatus.Content = s);

                    Process process = new Process(fsSource, fsNew, Condition.Text);
                    await Task.Run(() => process.XMLReading(progress));

                    MessageBox.Show("Операция завершена");
                    pbStatus.Content = "";
                }
            }
            else
            {
                MessageBox.Show("Неккоректный выбор варианта.");
            }
        }
    }
}
