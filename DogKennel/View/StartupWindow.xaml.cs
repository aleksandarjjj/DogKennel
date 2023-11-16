using DogKennel.ViewModels;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DogKennel.View
{
    public partial class StartupWindow : Window
    {
        ViewModel _viewModel = new ViewModel();

        public StartupWindow()
        {
            //Bind data
            DataContext = _viewModel;

            //Initialize startup window
            InitializeComponent();

            //Show startup window before messagebox with status for testing connection
            this.Show();
            MainWindow_Sync();
        }

        //Attempt to sync startupwindow with database
        private void MainWindow_Sync()
        {
            bool viewModelBoolean;

            //Test connection by pinging server with a set connection time
            viewModelBoolean = _viewModel.TestConnection();

            //Synch with local ViewModel collections
            if (viewModelBoolean) { viewModelBoolean = _viewModel.SelectAll(); }

            //Prompt message to user
            switch (viewModelBoolean)
            {
                case true:
                    btnAddFile.IsEnabled = true;
                    if (0 < _viewModel.DogCount) { btnTruncate.IsEnabled = true; btnAdd.IsEnabled = true; }
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

        //Open window for adding record
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
        }

        //Attempt to read excel file and sync to database
        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            FileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool? viewModelBoolean;

            //Select file to read
            bool? viewBoolean = openFileDialog.ShowDialog();

            //Switch to read file if a correct file format was selected
            switch (viewBoolean)
            {
                case true:
                    string _filePath = openFileDialog.FileName;
                    viewModelBoolean = _viewModel.ReadExcel(_filePath);
                    break;
                default:
                    return;
            }

            //Prompt message to user
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

        //Attempt to delete record
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            bool viewModelBoolean;

            viewModelBoolean = _viewModel.Delete();

            //Prompt message to user
            switch (viewModelBoolean)
            {
                case true:
                    TblDogs.SelectedItem = null;
                    btnDelete.IsEnabled = false;
                    break;
                case false:
                    MessageBox.Show($"Hunden kunne ikke slettes fra databasen. Prøv igen.", "Slet hund");
                    TblDogs.SelectedItem = null;
                    btnDelete.IsEnabled = false;
                    break;
            }
        }

        //Attempt to clear database and ViewModel collections
        private void btnTruncate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult viewResult;
            bool viewModelBoolean;

            //Prompt for user confirmation
            viewResult = MessageBox.Show("Er du sikker på, at du vil rydde databasen?\nDette sletter alle hunde.", "Ryd database", (MessageBoxButton)4);

            //Prompt message to user
            switch (viewResult)
            {
                case MessageBoxResult.Yes:
                    viewModelBoolean = _viewModel.TruncateAll();

                    if (viewModelBoolean)
                    {
                        btnTruncate.IsEnabled = false;
                        btnDelete.IsEnabled = false;
                        btnAddFile.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show($"Programmet kunne ikke forbinde til databsen.", "Ryd database");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        //Test connection to database
        private void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            bool viewModelBoolean;

            //Try-catch for showing same MessageBox
            try
            {
                //Test connection by pinging server
                viewModelBoolean = _viewModel.TestConnection(); if (!viewModelBoolean) { throw new Exception(); }

                if (viewModelBoolean)
                {
                    MessageBox.Show($"Programmet er forbundet til databasen.", "Test forbindelse");

                    //Sync
                    viewModelBoolean = _viewModel.SelectAll(); if (!viewModelBoolean) { throw new Exception(); }

                    if (0 < _viewModel.DogCount)
                    {
                        btnTruncate.IsEnabled = true;
                        btnAddFile.IsEnabled = true;
                        btnAdd.IsEnabled = true;
                    }
                    else if (_viewModel.DogCount == 0)
                    {
                        MessageBox.Show($"Databasen har i øjeblikket ingen hunde." +
                            $"\nTryk på \"Tilføj hund\" eller \"Indlæs fil\" for at tilføje hunde.", "Startup Dialog");
                        btnAddFile.IsEnabled = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"Der kunne ikke oprettes forbindelse til databasen." +
                                        $"\nTjek venligst database credentials eller VPN.", "Test forbindelse");
                btnTruncate.IsEnabled = false;
                btnAddFile.IsEnabled = false;
                btnAdd.IsEnabled = false;
            }
        }

        //Enable delete button if record is selected
        private void TblDogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TblDogs.SelectedItem != null)
            {
                btnDelete.IsEnabled = true;
            }
        }

        //Open window with collection information about the given dog
        private void TblDogs_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TblDogs.SelectedItem != null)
            {
                new ViewDog(_viewModel).ShowDialog();
            }
        }
    }
}
