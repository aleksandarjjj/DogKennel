﻿using DogKennel.ViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace DogKennel.View
{
    public partial class StartupWindow : Window
    {
        ViewModel _viewModel = new ViewModel();

        public StartupWindow()
        {
            DataContext = _viewModel;
            InitializeComponent();
            this.Show();
            MainWindow_Sync();
        }
        private void MainWindow_Sync()
        {
            bool viewModelBoolean;

            //Test connection by pinging server
            viewModelBoolean = _viewModel.TestConnection();

            //Synch with local collections
            if (viewModelBoolean) { viewModelBoolean = _viewModel.SelectAll(); }

            //Prompt messages to user
            switch (viewModelBoolean)
            {
                case true:
                    btnAddFile.IsEnabled = true;
                    if (0 < _viewModel.DogCount) { btnTruncate.IsEnabled = true; }
                    else if (_viewModel.DogCount == 0)
                    {
                        MessageBox.Show($"Databasen har i øjeblikket ingen hunde." +
                            $"\nTryk på \"Tilføj hund\" eller \"Indlæs fil\" for at tilføje hunde.", "Startup Dialog");
                    }
                    break;
                case false:
                    MessageBox.Show($"Der kunne ikke oprettes forbindelse til databasen." +
                        $"\nTjek venligst database credentials eller VPN.", "Startup Dialog");
                    break;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            FileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? viewModelBoolean;

            //Select file to read
            bool? viewBoolean = openFileDialog.ShowDialog();

            //Switch to read file if a file was selected
            switch (viewBoolean)
            {
                case true:
                    string _filePath = openFileDialog.FileName;
                    viewModelBoolean = _viewModel.ReadExcel(_filePath);
                    break;
                default:
                    return;
            }

            //Prompt messages to user
            switch (viewModelBoolean)
            {
                case true:
                    btnTruncate.IsEnabled = true;
                    break;
                case false:
                    MessageBox.Show($"Den valgte fil kunne ikke indlæses.", "Indlæs fil");
                    break;
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnTruncate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult viewResult;
            bool viewModelBoolean;

            viewResult = MessageBox.Show("Er du sikker på, at du vil rydde databasen?\nDette sletter alle hunde.", "Clear Database", (MessageBoxButton)4);

            switch (viewResult)
            {
                case MessageBoxResult.Yes:
                    viewModelBoolean = _viewModel.Truncate();

                    if (viewModelBoolean)
                    {
                        btnTruncate.IsEnabled = false;
                        btnAddFile.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show($"Programmet kunne ikke forbinde til databsen.");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        private void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            bool viewModelBoolean;

            //Test connection by pinging server
            viewModelBoolean = _viewModel.TestConnection();

            //Prompt messages to user
            switch (viewModelBoolean)
            {
                case true:
                    MessageBox.Show($"Programmet er forbundet til databasen.", "Test forbindelse");
                    btnAddFile.IsEnabled= true;
                    break;
                case false:
                    MessageBox.Show($"Der kunne ikke oprettes forbindelse til databasen." +
                        $"\nTjek venligst database credentials eller VPN.", "Test forbindelse");
                    break;
            }
        }

        private void TblDogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TblDogs.SelectedItem != null)
            {
                btnDelete.IsEnabled = true;
            }
        }
    }
}
