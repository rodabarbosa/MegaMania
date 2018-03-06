using System;

namespace megamaniaresults
{
    class Program
    {
        static void Main(string[] args)
        {
            var quantity = 30;
            var manipulator = new WebManipulator("http://adm.megamaniadasorte.com.br/consultaresultado/index.php");
            var numbers = manipulator.getTopNumbers(quantity);
            //numbers.Sort();
            Console.WriteLine(string.Format("Os {0} números mais sorteados:", quantity));            
            foreach(var number in numbers)
            {
                Console.Write(string.Format("{0} ", number));
            }
        }
    }
}
