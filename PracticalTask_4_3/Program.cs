using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTask_4_3
{
    // * Дан фрагмент программы: 
    // а) Свернуть обращение к OrderBy с использованием лямбда-выражения =>
    // b) * Развернуть обращение к OrderBy с использованием делегата.

    class Program
    {
        static int Expend(KeyValuePair<string, int> pair) 
        { 
            return pair.Value; 
        }

        static void Main(string[] args)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>()
            {
                {"four",4 },
                {"two",2 },
                { "one",1 },
                {"three",3 },
            };

            var d = dict.OrderBy(pair => pair.Value);
            foreach (var pair in d)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }
            Console.ReadLine();



            d = dict.OrderBy(Expend);
            foreach (var pair in d)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }
            Console.ReadLine();

        }
    }
}
