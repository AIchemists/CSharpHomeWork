using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1
{
    public delegate void Tick(object sender,int time);
    public delegate void Alarm(object sender,int time);
    public class Clock
    {
        public event Tick tick;
        public event Alarm alarm;
        public int time=0;
        public void ClockGo()
        {
            while(true)
            {
                tick(this,time);
                if (time % 24 == 8)                              //达到条件（8：00），触发alarm
                alarm(this,time);
                time += 1;
                Thread.Sleep(1000);
            } 
        }
    }
    public class createClock
    {
        public Clock clock = new Clock();
        public createClock()
            {
            clock.tick += tick1;                                   //事件订阅
            clock.alarm += alarm1;                                
            }
            void tick1(object sender, int time)
        {
            Console.WriteLine($"the clock is ticking {time % 24}:00");              //时钟走时发出信息
        }
        void alarm1(object sender, int time)
        {
            Console.WriteLine($"叮铃叮铃，现在的时间是 {time % 24}:00！");             //响铃信息
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            createClock clockTest = new createClock();
            clockTest.clock.ClockGo();                                //闹钟启动
        }
    }
}
