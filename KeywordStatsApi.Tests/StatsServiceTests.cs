using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using KeywordStatsApi.Models;
using KeywordStatsApi.Services.Implementation;
using KeywordStatsApi.Services.Interface;
using Moq;
using Xunit;

namespace KeywordStatsApi.Tests
{
    public class StatsServiceTests
    {
        public StatsServiceTests()
        {
            _webClientMock = new Mock<IWebClient>(MockBehavior.Strict);
            _htmlParserMock = new Mock<IHtmlParser>(MockBehavior.Strict);
            _sut = new StatsService(_webClientMock.Object, _htmlParserMock.Object);
        }

        private readonly Mock<IWebClient> _webClientMock;
        private readonly Mock<IHtmlParser> _htmlParserMock;
        private readonly IStatsService _sut;

        [Fact]
        public void GetPageStats_HtmlParserThrowsException_ThrowsException()
        {
            _webClientMock.Setup(x => x.DownloadString("http://abc.com"))
                .Returns("zzz");
            _htmlParserMock.Setup(x => x.GetPageStats("zzz"))
                .Throws<Exception>();

            Action act = () => _sut.GetPageStats("http://abc.com");

            act.Should().Throw<Exception>();

            _webClientMock.Verify(x => x.DownloadString(It.IsAny<string>()), Times.Once);
            _webClientMock.Verify(x => x.DownloadString("http://abc.com"), Times.Once);
            _htmlParserMock.Verify(x => x.GetPageStats(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetPageStats_ParserReturnStats_ReturnsResult()
        {
            _webClientMock.Setup(x => x.DownloadString("http://abc.com"))
                .Returns("zzz");

            _htmlParserMock.Setup(x => x.GetPageStats("zzz"))
                .Returns(new List<KeywordStat> { new KeywordStat { Keyword = "abcdef", Count = 11 } });

            var result = _sut.GetPageStats("http://abc.com").ToList();

            result.Should().HaveCount(1);
            result.Single().Keyword.Should().Be("abcdef");
            result.Single().Count.Should().Be(11);

            _webClientMock.Verify(x => x.DownloadString(It.IsAny<string>()), Times.Once);
            _webClientMock.Verify(x => x.DownloadString("http://abc.com"), Times.Once);
            _htmlParserMock.Verify(x => x.GetPageStats(It.IsAny<string>()), Times.Once);
            _htmlParserMock.Verify(x => x.GetPageStats("zzz"), Times.Once);
        }

        [Fact]
        public void GetPageStats_WebClientThrowsException_ThrowsException()
        {
            _webClientMock.Setup(x => x.DownloadString("http://abc.com"))
                .Throws<WebException>();

            Action act = () => _sut.GetPageStats("http://abc.com");

            act.Should().Throw<WebException>();

            _webClientMock.Verify(x => x.DownloadString(It.IsAny<string>()), Times.Once);
            _webClientMock.Verify(x => x.DownloadString("http://abc.com"), Times.Once);
            _htmlParserMock.Verify(x => x.GetPageStats(It.IsAny<string>()), Times.Never);
        }
    }
}