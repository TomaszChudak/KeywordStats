using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using HtmlAgilityPack;
using KeywordStatsApi.Services.Implementation;
using KeywordStatsApi.Services.Interface;
using Moq;
using Xunit;

namespace KeywordStatsApi.Tests
{
    public class HtmlParserTests
    {
        private readonly Mock<IHtmlDocumentFactory> _htmlDocumentFactoryMock;
        private readonly Mock<IMetaKeywordsTagParser> _metaKeywordsTagParserMock;
        private readonly Mock<IBodyParser> _bodyParserMock;
        private readonly Mock<IHtmlDocument> _htmlDocumentMock;
        private readonly IHtmlParser _sut;

        public HtmlParserTests()
        {
            _htmlDocumentFactoryMock = new Mock<IHtmlDocumentFactory>(MockBehavior.Strict);
            _metaKeywordsTagParserMock = new Mock<IMetaKeywordsTagParser>(MockBehavior.Strict);
            _bodyParserMock = new Mock<IBodyParser>(MockBehavior.Strict);
            _htmlDocumentMock = new Mock<IHtmlDocument>(MockBehavior.Strict);
            _sut = new HtmlParser(_htmlDocumentFactoryMock.Object, _metaKeywordsTagParserMock.Object, _bodyParserMock.Object);
        }

