using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6.Tests
{
    [TestClass()]
    public class OrderServiceTests
    {
        [TestMethod()]
        public void OrderServiceTest()
        {
            List<Order> orders = new List<Order> { new Order () };
            OrderService orderServiceTest=new OrderService(orders);
            CollectionAssert.AreEqual(orderServiceTest.orderList,orders);
        }

        [TestMethod()]
        public void addOrderTest()
        {
            Order order = new Order();
            OrderService orderServiceTest = new OrderService(null);
            orderServiceTest.addOrder(order);
            Assert.AreEqual(orderServiceTest.orderList[0],order);
        }

        [TestMethod()]
        public void sortTest()
        {
            Order order1 = new Order("123",null,new DateTime(), null);
            Order order2 = new Order("456", null,new DateTime(), null);
            List<Order> list = new List<Order> { order2, order1 };
            OrderService orderServiceTest = new OrderService(list);
            orderServiceTest.sort();
            CollectionAssert.AreEqual(new List<Order> { order1, order2 },orderServiceTest.orderList);
        }

        [TestMethod()]
        public void deleteOrderTest()
        {

            Order order1 = new Order("123", null, new DateTime(), null);
            Order order2 = new Order("456", null, new DateTime(), null);
            List<Order> list = new List<Order> { order1, order2 };
            OrderService orderServiceTest = new OrderService(list);
            try
            {
                orderServiceTest.deleteOrder("789");             //找不到的情况,会抛出异常
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "找不到订单789。");          //判断异常是否正确
            }
            
            finally
            {
                orderServiceTest.deleteOrder("123");
                CollectionAssert.AreEqual(new List<Order> { order2 }, orderServiceTest.orderList);
            }
        }

        [TestMethod()]
        public void modifyOrderTest()
        {
            Order order1 = new Order("123", null, new DateTime(), null);
            Order order2 = new Order("456", null, new DateTime(), null);
            List<Order> list = new List<Order> { order1, order2 };
            OrderService orderServiceTest = new OrderService(list);
            Order newOrder2 = new Order("456", null, new DateTime(2020,3,27), null);
            try
            {
                orderServiceTest.modifyOrder("444", newOrder2); //找不到的情况，会抛出异常
            }
            catch(Exception e)
            {
                Assert.AreEqual(e.Message, "找不到订单444。");   //判断异常是否正确
            }
            orderServiceTest.modifyOrder("456",newOrder2);
            CollectionAssert.AreEqual(new List<Order> { order1, newOrder2 }, orderServiceTest.orderList);
        }

        [TestMethod()]
        public void InquireNoTest()
        {
            Order order1 = new Order("123", null, new DateTime(), null);
            List<Order> list = new List<Order> { order1 };
            OrderService orderServiceTest = new OrderService(list);
            Assert.IsNull(orderServiceTest.InquireNo("456"));    //查询不到会返回null
            Assert.AreSame(orderServiceTest.InquireNo("123"),order1);
        }

        [TestMethod()]
        public void InquireProductNameTest()
        {
            Product productTest = new Product("test", 1);
            Order order1 = new Order(null, null, new DateTime(), null);
            order1.AddItem(new OrderItem(productTest,1));
            List<Order> list = new List<Order> { order1 };
            List<Order> nullList = new List<Order> { };
            OrderService orderServiceTest = new OrderService(list);
            CollectionAssert.AreEqual(orderServiceTest.InquireProductName("null"),nullList);
            CollectionAssert.AreEqual(orderServiceTest.InquireProductName("test"),list);
        }

        [TestMethod()]
        public void InquireClientNameTest()
        {
            Client clientTest = new Client("test",null);
            Order order1 = new Order(null,clientTest,new DateTime(),null);
            List<Order> list = new List<Order> { order1 };
            List<Order> nullList = new List<Order> { };
            OrderService orderServiceTest = new OrderService(list);
            CollectionAssert.AreEqual(orderServiceTest.InquireClientName("null"),nullList);
            CollectionAssert.AreEqual(orderServiceTest.InquireClientName("test"), list);
        }

        [TestMethod()]
        public void ExportTest()
        {
            Order order1 = new Order("123", null, new DateTime(), null);
            Order order2 = new Order("456", null, new DateTime(), null);
            List<Order> list1 = new List<Order> { order1, order2 };
            List<Order> list2= new List<Order> { order1};
            OrderService orderServiceTest1 = new OrderService(list1);
            orderServiceTest1.Export("test1.xml");
            OrderService orderServiceTest2 = new OrderService(list2);
            orderServiceTest2.Export("test2.xml");
            OrderService orderServiceTest3 = new OrderService(null);
            orderServiceTest3.Import("test1.xml");
            OrderService orderServiceTest4 = new OrderService(null);
            orderServiceTest4.Import("test2.xml");
            CollectionAssert.AreEqual(orderServiceTest1.orderList,orderServiceTest3.orderList);
            CollectionAssert.AreEqual(orderServiceTest2.orderList, orderServiceTest4.orderList);
            CollectionAssert.AreNotEqual(orderServiceTest3.orderList, orderServiceTest4.orderList);
        }

        [TestMethod()]
        public void ImportTest()
        {
            Order order1 = new Order("123", null, new DateTime(), null);
            Order order2 = new Order("456", null, new DateTime(), null);
            List<Order> list1 = new List<Order> { order1, order2 };
            OrderService orderServiceTest1 = new OrderService(list1);
            orderServiceTest1.Export("test1.xml");
            OrderService orderServiceTest2 = new OrderService(null);
            orderServiceTest2.Import("test1.xml");
            CollectionAssert.AreEqual(list1,orderServiceTest2.orderList);
        }
    }
}