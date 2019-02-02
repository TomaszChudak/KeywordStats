using System.Net;
using KeywordStatsApi.Services.Interface;

namespace KeywordStatsApi.Services.Implementation
{
    public class WebClientAdapter : IWebClient
    {
        public string DownloadString(string url)
        {
            var webClient = new WebClient();
            var html = webClient.DownloadString(url);
            return html;
        }
    }
}