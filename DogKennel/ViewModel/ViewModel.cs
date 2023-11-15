using DogKennel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace DogKennel.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        //Initializing collections to accomodate data from the SQL-tables
        public ObservableCollection<Dog> TblDogs { get; set; } = new ObservableCollection<Dog>();
        public ObservableCollection<Health> TblDogHealth { get; set; } = new ObservableCollection<Health>();
        public ObservableCollection<Pedigree> TblDogPedigree { get; set; } = new ObservableCollection<Pedigree>();

        //Other property and field definitions
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

        //Deleting a record across all tables and collections
        public bool Delete()
        {
            bool modelBoolean;

            //Try deletion for database followed by deletion for local collections
            modelBoolean = _dataAccess.CommandDelete(CurrentDog.PedigreeID);

            switch (modelBoolean)
            {
                case true:
                    //Deletion must be done from lowest referenced tables first
                    //Using LINQ to find object based on ID
                    TblDogHealth.Remove(TblDogHealth.ToList().Find(health => health.PedigreeID == CurrentDog.PedigreeID));
                    TblDogPedigree.Remove(TblDogPedigree.ToList().Find(pedigree => pedigree.PedigreeID == CurrentDog.PedigreeID));

                    //Delete from main collection (containing references)
                    TblDogs.Remove(CurrentDog);

                    DogCount--;
                    CurrentDog = null;
                    return true;
                case false:
                    return false;
            }
        }
        
        //CLearing tables and collections
        public bool Truncate()
        {
            bool modelBoolean;

            //List of enums initialized to handle data columns
            List<Enum> enums = new List<Enum>() { new TblDogHealth(), new TblDogPedigree(), new TblDogs(), };

            //Convert from ObservableCollection to generic collections for running LINQ queries
            List<Dog> tempTblDogs = TblDogs.ToList();
            List<Health> tempTblDogHealth = TblDogHealth.ToList();
            List<Pedigree> tempTblDogPedigree = TblDogPedigree.ToList();

            foreach (Enum e in enums)
            {
                //Attempt to truncate database
                modelBoolean = _dataAccess.CommandTruncateBuilder(e); if (!modelBoolean) { return false; }

                //Clear local collections if database is truncated
                switch (e.GetType().Name)
                {
                    case "TblDogs":
                        foreach (Dog dog in tempTblDogs)
                        {
                            TblDogs.Remove(dog);
                            DogCount--;
                        }
                        break;
                    case "TblDogHealth":
                        foreach (Health health in tempTblDogHealth)
                        {
                            TblDogHealth.Remove(health);
                        }
                        break;
                    case "TblDogPedigree":
                        foreach (Pedigree pedigree in tempTblDogPedigree)
                        {
                            TblDogPedigree.Remove(pedigree);
                        }
                        break;
                }
            }
            return true;
        }

        //Testing connection to database
        public bool TestConnection()
        {
            bool modelBoolean;
            
            modelBoolean = _dataAccess.CommandTestConnection();

            switch (modelBoolean)
            {
                case true:
                    return true;
                case false:
                    return false;
            }
        }

        //Reading Excel file
        public bool ReadExcel(string _filePath)
        {
            DataTable? readTable;
            bool modelBoolean;

            //List of enums initialized to handle data columns
            List<Enum> enums = new List<Enum>() { new TblDogs(), new TblDogHealth(), new TblDogPedigree() };

            //Read Excel document
            modelBoolean = DataReader.ReadExcel(_filePath, out readTable); if (!modelBoolean) { return false; }

            //Insert cleaned and trimmed data into all databases
            foreach (Enum enumerator in enums) { modelBoolean = _dataAccess.CommandBulkInsertBuilder(readTable, enumerator); if (!modelBoolean) { return false; } }

            //Synchronize class collections with database
            modelBoolean = SelectAll(); if (!modelBoolean) { return false; }

            return true;
        }

        //Selecting all records to populate local collections
        public bool SelectAll()
        {
            DataTable? readTable;
            bool modelBoolean;

            //List of enums initialized to handle data columns
            List<Enum> enums = new List<Enum>() { new TblDogs(), new TblDogHealth(), new TblDogPedigree() };

            //Select all from all tables and insert into class collections
            foreach (Enum enumerator in enums)
            {
                readTable = new DataTable();

                //If selection fails returns false
                modelBoolean = _dataAccess.CommandSelectAllBuilder(enumerator, out readTable); if (!modelBoolean) { return false; }

                foreach (DataRow dataRow in readTable.Rows)
                {
                    object[] convertedRow = dataRow.ItemArray;

                    //Populate collections along with type conversion from SQL to C#
                    switch (enumerator.GetType().Name)
                    {
                        case "TblDogs":
                            TblDogs.Add(DataAccess.DogConstructor(convertedRow));
                            DogCount++;
                            break;
                        case "TblDogHealth":
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

        //Handling INotifyPropertyChanged for databinding to XAML
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}