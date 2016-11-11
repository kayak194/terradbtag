using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace projektseminar_test
{
    public class TagCollectionEditorViewModel : INotifyPropertyChanged
    {
        private Tag _selectedTag;
        private string _tagContent;
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();

        public ICommand RemoveCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ClearSelectionCommand { get; set; }

        public TagCollectionEditorViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSaveCommand/*, CanExecuteSaveCommand*/);
            RemoveCommand = new RelayCommand(ExecuteRemoveCommand/*, CanExecuteRemoveCommand*/);
            ClearSelectionCommand = new RelayCommand(ExecuteClearSelectionCommand);
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedTag) && SelectedTag != null)
                {
                    TagContent = SelectedTag.Content;
                }
            };
        }

       /* private bool CanExecuteRemoveCommand(object o)
        {
            return SelectedTag != null;
        }*/

        private void ExecuteRemoveCommand(object o)
        {
            Tags.Remove(SelectedTag);
        }

        /*private bool CanExecuteSaveCommand(object o)
        {
            return !string.IsNullOrEmpty(TagContent);
        }*/

        private void ExecuteSaveCommand(object o)
        {
            if (SelectedTag == null)
            {
                Tags.Add(new Tag { Content = TagContent });
            }
            else
            {
                SelectedTag.Content = TagContent;
            }
            OnPropertyChanged(nameof(Tags));
        }

        private void ExecuteClearSelectionCommand(object o)
        {
            SelectedTag = null;
        }

        public Tag SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                _selectedTag = value;
                OnPropertyChanged();
            }
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            Tags.Clear();
            foreach (var tag in tags)
            {
                Tags.Add(tag);
            }
        }

        public string TagContent
        {
            get { return _tagContent; }
            set { _tagContent = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
