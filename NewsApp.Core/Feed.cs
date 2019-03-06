using System;

namespace NewsApp.Core
{
    public class Feed
    {
        public Feed(string uri, Source source, Category[] categories)
        {
            Uri = uri ?? string.Empty;
            Source = source;
            Categories = categories ?? new Category[0];
        }

        public string Uri { get; }
        public Source Source { get; }
        public Category[] Categories { get; }
    }
}
