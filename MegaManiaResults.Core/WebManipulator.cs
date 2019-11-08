using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MegaManiaResults.Core
{
	public class WebManipulator : IWebManipulator
	{
		private readonly string _baseUrl;
		public Dictionary<int, int> Results { get; }

		public WebManipulator(string baseUrl)
		{
			_baseUrl = baseUrl;

			Results = new Dictionary<int, int>();
			InitDictionary(Results);
		}

		public int[] GetTopNumbers(int topQuantity = 30)
		{
			return Results.OrderByDescending(x => x.Value).Take(topQuantity).Select(x => x.Key).ToArray();
		}

		// Method had better performance using parallel top-down 1:48 Task 2:01 Parallel 0:23
		public void LoadResults()
		{
			DateTime date = GetStartDate();
			TimeSpan diference = DateTime.Now - date;
			int days = (int)diference.TotalDays;

			Parallel.For(0, days, (index) =>
			{
				var dt = date.AddDays(index);
				if (dt.DayOfWeek == DayOfWeek.Sunday)
					GetResults(dt);
			});
		}

		private void InitDictionary(Dictionary<int, int> dic)
		{
			for (int i = 0; i < 60; i++)
				dic.Add(i + 1, 0);
		}

		private DateTime GetStartDate() => new DateTime(2015, 1, 11);

		/// <summary>
		/// Get draw numbers from a specific date
		/// </summary>
		/// <exception cref="ArgumentException">Datetime is not Sunday</exception>
		/// <param name="dateTime">Draw date</param>
		private void GetResults(DateTime dateTime)
		{
			if (dateTime.DayOfWeek != DayOfWeek.Sunday)
				throw new ArgumentException("Invalid date, draw happens only on Sundays.");

			string url = $"{this._baseUrl}?DtSorteio={dateTime.ToString("yyyy-MM-dd")}&enviar=Ok";
			string html = GetWebPage(url);
			if (string.IsNullOrEmpty(html))
				return;

			Dictionary<int, int> drawNumbers = ExtractDrawNumbers(html);
			IncrementDictionaryWithDrawValues(drawNumbers, Results);
		}

		private string GetWebPage(string url)
		{
			using (var client = new WebClient())
				return client.DownloadString(url);
		}

		private Dictionary<int, int> ExtractDrawNumbers(string html)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			InitDictionary(dictionary);

			string expressionNumberTag = "<div class=\"bola\">(.*?)</div>";
			MatchCollection matchCollection = Regex.Matches(html, expressionNumberTag);

			int length = matchCollection.Count;
			for (int i = 0; i < length; i++)
			{
				string div = matchCollection[i].Value;
				string draw = Regex.Replace(div, @"[^\d]", string.Empty);
				int value = int.Parse(draw);
				dictionary[value]++;
			}
			return dictionary;
		}

		private void IncrementDictionaryWithDrawValues(Dictionary<int, int> draw, Dictionary<int, int> result)
		{
			foreach (int key in draw.Keys)
				result[key] += draw[key];
		}
	}
}