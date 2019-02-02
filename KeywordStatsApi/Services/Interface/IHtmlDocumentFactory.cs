namespace KeywordStatsApi.Services.Interface
{
    public interface IHtmlDocumentFactory
    {
        IHtmlDocument Create(string html);
    }
}