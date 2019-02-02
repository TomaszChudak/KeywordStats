using HtmlAgilityPack;

namespace KeywordStatsApi.Services.Interface
{
    public interface IBodyParser
    {
        HtmlNode GetBodyNode(HtmlDocument htmlDocument);
        int CountKeywordOccurrences(HtmlNode bodyNode, string keyword);
    }
}