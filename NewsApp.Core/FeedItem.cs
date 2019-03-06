using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NewsApp.Core
{
    public class FeedItem
    {
        public FeedItem(Category[] categories, Source source, string title, string link, string description, DateTimeOffset pubDate, string thumbnail)
        {
            Categories = categories;
            Source = source;
            Title = title;
            Link = link;
            Description = description;
            PubDate = pubDate;
            Thumbnail = thumbnail;
        }

        public string Title { get; }

        public string Description { get; }
        public string Link { get; }

        public DateTimeOffset PubDate { get; }
        public string Thumbnail { get; }

        public Category[] Categories { get; }
        public Source Source { get; }

        public bool ShowImage => Thumbnail != null;
        public float ImageColumnSize => ShowImage ? 100 : 0;
   }
}
