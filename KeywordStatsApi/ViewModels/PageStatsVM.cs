using System.Collections.Generic;

namespace KeywordStatsApi.ViewModels
{
    public class PageStatsVM
    {
        public string PageUrl { get; set; }
        public IEnumerable<KeywordStatVM> KeywordStats { get; set; }
        public string ErrorDesc { get; set; }
    }
}