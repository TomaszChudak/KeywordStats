using KeywordStatsApi.Services.Interface;

namespace KeywordStatsApi.Services.Implementation
{
    public class HtmlDocumentFactory : IHtmlDocumentFactory
    {
        public IHtmlDocument Create(string html)
        {
            var htmlDocument = new HtmlDocumentAdapter(html);
            return htmlDocument;
        }
    }
}