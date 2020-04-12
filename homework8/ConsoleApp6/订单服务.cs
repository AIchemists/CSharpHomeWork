using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
namespace ConsoleApp6
{
   public class OrderService
    {
        public List<Order> orderList;
        public OrderService(List<Order> list)             //构造函数
        {
            if (list == null)
                this.orderList = new List<Order> { };
            else
            this.orderList = list;
        }
        public void addOrder(Order o)
        {
            foreach (Order x in orderList)                  //添加订单
                if (x.Equals(o))
                    return;
            orderList.Add(o);
            sort();                     //添加后按订单号排序
        }
        public void sort()                              //排序
        {
            this.orderList.Sort();
        }
        public void deleteOrder(string orderNo)          //删除订单（根据订单号）
        {
            int i;
            for (i = orderList.Count-1; i >= 0; i--)
            {
                if (orderList[i].OrderNo == orderNo)
                {
                    orderList.Remove(orderList[i]);
                    break;
                }
            }
            if (i <0)
            {
                Exception e = new Exception($"找不到订单{orderNo}。");
                throw e;
            }
        }
        public void modifyOrder(string orderNo, Order o)      //修改订单
        {
            int i = 0;
                for ( i = 0; i < orderList.Count; i++)
                {
                    if (orderList[i].OrderNo == orderNo)
                    {
                        orderList[i] = o;
                        break;
                    }
                }
            if (i == orderList.Count)
            {
                Exception e = new Exception($"找不到订单{orderNo}。");
                throw e;
            }
        }
        public Order InquireNo(String no)                     //按订单号查询（订单号不会重复，因此只返回一个order）
        {
            foreach(Order o in orderList)
            {
                if (o.OrderNo==no)
                {
                    return o;
                }
            }
                return null;                      //没找到
        }
        public List<Order> InquireProductName(String name)  //按商品名称查询
        {
            var query = orderList.Where(order =>
            {
                foreach (OrderItem item in order.OrderItemList)             //搜索每个order中的orderlist，返回布尔值
                    if (item.Products.Name == name)
                        return true;
                return false;
            })
            .OrderBy(order => order.Total);                           //根据总金额排序
            if (query.ToList().Count != 0)
                return query.ToList();
            else
                return null;
        }
        public List<Order> InquireClientName(String name)    //按客户名称查询
        {
            var query = from order in orderList
                        where order.Clients.Name == name
                        orderby order.Total
                        select order;
            List < Order > list= query.ToList();
            if (list.Count!=0)
                return query.ToList();
            else
                return null;
        }
        public void Export(String name)
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(name, FileMode.Create)) 
            {
                xmlserializer.Serialize(fs, this.orderList);
            }
        }
        public void Import(String name)
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(name, FileMode.Open))
            {
                this.orderList = (List<Order>)xmlserializer.Deserialize(fs);
            }
        }
    }
}
