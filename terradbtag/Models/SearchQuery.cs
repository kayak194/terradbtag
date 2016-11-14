﻿using System.Collections.ObjectModel;

namespace terradbtag.Models
{
    public class SearchQuery : ISearchQuery
    {
        public SearchQuery(string typeFilter = "", string textQuery = "", params Tag[] tags)
        {
            TypeFiler = typeFilter;
            TextQuery = textQuery;
            foreach (var tag in tags)
            {
                SelectedTags.Add(tag);
            }
        }
        public static readonly ISearchQuery NoFilter = new SearchQuery();
        public string TypeFiler { get; set; }
        public ObservableCollection<Tag> SelectedTags { get; set; } = new ObservableCollection<Tag>();
        public string TextQuery { get; set; }
        public int TagLimit { get; } = 20;
        public int ResultLimit { get; } = 30;
    }
}