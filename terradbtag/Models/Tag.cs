using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace terradbtag.Models
{
    public class Tag : INotifyPropertyChanged
    {
        private string _content;

        public string Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged();}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
