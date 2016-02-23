using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using RestSharp;

namespace Crawler.Workflow.Dowmloaders
{
    public class RestDownloader: IDownloader
    {
        public string Download(string url)
        {
            var content = string.Empty;

            var charSet = "utf-8";

            try
            {
                var restClient = new RestClient(url);

                var restRequest = new RestRequest(Method.GET);

                var restResponse = restClient.Execute(restRequest);

                var buffer = restResponse.RawBytes;

                if (restResponse.ContentType != null)
                {
                    charSet = new ContentType(restResponse.ContentType).CharSet ?? charSet;
                }

                content = Encoding.GetEncoding(charSet).GetString(buffer);

                return content;
            }
            catch (Exception)
            {
                //ignore
            }

            return content;
        }

        public byte[] DownloadFile(string url)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.ResponseWriter = responseStream => responseStream.CopyTo(memoryStream);
                client.DownloadData(request);

                return memoryStream.ToArray();
            }
        }
    }
}