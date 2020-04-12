using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    [Serializable]
    public class OrderItem
    {
        public Product Products { get; set; }
        public double price { get; set; }
        public int Number { get; set; }                  //商品数量（商品价格在Product类中）
        public double Total { get; set; }                   //总金额
        public OrderItem() { this.Products = new Product(); }
       public OrderItem(Product product,int number)
        {
            Products = new Product();
            Products.Name = product.Name;
            Products.Price = product.Price;
            price = Products.Price;
            this.Number = number;
            this.Total = number * product.Price;
        }
        
        public override bool Equals(object obj)                //判断订单号相同否？
        {
            OrderItem m = obj as OrderItem;
            return m != null && m.Products.Name == Products.Name;
        }
        public override string ToString()
        {
            return $"商品名称：{Products.Name},商品数量：{Number},商品单价：{Products.Price}";
        }
    }
    [Serializable]
    public class Order : IComparable
    {
        public String OrderNo { get; set; }
        public Client Clients { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItemList = new List<OrderItem>();
        public double Total { get; set; }
        public Order() { this.Clients = new Client(); }
        public Order(String no,Client person,DateTime time, List<OrderItem> list )
        {
            this.OrderNo = no;
            this.Clients = person;
            this.OrderDate = time;
            this.Total = 0;
            if (list != null)
                this.OrderItemList = list;
            foreach(OrderItem item in OrderItemList)
            {
                this.Total += item.Total;
            }
        }

        public override string ToString()                      //toString重写
        {
            string information =$"订单号：{OrderNo},客户信息：{Clients},下单时间：{OrderDate},总金额：{Total}";
            foreach(OrderItem oi in OrderItemList)
            {
                information+="\n订单明细："+oi.ToString();
            }
            return information;
        }
        public override bool Equals(object obj)                //Equals重写
        {
            Order m = obj as Order;
            return m != null && m.OrderNo == OrderNo;
        }
        public int CompareTo(object obj)                //按订单号排序
        {
            Order o = obj as Order;
            if (o == null)
                throw new System.ArgumentException();
            return this.OrderNo.CompareTo(o.OrderNo);
        }
        public void AddItem(OrderItem item)          //添加项目明细
        {
            OrderItemList.Add(item);
            Total += item.Total;
        }
        public bool HasItem(string name)          //判断是否有该商品名
        {
            foreach (OrderItem item in OrderItemList)
                if (item.Products.Name == name)
                    return true;
            return false;
        }
    }
}
