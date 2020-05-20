using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.models
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; }
        [Required]
        public string ClientName { get; set; }
        public string CreateTime { get; set; }  //不用手动添加修改
        public double Total { get; set; }//不用手动添加修改
        public List<OrderItem> Items { get; set; }

    }
}
