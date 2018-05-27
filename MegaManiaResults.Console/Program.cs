using MegaManiaResults.Core;
using System;

namespace megamaniaresults
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("Esta aplicação irá os número mais sorteados da Magamania Paraná");
            Console.WriteLine("Endereço dos resultados:");
            try
            {
                string url = "http://adm.megamaniadasorte.com.br/consultaresultado/index.php";
                Console.WriteLine(url);
                Console.WriteLine();

                var quantity = 30;
                var manipulator = new WebManipulator(url);
                manipulator.LoadResults();
                foreach (var key in manipulator.Results.Keys)
                {
                    Console.WriteLine($"{key}: {manipulator.Results[key]}");
                }


                var numbers = manipulator.GetTopNumbers(quantity);

                Console.WriteLine(string.Format("Os {0} números mais sorteados:", quantity));
                Console.WriteLine("Número \t Quantidade");
                quantity = numbers.Length;
                for (var i = 0; i < quantity; i++)
                {
                    Console.WriteLine(string.Format("{0} \t {1}", numbers[i], i));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();
            Console.WriteLine("Tempo consumido: {0}", (DateTime.Now-start));
            Console.Read();
        }
    }
}
