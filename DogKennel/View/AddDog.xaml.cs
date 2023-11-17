using DogKennel.ViewModels;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace DogKennel.View
{
    public partial class AddDog : Window
    {
        ViewModel _viewModel;
        string[] strings = new string[19];

        public AddDog(ViewModel _viewModelPassed)
        {
            //Bind datacontext
            _viewModel = _viewModelPassed;
            DataContext = _viewModel;

            //Initialize ListViews by initializing window
            InitializeComponent();

            //Define XAML listviews containing all properties and their values
            ListViewCreator(lstDogProp, _viewModel.BlankDog().GetType().GetProperties());
            ListViewCreator(lstHealthProp, _viewModel.BlankHealth().GetType().GetProperties());
            ListViewCreator(lstPedigreeProp, _viewModel.BlankPedigree().GetType().GetProperties());
        }

        //Generic method for populating multiple listviews with property names and values
        private void ListViewCreator(ListView lstProp, PropertyInfo[] properties)
        {
            foreach (PropertyInfo property in properties)
            {
                //Define string name from specific type
                string propName = property.Name.Split(' ')[0];

                //Switch for handling cases to singularly convert certain properties
                switch (propName)
                {
                    case "PedigreeID":
                        break;
                    case "Picture":
                        break;
                    case "DateOfBirth":
                        lstProp.Items.Add("Fødselsdato");
                        break;
                    case "Alive":
                        lstProp.Items.Add("Lever");
                        break;
                    case "Sex":
                        lstProp.Items.Add("Køn");
                        break;
                    case "Colour":
                        lstProp.Items.Add("Farve");
                        break;
                    case "Owner":
                        lstProp.Items.Add("Ejer");
                        break;
                    case "Father":
                        lstProp.Items.Add("Far");
                        break;
                    case "Mother":
                        lstProp.Items.Add("Mor");
                        break;
                    case "BreedStatus":
                        lstProp.Items.Add("Avlsstatus");
                        break;
                    default:
                        lstProp.Items.Add(propName);
                        break;
                }
            }
        }

        //Disable selection for all listviews
        private void NullClick_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstDogProp.SelectedItem = null;
            lstHealthProp.SelectedItem = null;
            lstPedigreeProp.SelectedItem = null;
        }

        //Define closing of window
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        //DialogbuttonOK
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int tempDogCount = _viewModel.DogCount;

            if (!_viewModel.Insert(strings) || _viewModel.DogCount == tempDogCount)
            {
                MessageBox.Show($"Hunden kunne ikke indsættes. Tjek om stambogsID allerede\neksisterer i databasen eller om værdierne er indtastet korrekt.", "Tilføj hund");
            }
            DialogResult = true;
        }

        //DialogbuttonCancel
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ID_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[0] = ID.Text;
        }

        private void DateOfBirth_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[1] = DateOfBirth.Text;
        }

        private void IsAlive_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[2] = IsAlive.Text;
        }

        private void Sex_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[3] = Sex.Text;
        }

        private void Colour_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[4] = Colour.Text;
        }

        private void AK_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[5] = AK.Text;
        }

        private void BreedStatus_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[6] = BreedStatus.Text;
        }

        private void DKTitles_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[7] = DKTitles.Text;
        }

        private void Titles_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[8] = Titles.Text;
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[9] = Name.Text;
        }

        private void HD_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[10] = HD.Text;
        }

        private void AD_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[11] = AD.Text;
        }

        private void HZ_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[12] = HZ.Text;
        }

        private void SP_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[13] = SP.Text;
        }

        private void Father_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[14] = Father.Text;
        }

        private void Mother_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[15] = Mother.Text;
        }

        private void TattooNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[16] = TattooNo.Text;
        }

        private void Owner_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings[17] = Owner.Text;
        }
    }
}
