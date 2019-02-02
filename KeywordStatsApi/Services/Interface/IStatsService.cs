using System.Collections.Generic;
using KeywordStatsApi.Models;

namespace KeywordStatsApi.Services.Interface
{
    public interface IStatsService
    {
        IEnumerable<KeywordStat> GetPageStats(string url);
    }
}