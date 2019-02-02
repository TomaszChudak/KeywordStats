using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using KeywordStatsApi.Services.Interface;

namespace KeywordStatsApi.Services.Implementation
{
    public class MetaKeywordsTagParser : IMetaKeywordsTagParser
    {
        private const string MetaKeywordTagRegex = "<meta\\sname=\"keywords\"\\scontent=\"[^\"]*\">";

        public IEnumerable<string> GetKeywordsFromMetaKeywordTag(string metaKeywordsTag)
        {
            try
            {
                var content =
                    metaKeywordsTag.Substring(metaKeywordsTag.IndexOf("content=\"", StringComparison.InvariantCultureIgnoreCase) + 9);
                content = content.Substring(0, content.IndexOf("\"", StringComparison.InvariantCultureIgnoreCase));
                return content.Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim());
            }
            catch
            {
                throw new ArgumentException("Error during parsing meta keyword tag");
            }
        }

        public string GetMetaKeywordTag(HtmlNode documentNode)
        {
            try
            {
                return documentNode
                    .ChildNodes.Single(x => x.Name == "html")
                    .ChildNodes.Single(x => x.Name == "head")
                    .ChildNodes.Where(x => x.Name == "meta")
                    .Single(x => Regex.Match(x.OuterHtml, MetaKeywordTagRegex, RegexOptions.IgnoreCase).Success)
                    .OuterHtml;
            }
            catch
            {
                throw new ArgumentException("No meta keyword tag has been found.");
            }
        }
    }
}