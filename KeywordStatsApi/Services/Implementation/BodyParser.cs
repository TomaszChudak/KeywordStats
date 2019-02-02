using System;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using KeywordStatsApi.Services.Interface;

namespace KeywordStatsApi.Services.Implementation
{
    public class BodyParser : IBodyParser
    {
        public HtmlNode GetBodyNode(HtmlDocument htmlDocument)
        {
            try
            {
                return htmlDocument.DocumentNode.ChildNodes
                    .Single(x => x.Name.Equals("HTML", StringComparison.InvariantCultureIgnoreCase))
                    .ChildNodes
                    .Single(x => x.Name.Equals("BODY", StringComparison.InvariantCultureIgnoreCase));
            }
            catch
            {
                throw new ArgumentException("lack of body node in html document");
            }
        }

        public int CountKeywordOccurrences(HtmlNode htmlNode, string keyword)
        {
            var occurrences = 0;

            foreach (var childNode in htmlNode.ChildNodes)
            {
                occurrences += CountKeywordOccurrences(childNode, keyword);
            }

            if(htmlNode.NodeType == HtmlNodeType.Text)
                occurrences += Regex.Matches(htmlNode.InnerHtml, keyword).Count;
            
            return occurrences;
        }
    }
}