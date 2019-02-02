using System.Collections.Generic;
using HtmlAgilityPack;

namespace KeywordStatsApi.Services.Interface
{
    public interface IMetaKeywordsTagParser
    {
        string GetMetaKeywordTag(HtmlNode documentNode);
        IEnumerable<string> GetKeywordsFromMetaKeywordTag(string metaKeywordsTag);
    }
}