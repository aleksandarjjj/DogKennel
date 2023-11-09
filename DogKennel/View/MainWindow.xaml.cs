﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DogKennel.Model;
using Microsoft.Win32;
using Microsoft.Data.SqlClient;

namespace DogKennel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnInsertSampleData_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            bool? result = dialog.ShowDialog();
            DataReader.ReadExcel(dialog.FileName, out DataTable? dt);

            DataAccess da = new DataAccess();
            bool hi = da.TestConnection();
        }
    }
}