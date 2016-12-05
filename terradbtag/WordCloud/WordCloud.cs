using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using terradbtag.Models;
using WordCloudCalculator.Contract.Word;

namespace terradbtag.WordCloud
{
	public class WordCloud : Control
	{
		public static readonly DependencyProperty TagsProperty = DependencyProperty.Register("Tags", typeof(ObservableCollection<IWeightedWord>), typeof(WordCloud), new PropertyMetadata 
		{
			DefaultValue = new ObservableCollection<IWeightedWord>()
			{
				new Tag() {Text = "Foo", Weight = 100},
				new Tag() {Text = "Foo2", Weight = 80},
				new Tag() {Text = "Foo3", Weight = 70},
				new Tag() {Text = "Foo4", Weight = 40},
				new Tag() {Text = "Foo5", Weight = 40},
			}
		});

		public ObservableCollection<IWeightedWord> Tags
		{
			get { return GetValue(TagsProperty) as ObservableCollection<IWeightedWord>;}
			set { SetValue(TagsProperty, value);}
		}
	}
}
