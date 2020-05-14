using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Data.Entity;
namespace ConsoleApp6
{
   public class OrderService                           //数据库版
    {
        public List<Order> orderList;
        public OrderService(List<Order> list)             //构造函数
        {
            if (list == null)
                this.orderList = new List<Order> { };
            else
            this.orderList = list;
        }
        public OrderService()
        {
            this.orderList = new List<Order>();

            using (var context = new OrderingContext())                 //将数据库的数据读入orderlist
            {
                var orderquery = context.OrderforDBs.Include("ItemforDBs");  //联接查询
                foreach (var orderdb in orderquery)
                {
                    Order order = new Order();
                    order.OrderNo = orderdb.orderNo;                    
                    order.Clients.Name = orderdb.clientName;
                    order.OrderDate = Convert.ToDateTime(orderdb.dateTime);
                    order.Total = orderdb.total;
                    foreach (var itemdb in orderdb.ItemforDBs)           //对每个order的itemlist赋值
                    {
                        OrderItem item = new OrderItem();
                        item.Products.Name = itemdb.productName;
                        item.price = itemdb.price;
                        item.Number = itemdb.count;
                        item.Total = itemdb.total;
                        order.OrderItemList.Add(item);
                    }
                    this.orderList.Add(order);
                }
            }
        }


        public void addOrder(Order o)                        //添加订单
        {
            foreach (Order x in orderList)                
                if (x.Equals(o))
                    return;
            orderList.Add(o);
            sort();                     //添加后按订单号排序
  
            using (var context = new OrderingContext())                 //将新order加入数据库
            {
                var orderdb = new OrderforDB
                {
                    orderNo = o.OrderNo,
                    clientName = o.Clients.Name,
                    dateTime = o.OrderDate.ToString(),
                    total = o.Total
                };
                    orderdb.ItemforDBs = new List<ItemforDB>();
                    foreach (OrderItem oi in o.OrderItemList)         //对order的itemlist赋值
                {
                       ItemforDB itemfordb = new ItemforDB() { productName= oi.Products.Name, price=oi.price,
                                                                   count = oi.Number,total=oi.Total};
                       orderdb.ItemforDBs.Add(itemfordb);
                    }
                    context.OrderforDBs.Add(orderdb);
                    context.SaveChanges();
            } 
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

            using (var context = new OrderingContext())                 //在数据库中删除order
            {
                var orderdb = context.OrderforDBs.Include("ItemforDBs").FirstOrDefault(p => p.orderNo == orderNo); //联接查询删除
                if (orderdb != null)
                {
                    context.OrderforDBs.Remove(orderdb);
                    context.SaveChanges();
                }
            }
        }



        public void modifyOrder(string orderNo, Order o)      //修改订单
        {
            int i = 0;
            for (i = 0; i < orderList.Count; i++)
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

            using (var context = new OrderingContext())                 //在数据库中修改order
            {
                var orderdb = context.OrderforDBs.Include("ItemforDBs").FirstOrDefault(p => p.orderNo == orderNo);
                if (orderdb != null)
                {
                    orderdb.orderNo = o.OrderNo;
                    orderdb.clientName = o.Clients.Name;
                    orderdb.total = o.Total;

                    var itemdb = context.ItemforDBs.Where(p => p.OrderforDBId == orderdb.OrderforDBId);  //参照完整性约束，先删除原item
                    if (itemdb != null)
                    {
                        foreach (var item in itemdb)
                            context.ItemforDBs.Remove(item);
                    }
                    context.SaveChanges();

                    orderdb.ItemforDBs = new List<ItemforDB>();
                    foreach (OrderItem item in o.OrderItemList)                      //添加新item，重新将新item加入orderlist
                    {
                        ItemforDB itemfordb = new ItemforDB()
                        {
                            productName = item.Products.Name,
                            price = item.price,
                            count = item.Number,
                            total = item.Total
                        };
                        orderdb.ItemforDBs.Add(itemfordb);
                    }
                    context.SaveChanges();
                }
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
