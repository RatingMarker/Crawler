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
                var client = new RestClient(url);

                var request = new RestRequest(Method.GET);

                var response = client.Execute(request);

                if (response.ResponseStatus != ResponseStatus.Error)
                {
                    var buffer = response.RawBytes;

                    if (response.ContentType != null)
                    {
                        charSet = new ContentType(response.ContentType).CharSet ?? charSet;
                    }

                    content = Encoding.GetEncoding(charSet).GetString(buffer);
                }
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