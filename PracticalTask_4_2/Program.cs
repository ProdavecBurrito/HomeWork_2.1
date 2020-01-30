using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTask_4_2
{
    class Program
    {
        // Задача: Дана коллекция List<T>. Требуется подсчитать, сколько раз каждый элемент встречается в данной коллекции:
        // a) для целых чисел;
        // б) * для обобщенной коллекции
        // c) ** используя Linq.




        static void Main(string[] args)
        {
            List<int> num = new List<int>() { 1, 2, 3, 4, 5, 4, 3, 2, 1 };
            GetNumOfVal(num);
            Console.ReadKey();
            Console.WriteLine();

            List<double> doubleNum = new List<double>() { 2.3, 2.7, 2.8, 2.3 };
            GetNumOfVal(doubleNum);
            Console.ReadKey();
            Console.WriteLine();

            List<string> words = new List<string>() { "Taco", "Burrito", "Taco", "Cat", "Burrito" };
            GetNumOfVal(words);
            Console.ReadKey();


            void GetNumOfVal<T>(ICollection<T> list)
            {
                var result = list.GroupBy(x => x)
                              .Where(x => x.Count() > 0)
                              .Select(x => new { val = x.Key, number = x.Count() });

                foreach (var item in result)
                {
                    Console.WriteLine($"Значение {item.val} повторяется {item.number} раз(а)");
                }

            }
            }

        }

    }
