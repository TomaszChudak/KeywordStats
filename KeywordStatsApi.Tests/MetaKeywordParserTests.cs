using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using HtmlAgilityPack;
using KeywordStatsApi.Services.Implementation;
using KeywordStatsApi.Services.Interface;
using Xunit;

namespace KeywordStatsApi.Tests
{
    public class MetaKeywordParserTests
    {
        private readonly IMetaKeywordsTagParser _sut;

        public MetaKeywordParserTests()
        {
            _sut = new MetaKeywordsTagParser();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("aaaaaaa")]
        public void GetKeywordsFromMetaKeywordTag_WrongKeywords_ThrowsException(string tag)
        {
            Action act = () => _sut.GetKeywordsFromMetaKeywordTag(tag);

            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("<meta name=\"Keywords\" content=\"\">")]
        public void GetKeywordsFromMetaKeywordTag_EmptyKeywordsTag_ReturnsEmptySet(string tag)
        {
            var result = _sut.GetKeywordsFromMetaKeywordTag(tag);

            result.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("<meta name=\"Keywords\" content=\"abc\">")]
        public void GetKeywordsFromMetaKeywordTag_OneKeyword_ReturnsOne(string tag)
        {
            var result = _sut.GetKeywordsFromMetaKeywordTag(tag).ToList();

            result.Should().HaveCount(1);
            result.Single().Should().Be("abc");
        }

        [Theory]
        [InlineData("<meta name=\"Keywords\" content=\"abc def, ghi jkl, mno pqr\">")]
        public void GetKeywordsFromMetaKeywordTag_OneKeywords_ReturnsKeyword(string tag)
        {
            var result = _sut.GetKeywordsFromMetaKeywordTag(tag).ToList();

            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(new List<string> { "abc def", "ghi jkl", "mno pqr" });
        }

        [Fact]
        public void GetMetaKeywordTag_NullHtmlDoc_ThrowsException()
        {
            Action act = () => _sut.GetMetaKeywordTag(null);

            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaaaaa")]
        [InlineData("<html><head></head><body></body></html>")]
        public void GetMetaKeywordTag_NoKeywords_ThrowsException(string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            Action act = () => _sut.GetMetaKeywordTag(htmlDocument.DocumentNode);

            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("<html><head><meta name=\"keywords\" </head><body></body></html>")]
        [InlineData("<html><head></head><body><meta name=\"keywords\" content=\"wrong\"></body></html>")]
        [InlineData("<html><head></head><body></body><meta name=\"keywords\" content=\"wrong\"></html>")]
        public void GetMetaKeywordTag_WrongKeywordsTag_ThrowsException(string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            Action act = () => _sut.GetMetaKeywordTag(htmlDocument.DocumentNode);

            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("<html><head><meta name=\"keywords\" content=\"abc\"></head><body></body></html>")]
        public void GetMetaKeywordTag_RightTag_ReturnsTag(string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var result = _sut.GetMetaKeywordTag(htmlDocument.DocumentNode);

            result.Should().Be("<meta name=\"keywords\" content=\"abc\">");
        }
    }
}