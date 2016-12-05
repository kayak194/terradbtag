using System.ComponentModel;
using System.Runtime.CompilerServices;
using WordCloudCalculator.Contract.Word;

namespace terradbtag.Models
{
    public class Tag : INotifyPropertyChanged, IWeightedWord
    {
        private string _text;
	    private double _weight;

	    public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged();}
        }

		public double Weight
		{
			get { return _weight; }
			set { _weight = value; OnPropertyChanged(); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
