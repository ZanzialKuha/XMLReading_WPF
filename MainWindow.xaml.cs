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
        private void BttnSource_Click(object sender, RoutedEventArgs e)
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
                PathSource.Text = dlg.FileName;
                PathNew.Text = System.IO.Path.GetDirectoryName(dlg.FileName) + "\\Save.XML";

                // выведем превью
                XMLContent_PreviewXML(PathSource.Text);
            }
        }
        private void BttnNew_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
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
                PathNew.Text = dlg.FileName;
            }
        }
        private void RBSource1_Checked(object sender, RoutedEventArgs e)
        {
            if (PathSource != null)
            {
                PathSource.Text = "";
                PathSource.IsEnabled = false;
            }
            if (BttnSource != null)
                BttnSource.IsEnabled = false;

            // выведем превью
            XMLContent_PreviewXML("Load.xml");
        }

        private void RBSource2_Checked(object sender, RoutedEventArgs e)
        {
            PathSource.IsEnabled = true;
            BttnSource.IsEnabled = true;
        }
        private void RBNew1_Checked(object sender, RoutedEventArgs e)
        {
            if (PathNew != null)
            {
                PathNew.Text = "";
                PathNew.IsEnabled = false;
            }
            if (BttnNew != null)
                BttnNew.IsEnabled = false;
        }

        private void RBNew2_Checked(object sender, RoutedEventArgs e)
        {
            PathNew.IsEnabled = true;
            BttnNew.IsEnabled = true;
        }
        private void XMLContent_PreviewXML(string pathXML)
        {
            try
            {
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
            if (RBSource1.IsChecked == true)
                variantLoad = 0;
            else if (RBSource2.IsChecked == true)
                variantLoad = 1;

            int variantSave = 0;
            if (RBSource1.IsChecked == true)
                variantSave = 0;
            else if (RBSource2.IsChecked == true)
                variantSave = 1;

            if (variantLoad > -1 && variantLoad < load.Length)
            {
                LoadObj fsSource;
                SaveObj fsNew;

                Dictionary<string, string> dictArgs = new Dictionary<string, string>
                {
                    ["pathSource"] = PathSource.Text,
                    ["pathNew"]    = PathNew.Text
                };

                load[variantLoad].Load(dictArgs, out fsSource);
                save[variantSave].Save(dictArgs, out fsNew);

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
