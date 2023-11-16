using DogKennel.ViewModels;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace DogKennel.View
{
    public partial class ViewDog : Window
    {
        public ViewDog(ViewModel _viewModel)
        {
            //Bind datacontext
            DataContext = _viewModel;

            //Initialize ListViews by initializing window
            InitializeComponent();

            //Define XAML listviews containing all properties and their values
            ListViewCreator(lstDogProp, lstDogVal, _viewModel.BlankDog().GetType().GetProperties(), _viewModel.CurrentDog);
            ListViewCreator(lstHealthProp, lstHealthVal, _viewModel.BlankHealth().GetType().GetProperties(), _viewModel.CurrentHealth());
            ListViewCreator(lstPedigreeProp, lstPedigreeVal, _viewModel.BlankPedigree().GetType().GetProperties(), _viewModel.CurrentPedigree());

            //Define XAML listview for finding the number of offspring
            foreach (string offspring in _viewModel.GetOffspring())
            {
                lstChildren.Items.Add(offspring);
            }
        }

        //Generic method for populating multiple listviews with property names and values
        private void ListViewCreator<T>(ListView lstProp, ListBox lstVal, PropertyInfo[] properties, T obj)
        {
            foreach (PropertyInfo property in properties)
            {
                //Define string name from specific type
                string propName = property.Name.Split(' ')[0];

                //Define string value from specific property
                string propVal = property.GetValue(obj).ToString();

                //Switch for handling cases to singularly convert certain properties
                switch (propName)
                {
                    case "PedigreeID":
                        break;
                    case "Picture":
                        break;
                    case "DateOfBirth":
                        lstProp.Items.Add("Fødselsdato");

                        DateOfBirthConverter dateOfBirthConverter = new DateOfBirthConverter();
                        lstVal.Items.Add(dateOfBirthConverter.Convert(propVal, typeof(string), null, CultureInfo.InvariantCulture));
                        break;
                    case "Alive":
                        lstProp.Items.Add("Lever");

                        AliveConverter aliveConverter = new AliveConverter();
                        lstVal.Items.Add(aliveConverter.Convert(propVal, typeof(string), null, CultureInfo.InvariantCulture)); ;
                        break;
                    case "Sex":
                        lstProp.Items.Add("Køn");
                        lstVal.Items.Add(propVal);
                        break;
                    case "Colour":
                        lstProp.Items.Add("Farve");
                        lstVal.Items.Add(propVal);
                        break;
                    case "Owner":
                        lstProp.Items.Add("Ejer");
                        lstVal.Items.Add(propVal);
                        break;
                    case "Father":
                        lstProp.Items.Add("Far");
                        lstVal.Items.Add(propVal);
                        break;
                    case "Mother":
                        lstProp.Items.Add("Mor");
                        lstVal.Items.Add(propVal);
                        break;
                    case "BreedStatus":
                        lstProp.Items.Add("Avlsstatus");
                        lstVal.Items.Add(propVal);
                        break;
                    default:
                        lstProp.Items.Add(propName);
                        lstVal.Items.Add(propVal);
                        break;
                }
            }
        }

        //Disable selection for all listviews
        private void NullClick_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstDogProp.SelectedItem = null;
            lstDogVal.SelectedItem = null;
            lstHealthProp.SelectedItem = null;
            lstHealthVal.SelectedItem = null;
            lstPedigreeProp.SelectedItem = null;
            lstPedigreeVal.SelectedItem = null;
            lstChildren.SelectedItem = null;
        }

        //Define closing of window
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
