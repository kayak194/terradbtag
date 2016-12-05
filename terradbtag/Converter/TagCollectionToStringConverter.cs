using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using terradbtag.Models;

namespace terradbtag.Converter
{
    class TagCollectionToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tags = value as List<Tag>;
            return tags?.Aggregate("", (current, tag) => current + $"#{tag.Text} ");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return (from Match match in Regex.Matches(value.ToString(), "[#](\\w+)") select new Tag() {Text = match.Groups[1].Value}).ToList();
        }
    }
}
