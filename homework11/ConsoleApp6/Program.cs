using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6                      
{
    class Program
    {
        static void Main(string[] args)
        {
            Client dzq = new Client("dzq", "wuhan university");
            Client Tom = new Client("Tom", "wuhan university");
            Product book = new Product("Textbook",103.5);
            Product pen = new Product("Pen", 5.6);
            Product rubber = new Product("Rubber", 3);
            Product apple = new Product("Apple",4.5);
            Order order1 = new Order("123", dzq, new DateTime(2020, 3, 20), null);
            Order order2 = new Order("456", Tom, new DateTime(2020, 2, 29), null);
            Order order3 = new Order("289", dzq, new DateTime(2020, 2, 7), null);
            order1.AddItem(new OrderItem(book,5));
            order2.AddItem(new OrderItem(pen, 10));
            order3.AddItem(new OrderItem(rubber, 2));
            order3.AddItem(new OrderItem(apple, 4));
            List<Order> list = new List<Order> { order1, order2 };
            OrderService orderService1 = new OrderService(list);

            //开始测试
            Console.WriteLine("初始状态");
            Console.WriteLine(orderService1.orderList[0].ToString());
            Console.WriteLine(orderService1.orderList[1].ToString());

            Console.WriteLine("\n添加订单");
            orderService1.addOrder(order3);
            Console.WriteLine(orderService1.orderList[1].ToString());//后加的订单order3排在了order2前面，说明是按照订单号自动排序的
            Console.WriteLine(orderService1.orderList[2].ToString());

            Console.WriteLine("\n删除订单");
            orderService1.deleteOrder("123");//删除订单号为123的订单
            Console.WriteLine(orderService1.orderList[0].ToString());

            Console.WriteLine("\n修改订单");
            Order newOrder3 =  new Order("289", dzq, new DateTime(2020, 2, 7), null);
            newOrder3.AddItem(new OrderItem(rubber, 2));
            newOrder3.AddItem(new OrderItem(apple, 2));
            try
            {
                orderService1.modifyOrder("289", newOrder3);//修改订单号为289的订单
                Console.WriteLine(orderService1.orderList[0].ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\n查询订单");
                order1.AddItem(new OrderItem(apple, 4));
                orderService1.addOrder(order1);
            Console.WriteLine("\n按订单号查询订单");
            Order inquire1 = orderService1.InquireNo("123");           //查询订单号为123的订单
            if (inquire1 != null)
            {
                Console.WriteLine(inquire1.ToString());
            }
            else
                Console.WriteLine("\n未查询到订单");

            Console.WriteLine("\n按商品名称查询订单");
            List<Order> inquire2 = orderService1.InquireProductName("Apple");//查询包含apple的订单，是按总金额正序
            if (inquire2 != null)
            {
                foreach (Order order in inquire2)
                    Console.WriteLine(order.ToString());
            }
            else
                Console.WriteLine("\n未查询到订单");

            Console.WriteLine("\n按客户名查询订单");
            List<Order> inquire3 = orderService1.InquireClientName("dzq");     //查询客户为dzq的订单，是按总金额正序
            if(inquire3!=null)
            {
                foreach (Order order in inquire3)
                    Console.WriteLine(order.ToString());
            }
            else
                Console.WriteLine("\n未查询到订单");

            Console.WriteLine("\n\n序列化&反序列化");
            orderService1.Export("All orders.xml");
            OrderService orderService2 = new OrderService(null);
            orderService2.Import("All orders.xml");
            Console.WriteLine(orderService2.orderList[0].ToString());
            Console.WriteLine(orderService2.orderList[1].ToString());
        }
    }
}
