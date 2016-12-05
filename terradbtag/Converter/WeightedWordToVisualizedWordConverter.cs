using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WordCloudCalculator.Contract.Visualization;
using WordCloudCalculator.Contract.Word;
using WordCloudCalculator.ExtractingWordCloudCalculator;
using WordCloudCalculator.WordCloudCalculator;

namespace terradbtag.Converter
{
	class WeightedWordToVisualizedWordConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var tags = values[0] as IEnumerable<IWeightedWord>;
			var height = System.Convert.ToDouble(values[1]);
			var width = System.Convert.ToDouble(values[2]);
			var visArgs = parameter as IWordCloudAppearenceArguments;
			
			if (visArgs != null && height > 0 && width > 0)
			{
				visArgs.PanelSize = new WordCloudCalculator.Contract.Visualization.Size(width, height);

				var calculator = new ExtractingWordCloudCalculator<SimpleAppearenceCalculationMethod>();
				var result = calculator.Calculate(visArgs, tags);
				return result;
			}
			return null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
