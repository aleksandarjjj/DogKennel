using DogKennel.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DogKennel.View
{
    public partial class StartupWindow : Window
    {
        ViewModel _viewModel = new ViewModel();

        public StartupWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
            this.Show();
            MainWindow_Sync();
        }
        private void MainWindow_Sync()
        {
            bool viewModelBoolean = _viewModel.TestConnection();

            switch (viewModelBoolean)
            {
                case true:
                    if (0 < _viewModel.DogCount) { }
                    else if (_viewModel.DogCount == 0)
                    {
                        MessageBox.Show($"Databasen har i øjeblikket ingen hunde" +
                            $"\nTryk på \"Tilføj hund\" eller \"Indlæs fil\" for at tilføje hunde.", "Startup Dialog");
                    }
                    break;
                case false:
                    MessageBox.Show($"Der kunne ikke oprettes forbindelse til databasen" +
                        $"\nTjek venligst om en VPN er slået til eller om loginoplysningerne til databasen er korrekte.", "Startup Dialog");
                    break;
            }
        }

        //POPULATE
        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            FileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? viewModelBoolean;
            bool? viewBoolean = openFileDialog.ShowDialog();

            switch (viewBoolean)
            {
                case true:
                    string _filePath = openFileDialog.FileName;
                    viewModelBoolean = _viewModel.ReadExcel(_filePath);
                    break;
                default:
                    return;
            }

            switch (viewModelBoolean)
            {
                case true:
                    break;
                case false:
                    MessageBox.Show($"Den valgte fil kunne ikke indlæses.");
                    break;
            }
        }
    }
}
