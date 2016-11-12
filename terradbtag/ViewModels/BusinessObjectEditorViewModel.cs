using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using projektseminar_test.Framework;
using projektseminar_test.Models;

namespace projektseminar_test.ViewModels
{
    class BusinessObjectEditorViewModel : INotifyPropertyChanged
    {
        public BusinessObject BusinessObject { get; set; }

        public BusinessObjectEditorViewModel()
        {
            EditTagsCommand = new RelayCommand(ExecuteEditTagsCommand);
        }

        private void ExecuteEditTagsCommand(object o)
        {
            var editor = new Views.TagCollectionEditor();
            ((TagCollectionEditorViewModel)editor.DataContext).SetTags(Tags);
            editor.ShowDialog();
            Tags.Clear();
            Tags.AddRange(((TagCollectionEditorViewModel)editor.DataContext).Tags);
            OnPropertyChanged("Tags");
        }

        public string Name
        {
            get { return BusinessObject.Name; }
            set { BusinessObject.Name = value; OnPropertyChanged(); }
        }

        public string Id
        {
            get { return BusinessObject.Id; }
            set { BusinessObject.Id = value; OnPropertyChanged(); }
        }

        public string Data
        {
            get { return BusinessObject.Data; }
            set { BusinessObject.Data = value; OnPropertyChanged(); }
        }

        public List<Tag> Tags
        {
            get { return BusinessObject.Tags; }
            set { BusinessObject.Tags = value; OnPropertyChanged(); }
        }

        public ICommand EditTagsCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
