using System;
using System.Linq;
using FluentAssertions;
using KeywordStatsApi.Services.Implementation;
using KeywordStatsApi.Services.Interface;
using Xunit;

namespace KeywordStatsApi.Tests
{
    public class HtmlDocumentAdapterTests
    {
        private IHtmlDocument _sut;

        [Theory]
        [InlineData(null)]
        public void LoadHtml_NullHtml_ThrowsException(string html)
        {
            Action act = () => _sut = new HtmlDocumentAdapter(html);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        public void LoadHtml_EmptyHtml_ReturnsEmptyDoc(string html)
        {
            _sut = new HtmlDocumentAdapter(html);

            var result = _sut.DocumentNode;

            result.OuterHtml.Should().Be(html);
            result.ChildNodes.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("<html><head></head><body></body></html>")]
        public void LoadHtml_SimplestHtml_ReturnsHtmlDoc(string html)
        {
            _sut = new HtmlDocumentAdapter(html);

            var result = _sut.DocumentNode;

            result.OuterHtml.Should().Be(html);
            result.ChildNodes.Count.Should().Be(1);
            result.ChildNodes.Single().Name.Should().Be("html");
            result.ChildNodes.Single().ChildNodes.Count.Should().Be(2);
            result.ChildNodes.Single().ChildNodes.First().Name.Should().Be("head");
            result.ChildNodes.Single().ChildNodes.Last().Name.Should().Be("body");
        }
    }
}