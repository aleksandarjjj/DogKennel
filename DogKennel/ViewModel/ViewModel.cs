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
        //Model objects
        //List of enums initialized to handle different data tables (by highest reference used to construct tables
        List<Enum> enums = new List<Enum>() { new TblDogs(), new TblDogHealth(), new TblDogPedigree() };
        DataAccess da = new DataAccess();

        //Collections for tablewise database handling
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

        //BulkDataHandling
        public bool ReadExcel(string _filePath)
        {
            DataTable? dt;

            //Try reading file
            if (!DataReader.ReadExcel(_filePath, out dt)) { return false; }

            //Try inserting into database tables
            if (!InsertAll(enums, dt)) { return false; }

            //Synchronize to class collections
            if (!SelectAll()) { return false; }

            return true;
        }
        public bool SelectAll()
        {
            DataTable? readTable;
            bool modelBool;

            //Select all from all tables and insert into class collections
            foreach (Enum e in enums)
            {
                readTable = new DataTable();

                //If selection fails returns false
                modelBool = da.CommandSelectAllBuilder(e, out readTable); if (!modelBool) { return false; }

                foreach (DataRow dataRow in readTable.Rows)
                {
                    object[] convertedRow = dataRow.ItemArray;

                    //Populate collections along with type conversion from SQL to C#
                    switch (e.GetType().Name)
                    {
                        case "TblDogs":
                            Dog dog = DataAccess.DogConstructor(convertedRow);
                            Add(dog);
                            break;
                        case "TblDogHealth":
                            Health health = DataAccess.HealthConstructor(convertedRow);
                            Add(health);
                            break;
                        case "TblDogPedigree":
                            Pedigree pedigree = DataAccess.PedigreeConstructor(convertedRow);
                            Add(pedigree);
                            break;
                    }
                }
            }
            return true;
        }
        public bool InsertAll(List<Enum> enumList, DataTable dt)
        {
            foreach (Enum e in enumList)
            {
                if (!da.CommandBulkInsertAllBuilder(e, dt)) { return false; }
            }
            return true;
        }
        public bool TruncateAll()
        {
            //Rearrange list to cycle lowest referenced tables first
            enums = new List<Enum>() { new TblDogHealth(), new TblDogPedigree(), new TblDogs() };

            foreach (Enum e in enums)
            {
                //Try database truncation
                if (!da.CommandTruncateBuilder(e)) { return false; }

                //Truncate local collection
                switch (e.GetType().Name)
                {
                    case "TblDogs":
                        TblDogs.Clear();
                        DogCount = 0;
                        break;
                    case "TblDogHealth":
                        TblDogHealth.Clear();
                        break;
                    case "TblDogPedigree":
                        TblDogPedigree.Clear();
                        break;
                }
            }

            enums = new List<Enum>() { new TblDogs(), new TblDogHealth(), new TblDogPedigree() };
            return true;
        }

        //SingularDataHandling
        public bool Delete()
        {
            //Try database deletion
            switch (da.CommandDelete(CurrentDog.PedigreeID))
            {
                case true:
                    //Collection deletion by lowest reference
                    Remove(
                    TblDogHealth
                        .ToList()
                        .Find(health => health.PedigreeID == CurrentDog.PedigreeID));

                    Remove(
                    TblDogPedigree
                        .ToList()
                        .Find(pedigree => pedigree.PedigreeID == CurrentDog.PedigreeID));

                    Remove(CurrentDog);
                    CurrentDog = null;
                    return true;

                case false:
                    return false;
            }
        }

        //CollectionHandling
        public void Add<T>(T obj)
        {
            switch (obj)
            {
                case Dog:
                    TblDogs.Add(obj as Dog);
                    DogCount++;
                    break;
                case Health:
                    TblDogHealth.Add(obj as Health);
                    break;
                case Pedigree:
                    TblDogPedigree.Add(obj as Pedigree);
                    break;
            }
        }
        public void Remove<T>(T obj)
        {
            switch (obj)
            {
                case Dog:
                    TblDogs.Remove(obj as Dog);
                    DogCount--;
                    break;
                case Health:
                    TblDogHealth.Remove(obj as Health);
                    break;
                case Pedigree:
                    TblDogPedigree.Remove(obj as Pedigree);
                    break;
            }
        }
        public bool ObjectBuilder<T>(string[] objArray)
        {
            return true;
        }


        //Miscellaneous
        //Test database connection
        public bool TestConnection()
        {
            switch (da.CommandTestConnection())
            {
                case true:
                    return true;
                case false:
                    return false;
            }
        }

        #region region NotificationHandling
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
