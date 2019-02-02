using HtmlAgilityPack;
using KeywordStatsApi.Services.Interface;

namespace KeywordStatsApi.Services.Implementation
{
    public class HtmlDocumentAdapter : IHtmlDocument
    {
        private readonly HtmlDocument _htmlDocument;

        public HtmlDocumentAdapter(string html)
        {
            _htmlDocument = new HtmlDocument();
            _htmlDocument.LoadHtml(html);
        }

        public HtmlNode DocumentNode
        {
            get { return _htmlDocument.DocumentNode; }
        }
    }
}