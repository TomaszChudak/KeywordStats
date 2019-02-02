using System;
using System.IO;

namespace KeywordStatsApi.Services.Interface
{
    public interface IWebClient
    {
        string DownloadString(string url);
    }
}