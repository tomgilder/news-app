using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NewsApp.Core
{
    public static class RssParser
    {
        static readonly XNamespace _namespace = "http://search.yahoo.com/mrss/";

        public static ReadOnlyCollection<FeedItem> ParseFeed(Category[] categories, Source source, string xml)
        {
            try
            {
                return XDocument.Parse(xml)
                                .Descendants("item")
                                .Select(item => Parse(categories, source, item))
                                .ToList()
                                .AsReadOnly();
            }
            catch
            {
                return new List<FeedItem>().AsReadOnly();
            }
        }

        static FeedItem Parse(Category[] categories, Source source, XElement item)
        {
            var title = item.Element("title")?.Value;
            var link = item.Element("link")?.Value;

            var description = StripHtml(item.Element("description")?.Value);

            var date = item.Element("pubDate")?.Value;
            var pubDate = date == null ? DateTimeOffset.MinValue : DateTimeOffset.Parse(date);

            var thumbnail = item.Element(_namespace + "thumbnail")?.Attribute("url")?.Value;

            return new FeedItem(categories, source, title, link, description, pubDate, thumbnail);
        }

        static string StripHtml(string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            return Regex.Replace(str, "<.*?>", "").Trim();
        }
    }
}
