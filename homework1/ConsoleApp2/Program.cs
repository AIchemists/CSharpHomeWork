using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string s; double x, y; char option;
            do
            {
                Console.WriteLine("请输入一个数字");
                s = Console.ReadLine();
                if (!double.TryParse(s, out x))

                    Console.WriteLine("数据输入错误！");
            } while (!double.TryParse(s, out x));
            do
            {
                Console.WriteLine("请输入另一个数字");
                s = Console.ReadLine();
                if (!double.TryParse(s, out y))
                    Console.WriteLine("数据输入错误！");
            } while (!double.TryParse(s, out x));
            Console.WriteLine("请输入运算符:+,-,*,/");
            option =Convert.ToChar(Console.ReadLine());
            Console.WriteLine("结果为：");
            switch (option)
            {
                case '+': Console.WriteLine(x + y);break ;
                case '-': Console.WriteLine(x - y); break;
                case '*': Console.WriteLine(x * y); break;
                case '/': if (y == 0)
                        Console.WriteLine("除数不能为0！");
                        else
                        Console.WriteLine(x/ y); break;
                default:Console.WriteLine("请输入正确的运算符！");break;
            }
        }
    }
}
