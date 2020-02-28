using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = { 100, 233, 456, 88, 796, 335, 647, 164 };
            int maxNum(int []array)                //求最大值函数
            {
                if (array.Length==0)
                {
                    Console.WriteLine("数组为空");
                }
                int maximun = array[0];
                foreach ( int num in array)
                {
                    if (num > maximun)
                       maximun = num;
                }
                return maximun;
            }
            int minNum(int [] array)                  //求最小值函数
            {
                if (array.Length == 0)
                {
                    Console.WriteLine("数组为空");
                }
                int minimun = array[0];
                foreach(int num in array)
                {
                    if (minimun > num)
                        minimun = num;
                }
                return minimun;
            }
            double average(int []array)              //求平均值函数
            {
                if (array.Length == 0)
                {
                    Console.WriteLine("数组为空");
                }
                int sum = 0;
                foreach (int num in array)
                {
                    sum += num;
                }
                return (sum / array.Length);
            }
            int sumNum(int[]array)
            {
                if (array.Length == 0)
                {
                    Console.WriteLine("数组为空");
                }
                int sum = 0;
                foreach(int num in array)
                {
                    sum += num;
                }
                return sum;
            }
            Console.WriteLine("该数组的最大值为：");
            Console.WriteLine(maxNum(a));
            Console.WriteLine("该数组的最小值为：");
            Console.WriteLine(minNum(a));
            Console.WriteLine("该数组的平均值为：");
            Console.WriteLine(average(a));
            Console.WriteLine("该数组的和为：");
            Console.WriteLine(sumNum(a));
        }
    }
}
