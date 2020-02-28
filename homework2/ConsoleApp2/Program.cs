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
            bool[] isNotPrime;                                //不是质数，则值为true,反之
            isNotPrime = new bool[100];
            for(int i=2;i<=100/i;i++)                         //将所有数的倍数的数组值设为true,即定为非质数
            {
                for(int mutiple=i;mutiple*i<=100;mutiple++)   //倍数从当前值i开始，因为i之前的数的倍数都已经算过。即最多算到10乘10
                {
                    isNotPrime[mutiple * i-1] = true;
                }
            }
            for(int num=2;num<=100;num++)                    //输出所有isNotPrime为0的数，即质数
            {
                if (isNotPrime[num - 1] == false)
                    Console.WriteLine(num);
            }
            
        }
    }
}
