using DogKennel.Model;
using DogKennel.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace DogKennel.View
{
    public partial class ViewDog : Window
    {
        List<string>? pedigreeList;

        public ViewDog(ViewModel _viewModel)
        {
            DataContext = _viewModel;

            //Find matching properties for other collections from _viewModel.CurrentDog
            Health currentDogHealth = _viewModel.TblDogHealth.ToList().First(pedigreeID => pedigreeID.PedigreeID == _viewModel.CurrentDog.PedigreeID);
            Pedigree currentDogPedigreee = _viewModel.TblDogPedigree.ToList().First(pedigreeID => pedigreeID.PedigreeID == _viewModel.CurrentDog.PedigreeID);

            //Initialize ListViews by initializing window
            InitializeComponent();

            //Define XAML listviews containing all properties and their values
            ListViewCreator(lstDogProperties, lstDogValues, new Dog().GetType().GetProperties(), _viewModel.CurrentDog);
            ListViewCreator(lstHealthProperties, lstHealthValues, new Health().GetType().GetProperties(), currentDogHealth);
            ListViewCreator(lstPedigreeProperties, lstPedigreeValues, new Pedigree().GetType().GetProperties(), currentDogPedigreee);

            //Define list for finding the number of offspring
            pedigreeList = new List<string>();
            foreach (Pedigree pedigree in _viewModel.TblDogPedigree)
            {
                //Add offspring if PedigreeID matches either father or mother
                if (_viewModel.CurrentDog.PedigreeID == pedigree.Father ||
                    _viewModel.CurrentDog.PedigreeID == pedigree.Mother)
                {
                    pedigreeList.Add(pedigree.PedigreeID);

                }
            }

            //Populate offspring Listview
            foreach (string offspring in pedigreeList)
            {
                if (offspring != null)
                {
                    lstChildren.Items.Add(offspring);
                }
            }
        }

        //Disable selection for all listviews
        private void NullClick_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstDogProperties.SelectedItem = null;
            lstDogValues.SelectedItem = null;
            lstHealthProperties.SelectedItem = null;
            lstHealthValues.SelectedItem = null;
            lstPedigreeProperties.SelectedItem = null;
            lstPedigreeValues.SelectedItem = null;
            lstChildren.SelectedItem = null;
        }

        //Define closing of window
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
        
        //Generic method for populating multiple listviews with property names and values
        private void ListViewCreator<T>(ListView listViewProperties, ListBox listViewValues, PropertyInfo[] properties, T obj)
        {
            //Loop through object properties
            foreach (PropertyInfo property in properties)
            {
                //Define string name from specific type
                string propertyName = property.Name.Split(' ')[0];

                //Define string value from specific property
                string propertyValue = property.GetValue(obj).ToString();

                //Switch for handling cases to singularly convert certain properties
                switch (propertyName)
                {
                    case "PedigreeID":
                        break;
                    case "Picture":
                        break;
                    case "DateOfBirth":
                        propertyName = "Fødselsdato";
                        listViewProperties.Items.Add(propertyName);

                        DateOfBirthConverter dateOfBirthConverter = new DateOfBirthConverter();
                        listViewValues.Items.Add(dateOfBirthConverter.Convert(propertyValue, typeof(string), null, CultureInfo.InvariantCulture));
                        break;
                    case "Alive":
                        propertyName = "Lever";
                        listViewProperties.Items.Add(propertyName);

                        AliveConverter aliveConverter = new AliveConverter();
                        listViewValues.Items.Add(aliveConverter.Convert(propertyValue, typeof(string), null, CultureInfo.InvariantCulture)); ;
                        break;
                    case "Sex":
                        propertyName = "Køn";
                        listViewProperties.Items.Add(propertyName);
                        listViewValues.Items.Add(propertyValue);
                        break;
                    case "Colour":
                        propertyName = "Farve";
                        listViewProperties.Items.Add(propertyName);
                        listViewValues.Items.Add(propertyValue);
                        break;
                    case "Owner":
                        propertyName = "Ejer";
                        listViewProperties.Items.Add(propertyName);
                        listViewValues.Items.Add(propertyValue);
                        break;
                    case "Father":
                        propertyName = "Far";
                        listViewProperties.Items.Add(propertyName);
                        listViewValues.Items.Add(propertyValue);
                        break;
                    case "Mother":
                        propertyName = "Mor";
                        listViewProperties.Items.Add(propertyName);
                        listViewValues.Items.Add(propertyValue);
                        break;
                    case "BreedStatus":
                        propertyName = "Avlsstatus";
                        listViewProperties.Items.Add(propertyName);
                        listViewValues.Items.Add(propertyValue);
                        break;
                    default:
                        listViewProperties.Items.Add(propertyName);
                        listViewValues.Items.Add(propertyValue);
                        break;

                }
            }
        }
    }
}
