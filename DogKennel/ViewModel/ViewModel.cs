﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DogKennel.Model;

namespace DogKennel.ViewModels
{
    public class ViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<Dog> TblDogs { get; set; } = new ObservableCollection<Dog>();
        public ObservableCollection<Health> TblDogHealth { get; set; } = new ObservableCollection<Health>();
        public ObservableCollection<Pedigree> TblDogPedigree { get; set; } = new ObservableCollection<Pedigree>();

        public Dog? CurrentDog { get; set; } = null;
        private int _dogCount = 0;
        public int DogCount
        {
            get { return _dogCount; }
            set
            {
                _dogCount = value;
                OnPropertyChanged("DogCount");
            }
        }
        DataAccess _dataAccess = new DataAccess();


        public bool ReadExcel(string _filePath)
        {
            List<Enum> enums = new List<Enum>() { new TblDogs(), new TblDogHealth(), new TblDogPedigree() };
            DataTable? readTable;

            //Read Excel table
            bool modelBoolean = DataReader.ReadExcel(_filePath, out readTable); if (!modelBoolean) { return false; }

            //Insert into all tables in database
            foreach (Enum enumerator in enums) { modelBoolean = _dataAccess.CommandBulkInsertBuilder(readTable, enumerator); if (!modelBoolean) { return false; } }

            //Select from all tables and insert in class collections
            foreach (Enum enumerator in enums)
            {
                readTable = null;
                modelBoolean = _dataAccess.CommandSelectAllBuilder(enumerator, out readTable); if (!modelBoolean) { return false; }

                //Add rows to class collections
                foreach (DataRow dataRow in readTable.Rows)
                {
                    object[] convertedRow = dataRow.ItemArray;

                    switch (enumerator.GetType().Name)
                    {
                        case "TblDogs":
                            TblDogs.Add(DataAccess.DogConstructor(convertedRow));
                            DogCount++;
                            break;
                        case "TblDogHealth":;
                            TblDogHealth.Add(DataAccess.HealthConstructor(convertedRow));
                            break;
                        case "TblDogPedigree":
                            TblDogPedigree.Add(DataAccess.PedigreeConstructor(convertedRow));
                            break;
                    }
                }
            }

            return true;
        }

        public bool TestConnection()
        {
            bool modelBoolean = _dataAccess.CommandTestConnection();

            switch (modelBoolean)
            {
                case true:
                    return true;
                case false:
                    return false;
            }
        }
    }
}