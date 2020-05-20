using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderApi.models;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderContext orderDb;
        public OrderController(OrderContext context)
        {
            this.orderDb = context;
        }
        
        [HttpGet("all")]  //  api/order/all               //获得全部
        public IQueryable<Order> GetAllOrders()
        {
            IQueryable<Order> query = orderDb.Orders.Include("Items");
            return query;
        }

        // GET: api/order/inquiry
        [HttpGet("inquiry")]                            //按订单号、客户名、商品名查询（查询参数）
        public ActionResult <List<Order>> GetOrders(string OrderId,string clientName,string productName)
        {
            IEnumerable<Order> query = orderDb.Orders.Include("Items");
            if(OrderId!=null)
            {
                query = query.Where(order => order.OrderId==OrderId);
                
            }
            if(clientName!=null)
            {
                query = query.Where(order => order.ClientName==clientName);
            }
            if(productName!=null)
            {
                query = query.Where(order => order.Items.Exists(item => item.ProductName == productName));
            }
            var result = query.ToList();
            foreach(Order order in result)
            {
                order.Items.ToList();
            }
            return result;
        }

        //Post: api/order/add
        [HttpPost("add")]
        public ActionResult<Order> AddOrder(Order order)
        {
            double total = 0;
            order.CreateTime = DateTime.UtcNow.ToShortDateString();
            foreach(OrderItem item in order.Items)
            {
                item.OrderId = order.OrderId;
            }
            try
            {
                orderDb.Orders.Add(order);
                foreach (OrderItem item in order.Items)
                {
                    total += item.Count * item.Price;
                    orderDb.OrderItems.Add(item);
                }
                order.Total = total;
                orderDb.SaveChanges();
            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return order;
        }

        [HttpPut("modify/{OrderId}")]
            public ActionResult<Order> ModifyOrder(string OrderId,Order order)
        {
            double total = 0;
            var oldOrder = orderDb.Orders.FirstOrDefault(order => order.OrderId == OrderId);
            if (oldOrder == null)
            {
                return BadRequest("OrderId is not found!");
            }
            if (order.OrderId != OrderId)
            {
                return BadRequest("OrderId cannot be modified!");
            }
            try
            {
                order.CreateTime = oldOrder.CreateTime;
                foreach(OrderItem item in orderDb.OrderItems)
                {
                    if (item.OrderId == oldOrder.OrderId)
                        orderDb.OrderItems.Remove(item);
                }
                orderDb.Orders.Remove(oldOrder);
                orderDb.SaveChanges();
                order.Items.ForEach(item=> item.OrderId=OrderId);
                orderDb.Orders.Add(order);
                foreach(OrderItem item in order.Items)
                {
                    orderDb.OrderItems.Add(item);
                    total += item.Count * item.Price;
                }
                order.Total = total;
                orderDb.SaveChanges();
            }catch(Exception e)
            {
                string error = e.Message;
                if (e.InnerException != null)
                    error = e.InnerException.Message;
                return BadRequest(error);
            }
            return NoContent();
        }

        [HttpDelete("delete/{OrderId}")]
        public ActionResult DeleteOrder(string OrderId)
        {
            var order = orderDb.Orders.FirstOrDefault(order => order.OrderId == OrderId);
            if (order == null)
            {
                return BadRequest("OrderId is not found!");
            }
            else
            {
                foreach(OrderItem item in orderDb.OrderItems)
                {
                    if (item.OrderId == OrderId)
                        orderDb.OrderItems.Remove(item);
                }
                orderDb.Orders.Remove(order);
                orderDb.SaveChanges();
            }
            return NoContent();
        }
    }
}
