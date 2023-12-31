﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace DogKennel.View
{
    public class DateOfBirthConverter : IValueConverter
    {
        //Shorten string by parsing as datetime and converting back to polish data
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateValue;

            if (DateTime.TryParse((string)value, out dateValue))
            {
                return dateValue.ToString("yyyy/MM/dd");
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