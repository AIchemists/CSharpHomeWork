using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ConsoleApp6
{
    public class OrderforDB
    {
        [Key]
        public int OrderforDBId { get; set; }//自动识别为主键
        [Required]
        public string orderNo { get; set; }
        [Required]
        public string clientName { get; set; }
        public string dateTime { get; set; }
        public double total { get; set; }
        public List<ItemforDB> ItemforDBs { get; set; } //一对多关联
    }

    public class ItemforDB
    {
        [Key]
        public int ItemforDBId { get; set; }//自动识别为主键
        [Required]
        public string productName { get; set; }
        public double price { get; set; }
        public int count { get; set; }
        public double total { get; set; }
        public int OrderforDBId { get; set; } //自动识别为外键
        public OrderforDB OrderforDB { get; set; }  //多对一关联
    }
}
