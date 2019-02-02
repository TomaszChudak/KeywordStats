using System.Collections.Generic;
using System.Linq;
using KeywordStatsApi.Models;
using KeywordStatsApi.Services.Interface;

namespace KeywordStatsApi.Services.Implementation
{
    public class HtmlParser : IHtmlParser
    {
        private readonly IBodyParser _bodyParser;
        private readonly IHtmlDocumentFactory _htmlDocumentFactory;
        private readonly IMetaKeywordsTagParser _metaKeywordsTagParser;

        public HtmlParser(IHtmlDocumentFactory htmlDocumentFactory, IMetaKeywordsTagParser metaKeywordsTagParser,
            IBodyParser bodyParser)
        {
            _htmlDocumentFactory = htmlDocumentFactory;
            _metaKeywordsTagParser = metaKeywordsTagParser;
            _bodyParser = bodyParser;
        }

        public IEnumerable<KeywordStat> GetPageStats(string html)
        {
            var htmlDocument = _htmlDocumentFactory.Create(html);
            var metaKeywordTag = _metaKeywordsTagParser.GetMetaKeywordTag(htmlDocument.DocumentNode);
            var keywords = _metaKeywordsTagParser.GetKeywordsFromMetaKeywordTag(metaKeywordTag);

            var result = new List<KeywordStat>();
            keywords.ToList().ForEach(k => result.Add(new KeywordStat
                {
                    Keyword = k, 
                    Count = _bodyParser.CountKeywordOccurrences(htmlDocument.DocumentNode, k)

                }));
            return result;
        }
    }
}