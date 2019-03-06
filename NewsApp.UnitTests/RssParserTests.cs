using System;
using NewsApp.Core;
using System.Threading.Tasks;
using Xunit;
using System.Xml.Linq;

namespace NewsApp.Tests
{
    public class RssParserTests
    {
        [Fact]
        public void ParsesValidFeed()
        {
            var categories = new Category[] { Category.Technology };
            var source = Source.BBC;

            var result = RssParser.ParseFeed(categories, source, _validFeedXml);

            Assert.Equal(2, result.Count);

            Assert.Equal("Title", result[0].Title);
            Assert.Equal("Description", result[0].Description);
            Assert.Equal("https://link", result[0].Link);
            Assert.Equal(new DateTimeOffset(2019, 2, 26, 16, 10, 37, TimeSpan.Zero), result[0].PubDate);
            Assert.Equal("http://thumbnail", result[0].Thumbnail);
            Assert.Equal(categories, result[0].Categories);
            Assert.Equal(source, result[0].Source);
        }

        [Fact]
        public void InvalidXmlReturnsEmptyList()
        {
            var result = RssParser.ParseFeed(new[] { Category.Technology }, Source.BBC, ">invalid xml<");
            Assert.Empty(result);
        }


        static readonly string _validFeedXml = @"<?xml version='1.0' encoding='UTF-8'?>
<rss xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:content='http://purl.org/rss/1.0/modules/content/' xmlns:atom='http://www.w3.org/2005/Atom' version='2.0' xmlns:media='http://search.yahoo.com/mrss/'>
    <channel>
        <item>
            <title><![CDATA[Title]]></title>
            <description><![CDATA[Description]]></description>
            <link>https://link</link>
            <pubDate>Tue, 26 Feb 2019 16:10:37 GMT</pubDate>
            <media:thumbnail width='976' height='549' url='http://thumbnail'/>
        </item>
        <item>
            <title>Title2</title>
            <description>Description2</description>
            <link>https://link2</link>
            <pubDate>Tue, 26 Feb 2019 16:12:20 GMT</pubDate>
            <media:thumbnail width = '976' height='549' url='http://c.files.bbci.co.uk/12E26/production/_105805377_a1fb9184-0263-48f2-b97a-09043a868d6f.jpg'/>
        </item>
    </channel>
</rss>";


    }

}
