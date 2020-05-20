using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.models
{
    public class OrderItem
    {
        [Key]
        public int ItemId { get; set; }
       [Required]
        public string ProductName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Count { get; set; }
        public string OrderId { get; set; }  //外键
    }
}
