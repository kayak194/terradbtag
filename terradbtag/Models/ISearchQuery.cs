using System.Collections.ObjectModel;

namespace terradbtag.Models
{
    public interface ISearchQuery
    {
        string TypeFilter { get; }
        ObservableCollection<Tag> SelectedTags { get; }
        string TextQuery { get; }
        int TagLimit { get; }
        int ResultLimit { get; }
    }
}