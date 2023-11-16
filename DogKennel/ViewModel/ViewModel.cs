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

        //SingularDataHandling
        public bool Insert(string[] stringArray)
        {
            //Try database insertion
            if (!da.CommandInsert(stringArray)) { return false; };

            //Select from database and insert into local collections
            foreach (Enum e in enums)
            {
                DataTable dt;

                //If selection fails returns false
                if (!da.CommandSelectAllBuilder(e, out dt)) { return false; }


                foreach (DataRow dataRow in dt.Rows)
                {
                    object[] convertedRow = dataRow.ItemArray;

                    //Populate collections along with type conversion from SQL to C#
                    switch (e.GetType().Name)
                    {
                        case "TblDogs":
                            Dog dog = DataAccess.DogConstructor(convertedRow);

                            if (!CheckDuplicate(e, dog))
                            {
                                Add(dog);
                            }
                            break;
                        case "TblDogHealth":
                            Health health = DataAccess.HealthConstructor(convertedRow);
                            if (!CheckDuplicate(e, health))
                            {
                                Add(health);
                            }
                            break;
                        case "TblDogPedigree":
                            Pedigree pedigree = DataAccess.PedigreeConstructor(convertedRow);
                            if (!CheckDuplicate(e, pedigree))
                            {
                                Add(pedigree);
                            }
                            break;
                    }
                }
            }
            return true;
        }
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
                if (!da.CommandSelectAllBuilder(e, out readTable)) { return false; }

                foreach (DataRow dataRow in readTable.Rows)
                {
                    object[] convertedRow = dataRow.ItemArray;

                    //Populate collections along with type conversion from SQL to C#
                    switch (e.GetType().Name)
                    {
                        case "TblDogs":
                            Dog dog = DataAccess.DogConstructor(convertedRow);

                            if (!CheckDuplicate(e, dog))
                            {
                                Add(dog);
                            }
                            break;
                        case "TblDogHealth":
                            Health health = DataAccess.HealthConstructor(convertedRow);
                            if (!CheckDuplicate(e, health))
                            {
                                Add(health);
                            }
                            break;
                        case "TblDogPedigree":
                            Pedigree pedigree = DataAccess.PedigreeConstructor(convertedRow);
                            if (!CheckDuplicate(e, pedigree))
                            {
                                Add(pedigree);
                            }
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
            //Loop backwards to account for lowest reference
            for (int i = enums.Count - 1; i >= 0; i--)
            {
                //Try database truncation
                if (!da.CommandTruncateBuilder(enums[i])) { return false; }

                //Truncate local collection
                switch (enums[i].GetType().Name)
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
            return true;
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
        public bool CheckDuplicate<T>(Enum e, T obj)
        {
            //Checks for duplicate by PedigreeID
            switch (e.GetType().Name)
            {
                case "TblDogs":
                    Dog dog = obj as Dog;

                    for (int i = 0; i < TblDogs.Count; i++)
                    {
                        if (TblDogs[i].ToString() == dog.PedigreeID)
                        {
                            return true;
                        };
                    }
                    break;
                case "TblDogHealth":
                    Health health = obj as Health;

                    for (int i = 0; i < TblDogHealth.Count; i++)
                    {
                        if (TblDogHealth[i].ToString() == health.PedigreeID)
                        {
                            return true;
                        };
                    }
                    break;
                case "TblDogPedigree":
                    Pedigree pedigree = obj as Pedigree;

                    for (int i = 0; i < TblDogPedigree.Count; i++)
                    {
                        if (TblDogPedigree[i].ToString() == pedigree.PedigreeID)
                        {
                            return true;
                        };
                    }
                    break;
            }
            return false;
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
        public Dog BlankDog()
        {
            return new Dog();
        }
        public Health BlankHealth()
        {
            return new Health();
        }
        public Pedigree BlankPedigree()
        {
            return new Pedigree();
        }
        public Health CurrentHealth()
        {
            return TblDogHealth.ToList().First(health => health.PedigreeID == CurrentDog.PedigreeID);
        }
        public Pedigree CurrentPedigree()
        {
            return TblDogPedigree.ToList().First(pedigree => pedigree.PedigreeID == CurrentDog.PedigreeID);
        }
        public List<string> GetOffspring()
        {
            List<string> offsprings = new List<string>();
            foreach(Pedigree pedigree in TblDogPedigree)
            {
                if (CurrentDog.PedigreeID == pedigree.Father || CurrentDog.PedigreeID == pedigree.Mother)
                {
                    offsprings.Add(pedigree.PedigreeID);
                }
            }

            //Returns list of offspring for CurrentDog
            return offsprings;
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
