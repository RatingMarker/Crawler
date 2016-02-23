namespace Crawler.Workflow.Dowmloaders
{
    public interface IDownloader
    {
        string Download(string url);
        byte[] DownloadFile(string url);
    }
}