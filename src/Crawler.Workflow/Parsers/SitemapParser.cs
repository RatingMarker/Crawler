using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Crawler.Workflow.Parsers
{
    public class SitemapParser: IParser
    {
        public IEnumerable<string> GetUrls(string content)
        {
            IList<string> urls = new List<string>();

            try
            {
                XmlDocument xml = new XmlDocument();

                xml.LoadXml(content);

                XmlNamespaceManager manager = new XmlNamespaceManager(xml.NameTable);

                manager.AddNamespace("s", "http://www.sitemaps.org/schemas/sitemap/0.9");
                XmlNodeList xnList = GetNodes(xml, manager, "/s:sitemapindex/s:sitemap");

                if (xnList.Count > 0)
                {
                    foreach (XmlNode node in xnList)
                    {
                        var url = node["loc"].InnerText;
                        urls.Add(url);
                    }
                }

                xnList = GetNodes(xml, manager, "/s:urlset/s:url");

                if (xnList.Count > 0)
                {
                    foreach (XmlNode node in xnList)
                    {
                        var url = node["loc"].InnerText;
                        urls.Add(url);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            urls = urls.Distinct().ToList();

            return urls;
        }

        private XmlNodeList GetNodes(XmlDocument xml, XmlNamespaceManager manager, string tags)
        {
            return xml.SelectNodes(tags, manager);
        }
    }
}