        [Fact]
        public void GetPageStats_NullHtml_ThrowException()
        {
            _htmlDocumentFactoryMock.Setup(x => x.Create(null))
                .Throws<ArgumentNullException>();

            Action act = () => _sut.GetPageStats(null);

            act.Should().Throw<ArgumentNullException>();

            _htmlDocumentFactoryMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
            _htmlDocumentFactoryMock.Verify(x => x.Create(null), Times.Once);
            _metaKeywordsTagParserMock.Verify(x => x.GetMetaKeywordTag(It.IsAny<HtmlNode>()), Times.Never);
            _metaKeywordsTagParserMock.Verify(x => x.GetKeywordsFromMetaKeywordTag(It.IsAny<string>()), Times.Never);
            _bodyParserMock.Verify(x => x.GetBodyNode(It.IsAny<HtmlDocument>()), Times.Never);
            _bodyParserMock.Verify(x => x.CountKeywordOccurrences(It.IsAny<HtmlNode>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaa")]
        [InlineData("<html><head></head><body></body></html>")]
        [InlineData("<html><head></head><body>abc</body></html>")]
        public void GetPageStats_NotExistingKeyword_ThrowException(string html)
        {
            _htmlDocumentFactoryMock.Setup(x => x.Create(html))
                .Returns(_htmlDocumentMock.Object);
            _htmlDocumentMock.Setup(x => x.DocumentNode)
                .Returns(new HtmlNode(HtmlNodeType.Document, new HtmlDocument(), 0));
            _metaKeywordsTagParserMock.Setup(x => x.GetMetaKeywordTag(It.IsAny<HtmlNode>()))
                .Throws<ArgumentException>();

            Action act = () => _sut.GetPageStats(html);

            act.Should().Throw<ArgumentException>();

            _htmlDocumentFactoryMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
            _htmlDocumentFactoryMock.Verify(x => x.Create(html), Times.Once);
            _metaKeywordsTagParserMock.Verify(x => x.GetMetaKeywordTag(It.IsAny<HtmlNode>()), Times.Once);
            _metaKeywordsTagParserMock.Verify(x => x.GetKeywordsFromMetaKeywordTag(It.IsAny<string>()), Times.Never);
            _bodyParserMock.Verify(x => x.GetBodyNode(It.IsAny<HtmlDocument>()), Times.Never);
            _bodyParserMock.Verify(x => x.CountKeywordOccurrences(It.IsAny<HtmlNode>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("<html><head><meta name=\"keywords\" content=\"abc\"></head><body></body></html>")]
        public void GetPageStats_NoKeywords_ReturnsOneKeywordWithNoOccurrences(string html)
        {
            _htmlDocumentFactoryMock.Setup(x => x.Create(html))
                .Returns(_htmlDocumentMock.Object);
            _htmlDocumentMock.Setup(x => x.DocumentNode)
                .Returns(new HtmlNode(HtmlNodeType.Document, new HtmlDocument(), 0));
            _metaKeywordsTagParserMock.Setup(x => x.GetMetaKeywordTag(It.IsAny<HtmlNode>()))
                .Returns("<meta name=\"keywords\" content=\"abc\">");
            _metaKeywordsTagParserMock
                .Setup(x => x.GetKeywordsFromMetaKeywordTag("<meta name=\"keywords\" content=\"abc\">"))
                .Returns(new List<string>{"abc"});
            _bodyParserMock.Setup(x => x.CountKeywordOccurrences(It.IsAny<HtmlNode>(), "abc"))
                .Returns(0);

            var result = _sut.GetPageStats(html).ToList();

            result.Should().HaveCount(1);
            result.Single().Keyword.Should().Be("abc");
            result.Single().Count.Should().Be(0);

            _htmlDocumentFactoryMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
            _htmlDocumentFactoryMock.Verify(x => x.Create(html), Times.Once);
            _metaKeywordsTagParserMock.Verify(x => x.GetMetaKeywordTag(It.IsAny<HtmlNode>()), Times.Once);
            _metaKeywordsTagParserMock.Verify(x => x.GetKeywordsFromMetaKeywordTag(It.IsAny<string>()), Times.Once);
            _bodyParserMock.Verify(x => x.GetBodyNode(It.IsAny<HtmlDocument>()), Times.Never);
            _bodyParserMock.Verify(x => x.CountKeywordOccurrences(It.IsAny<HtmlNode>(), It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData("<html><head><meta name=\"keywords\" content=\"abc\"></head><body>abc</body></html>")]
        public void GetPageStats_OneKeywordOccurence_ReturnsOne(string html)
        {
            _htmlDocumentFactoryMock.Setup(x => x.Create(html))
                .Returns(_htmlDocumentMock.Object);
            _htmlDocumentMock.Setup(x => x.DocumentNode)
                .Returns(new HtmlNode(HtmlNodeType.Document, new HtmlDocument(), 0));
            _metaKeywordsTagParserMock.Setup(x => x.GetMetaKeywordTag(It.IsAny<HtmlNode>()))
                .Returns("<meta name=\"keywords\" content=\"abc\">");
            _metaKeywordsTagParserMock
                .Setup(x => x.GetKeywordsFromMetaKeywordTag("<meta name=\"keywords\" content=\"abc\">"))
                .Returns(new List<string>{"abc"});
            _bodyParserMock.Setup(x => x.CountKeywordOccurrences(It.IsAny<HtmlNode>(), "abc"))
                .Returns(1);

            var result = _sut.GetPageStats(html).ToList();

            result.Should().HaveCount(1);
            result.Single().Keyword.Should().Be("abc");
            result.Single().Count.Should().Be(1);

            _htmlDocumentFactoryMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
            _htmlDocumentFactoryMock.Verify(x => x.Create(html), Times.Once);
            _metaKeywordsTagParserMock.Verify(x => x.GetMetaKeywordTag(It.IsAny<HtmlNode>()), Times.Once);
            _metaKeywordsTagParserMock.Verify(x => x.GetKeywordsFromMetaKeywordTag(It.IsAny<string>()), Times.Once);
            _bodyParserMock.Verify(x => x.GetBodyNode(It.IsAny<HtmlDocument>()), Times.Never);
            _bodyParserMock.Verify(x => x.CountKeywordOccurrences(It.IsAny<HtmlNode>(), It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData("<html><head><meta name=\"keywords\" content=\"abc\"></head><body>abc def abc</body></html>", 2)]
        [InlineData("<html><head><meta name=\"keywords\" content=\"abc\"></head><body>abc www bbb abc <tag></tag> abc </body></html>", 3)]
        public void GetPageStats_WFewOccurrences_ReturnsExpectedCount(string html, int expectedCount)
        {
            _htmlDocumentFactoryMock.Setup(x => x.Create(html))
                .Returns(_htmlDocumentMock.Object);
            _htmlDocumentMock.Setup(x => x.DocumentNode)
                .Returns(new HtmlNode(HtmlNodeType.Document, new HtmlDocument(), 0));
            _metaKeywordsTagParserMock.Setup(x => x.GetMetaKeywordTag(It.IsAny<HtmlNode>()))
                .Returns("<meta name=\"keywords\" content=\"abc\">");
            _metaKeywordsTagParserMock
                .Setup(x => x.GetKeywordsFromMetaKeywordTag("<meta name=\"keywords\" content=\"abc\">"))
                .Returns(new List<string>{"abc"});
            _bodyParserMock.Setup(x => x.CountKeywordOccurrences(It.IsAny<HtmlNode>(), "abc"))
                .Returns(expectedCount);

            var result = _sut.GetPageStats(html).ToList();

            result.Should().HaveCount(1);
            result.Single().Keyword.Should().Be("abc");
            result.Single().Count.Should().Be(expectedCount);

            _htmlDocumentFactoryMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
            _htmlDocumentFactoryMock.Verify(x => x.Create(html), Times.Once);
            _metaKeywordsTagParserMock.Verify(x => x.GetMetaKeywordTag(It.IsAny<HtmlNode>()), Times.Once);
            _metaKeywordsTagParserMock.Verify(x => x.GetKeywordsFromMetaKeywordTag(It.IsAny<string>()), Times.Once);
            _bodyParserMock.Verify(x => x.GetBodyNode(It.IsAny<HtmlDocument>()), Times.Never);
            _bodyParserMock.Verify(x => x.CountKeywordOccurrences(It.IsAny<HtmlNode>(), It.IsAny<string>()), Times.Once);
        }
    }
}