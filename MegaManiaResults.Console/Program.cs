using MegaManiaResults.Core;
using System;
using System.Linq;

namespace megamaniaresults
{
	internal static class Program
	{
#pragma warning disable RCS1163 // Unused parameter.
#pragma warning disable IDE0060 // Remove unused parameter

		private static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore RCS1163 // Unused parameter.
		{
			DateTime start = DateTime.Now;
			Console.WriteLine("Esta aplicação irá os número mais sorteados da Magamania Paraná");
			Console.WriteLine("Endereço dos resultados:");

			try
			{
#pragma warning disable S1075 // URIs should not be hardcoded
				const string url = "http://adm.megamaniadasorte.com.br/consultaresultado/index.php";
#pragma warning restore S1075 // URIs should not be hardcoded
				Console.WriteLine($"{url}\n");

				var quantity = 30;
				IWebManipulator manipulator = new WebManipulator(url);
				manipulator.LoadResults();

				var test = manipulator.Results
							.OrderByDescending(x => x.Value)
							.ToArray();

				int count = 0;
				foreach (var item in test)
				{
					count++;
					Console.Write($"{item.Key}\t{item.Value}\t\t");

					if (count > 4)
					{
						Console.Write("\n");
						count = 0;
					}
				}

				Console.WriteLine("\n\nTop {0}", quantity);
				var numbers = manipulator.GetTopNumbers(quantity).OrderBy(x => x);
				foreach (int number in numbers)
				{
					count++;
					Console.Write("{0}\t", number);

					if (count > 10)
					{
						Console.Write("\n");
						count = 0;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
				Console.WriteLine(ex.Message);
			}
			Console.WriteLine();
			Console.WriteLine("Tempo consumido: {0}\n", DateTime.Now - start);
			Console.Read();
		}
	}
}