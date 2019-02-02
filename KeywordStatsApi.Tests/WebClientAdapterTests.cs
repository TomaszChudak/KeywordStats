using System;
using FluentAssertions;
using KeywordStatsApi.Services.Implementation;
using KeywordStatsApi.Services.Interface;
using Xunit;

namespace KeywordStatsApi.Tests
{
    public class WebClientAdapterTests
    {
        public WebClientAdapterTests()
        {
            _sut = new WebClientAdapter();
        }

        private readonly IWebClient _sut;

        [Fact]
        public void DownloadString_NonExistingUrl_ThrowsException()
        {
            Action act = () => _sut.DownloadString("abc.txt");

            act.Should().Throw<Exception>();
        }
    }
}