using System.Collections.ObjectModel;

namespace terradbtag.Models
{
    public interface ISearchQuery
    {
        string TypeFiler { get; }
        ObservableCollection<Tag> SelectedTags { get; }
        string TextQuery { get; }
        int TagLimit { get; }
        int ResultLimit { get; }
    }
}