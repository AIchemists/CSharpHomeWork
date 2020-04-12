using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    [Serializable]
    public class Product
    {
        public string Name { get; set; }
        public double Price {get;set;}                  //商品价格
        public Product(string name,double price)
        {
            this.Name = name;
            this.Price = price;
        }

        public Product()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
