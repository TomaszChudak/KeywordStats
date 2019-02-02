using System.Collections.Generic;
using KeywordStatsApi.Models;
using KeywordStatsApi.Services.Interface;

namespace KeywordStatsApi.Services.Implementation
{
    public class StatsService : IStatsService
    {
        private readonly IHtmlParser _htmlParser;
        private readonly IWebClient _webClient;

        public StatsService(IWebClient webClient, IHtmlParser htmlParser)
        {
            _webClient = webClient;
            _htmlParser = htmlParser;
        }

        public IEnumerable<KeywordStat> GetPageStats(string url)
        {
            var html = _webClient.DownloadString(url);
            return _htmlParser.GetPageStats(html);
        }
    }
}