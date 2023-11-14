using System;
using System.Globalization;
using System.Windows.Data;

namespace DogKennel.View
{
    public class AliveConverter : IValueConverter
    {
        //Converts string value from number to letter (status as alive or dead)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "0":
                    return "N";
                case "1":
                    return "J";
            }
            return "";
        }

        //Reduntant interface method
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
