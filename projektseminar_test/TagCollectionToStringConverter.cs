using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace projektseminar_test
{
    class TagCollectionToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tags = value as List<Tag>;
            return tags?.Aggregate("", (current, tag) => current + $"#{tag.Content} ");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return (from Match match in Regex.Matches(value.ToString(), "[#](\\w+)") select new Tag() {Content = match.Groups[1].Value}).ToList();
        }
    }
}
