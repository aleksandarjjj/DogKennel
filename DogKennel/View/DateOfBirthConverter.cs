using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Navigation;

namespace DogKennel.View
{
    public class DateOfBirthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateValue;

            if(DateTime.TryParse((string) value, out dateValue))
            {
                return dateValue.ToString("yyyy/MM/dd");
            }
            return "";
            /*
            switch (stringValue.Length)
            {
                case > 1:
                    return stringValue.Split(' ').First();
                default:
                    return "";
            }

            string stringValue = (string)value;

            switch (stringValue.Length)
            {
                case > 1:
                    return stringValue.Split(' ').First();
                default:
                    return "";
            }*/
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}