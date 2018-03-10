using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace megamaniaresults
{
    public class WebManipulator
    {
        private readonly string _baseUrl;

        public WebManipulator(string baseUrl)
        {
            this._baseUrl = baseUrl;
        }

        public string[] getTopNumbers(int topQuantity = 30)
        {
            var url = new StringBuilder();
            var results = new Dictionary<string, int>();
            var date = new DateTime(2015, 1, 11);

            while (date <= DateTime.Today)
            {
                url.Clear();
                url.Append(this._baseUrl);
                url.Append(string.Format("?DtSorteio={0}&enviar=Ok", date.ToString("yyyy-MM-dd")));
                date = date.AddDays(7);
                LoadResult(url.ToString(), results);

            }


            return results.AsParallel()
                        .Select(n => n.Key)
                        .Take(topQuantity)
                        .ToArray();

        }

        private void LoadResult(string url, Dictionary<string, int> data)
        {
            using (var client = new WebClient())
            {
                var htmlString = client.DownloadString(url);
                this.extractResult(htmlString, data);
            }
        }

        private void extractResult(string htmlString, Dictionary<string, int> data)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlString);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='bola']");
            if (nodes != null)
            {
                Parallel.ForEach(nodes, (node) =>
                {
                    var value = node.InnerHtml.Trim();
                    if (data.ContainsKey(value))
                        data[value]++;
                    else
                        data.Add(value, 1);
                });
            }
        }
    }
}