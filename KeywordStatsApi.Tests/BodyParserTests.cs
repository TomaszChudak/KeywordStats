using System;
using FluentAssertions;
using HtmlAgilityPack;
using KeywordStatsApi.Services.Implementation;
using KeywordStatsApi.Services.Interface;
using Xunit;

namespace KeywordStatsApi.Tests
{
    public class BodyParserTests
    {
        private readonly IBodyParser _sut;

        public BodyParserTests()
        {
            _sut = new BodyParser();
        }

        [Fact]
        public void GetBodyNode_NullDocument_ThrowsException()
        {
            Action act = () => _sut.GetBodyNode(null);

            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("<html><head></head></html>")]
        public void GetBodyNode_DocumentWithoutBody_ThrowsException(string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            Action act = () => _sut.GetBodyNode(htmlDocument);

            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("<html><head></head><body></body></html>")]
        [InlineData("<html><head></head><body></html>")]
        public void GetBodyNode_DocumentWithBody_ReturnsBodyNode(string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var result = _sut.GetBodyNode(htmlDocument);

            result.Name.ToLowerInvariant().Should().Be("body");
        }

        [Fact]
        public void CountKeywordOccurences_NullBodyNode_ThrowsException()
        {

            Action act = () => _sut.CountKeywordOccurrences(null, "abc");

            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("")]
        public void CountKeywordOccurrences_NullDocument_ThrowsException(string html)
        {
            var bodyNode = HtmlNode.CreateNode(html);

            Action act = () => _sut.CountKeywordOccurrences(bodyNode, "abc");

            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("   ", "abc")]
        [InlineData("aaaaaaa", "abc")]
        [InlineData("<body></body>", "abc")]
        [InlineData("<body><div></div></body>", "abc")]
        public void CountKeywordOccurrences_NotExistingInBodyKeyword_ReturnsZero(string html, string keyword)
        {
            var bodyNode = HtmlNode.CreateNode(html);

            var result = _sut.CountKeywordOccurrences(bodyNode, keyword);

            result.Should().Be(0);
        }

        [Theory]
        [InlineData("<body>abc</body>", "abc")]
        [InlineData("<body><div>def</div>abc</body>", "abc")]
        [InlineData("<body><div><span>abc</span></div></body>", "abc")]
        public void CountKeywordOccurrences_OneOccurence_ReturnsOne(string html, string keyword)
        {
            var bodyNode = HtmlNode.CreateNode(html);

            var result = _sut.CountKeywordOccurrences(bodyNode, keyword);

            result.Should().Be(1);
        }

        [Theory]
        [InlineData("body>abc def abc</body></html>", "abc", 2)]
        [InlineData("<html><head></head><body>abc www bbb abc <tag></tag> abc </body></html>", "abc", 3)]
        [InlineData("<html><head></head><body>abc www bbb abc <tag></tag> abc <div>abc abc</div> </body></html>", "abc", 5)]
        public void CountKeywordOccurrences_WFewOccurrences_ReturnsExpectedCount(string html, string keyword, int expectedCount)
        {
            var bodyNode = HtmlNode.CreateNode(html);

            var result = _sut.CountKeywordOccurrences(bodyNode, keyword);

            result.Should().Be(expectedCount);
        }
    }
}