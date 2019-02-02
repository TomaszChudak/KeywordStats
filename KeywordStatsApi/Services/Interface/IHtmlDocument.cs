using HtmlAgilityPack;

namespace KeywordStatsApi.Services.Interface
{
    public interface IHtmlDocument
    {
        HtmlNode DocumentNode { get; }
    }
}