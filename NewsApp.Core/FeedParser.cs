using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace NewsApp.Core
{
    public class FeedParser
    {
        readonly IEnumerable<Feed> _feeds;
        readonly Subject<IEnumerable<FeedItem>> _subject = new Subject<IEnumerable<FeedItem>>();
        readonly HttpClient _client = new HttpClient();

        public IObservable<IEnumerable<FeedItem>> OnData => _subject.AsObservable();

        public FeedParser(IEnumerable<Feed> feeds)
        {
            _feeds = feeds;
        }

        public void Update()
        {
            Task.Run(UpdateAsync);
        }

        public Task UpdateAsync()
        {
            return Task.WhenAll(_feeds.Select(UpdateFeedAsync));
        }

        async Task UpdateFeedAsync(Feed feed)
        {
            var rssItems = await GetFeed(feed);
            _subject.OnNext(rssItems);
        }

        async Task<ReadOnlyCollection<FeedItem>> GetFeed(Feed feed)
        {
            try
            {
                var xml = await _client.GetStringAsync(feed.Uri);
                return RssParser.ParseFeed(feed.Categories, feed.Source, xml);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Web request failed: {ex}");
                return new List<FeedItem>().AsReadOnly();
            }
        }
    }
}
