using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            string s;
            int number;
            do
                {
                Console.WriteLine("请输入一个自然数");
                s = Console.ReadLine();
                if (!int.TryParse(s, out number))
                    Console.WriteLine("请输入合法的数据！");
            } while (!int.TryParse(s, out number));
            bool isPrime(int factor)                  //判断是否为素数
                {
                    for(int i=2;i<=factor/2;i++)
                    {
                        if (factor % i == 0)
                            return false;
                    }
                return true;
                }
            void primeFactor(int num)              //输出素数因子函数
                {
                    Console.WriteLine($"{num}的素数因子为");
                    for(int i=2;i<=num/2;i++)
                        {
                            if (num % i == 0 && isPrime(i))  //如果是因子且是素数
                               Console.WriteLine(i);
                        }
                }
            primeFactor(number);
        }
    }
}
