using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public List<string> getTopNumbers(int topQuantity = 30)
        {       
            var url = "";     
            var results = new Dictionary<string, int>();
            var date = new DateTime(2015, 1, 11);
            while(date <= DateTime.Today)
            {
                url = this._baseUrl + string.Format("?DtSorteio={0}-{1}-{2}&enviar=Ok", date.Year, date.Month, date.Day);
                LoadResult(url, results);
                date = date.AddDays(7);
            } 

            var aux = results.OrderBy(n => n.Value)
                        .Select(n => n.Key)
                        .Take(topQuantity);
            
            return aux.ToList();
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
                foreach(var node in nodes)
                {
                    var value = node.InnerHtml.Trim();
                    if(data.ContainsKey(value))
                    {
                        data[value]++;
                    }
                    else
                    {
                        data.Add(value, 1);
                    }
                }
            }
        }
    }
}