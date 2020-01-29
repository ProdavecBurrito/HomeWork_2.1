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

            foreach (int a in num.Distinct())
            {
                Console.WriteLine(a + " Встречается " + num.Where(m => m == a).Count() + " раз(а)");
            }



            List<String> words = new List<string>() { "lol", "kek", "epe", "lol" };

            GetUniques(words);
            Console.ReadLine();


            void GetUniques<T>(ICollection<T> list)
            {
                int a = 1;
                Dictionary<T, bool> found = new Dictionary<T, bool>();
                foreach (T val in list)
                {
                    if (!found.ContainsKey(val))
                    {
                        a++;
                        Console.WriteLine($"{val} встречается {a} раз");
                        found[val] = true;
                    }
                    else
                    {
                        a = 0;
                    }

                }
            }

        }

    }
}
