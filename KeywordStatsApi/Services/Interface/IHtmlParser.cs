using System.Collections.Generic;
using KeywordStatsApi.Models;

namespace KeywordStatsApi.Services.Interface
{
    public interface IHtmlParser
    {
        IEnumerable<KeywordStat> GetPageStats(string html);
    }
}