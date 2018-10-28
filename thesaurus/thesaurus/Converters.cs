using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace thesaurus
{
    public class FullNameToShortName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string) value;
            var splits = name.Split('.');

            if (splits.Length > 2) return splits[splits.Length - 2] + "." + splits[splits.Length - 1];
            return splits.Last();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
