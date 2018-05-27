using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MegaManiaResults.Core
{
    public class WebManipulator
    {
        private readonly string _baseUrl;
        public Dictionary<string, int> _results;
        public Dictionary<string, int> Results { get => this._results; }

        public WebManipulator(string baseUrl)
        {
            this._baseUrl = baseUrl;
            this._results = new Dictionary<string, int>();
        }

        public void LoadResults()
        {
            var startDate = new DateTime(2015, 1, 11);
            int numberDayToNow = (int)(DateTime.Today - startDate).TotalDays;

            Parallel.For(0, numberDayToNow, index =>
            {
                this.GetResultFromUrl($"{this._baseUrl}?DtSorteio={startDate.AddDays(index).ToString("yyyy-MM-dd")}&enviar=Ok");
            });
        }

        public string[] GetTopNumbers(int topQuantity = 30)
        {
            if (this._results.Count == 0)
                this.LoadResults();

            return this._results.AsParallel()
                        .Select(n => n.Key)
                        .Take(topQuantity)
                        .ToArray();

        }

        private void GetResultFromUrl(string url)
        {
            using (var client = new WebClient())
            {
                var htmlString = client.DownloadString(url);
                this.ExtractResultNumbers(htmlString);
            }
        }

        private void ExtractResultNumbers(string htmlString)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlString);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='bola']");
            if (nodes != null)
            {
                Parallel.ForEach(nodes, (node) =>
                {
                    var value = node.InnerHtml.Trim();
                    if (this._results.ContainsKey(value))
                        this._results[value]++;
                    else
                        this._results.Add(value, 1);
                });
            }
        }
    }
}
