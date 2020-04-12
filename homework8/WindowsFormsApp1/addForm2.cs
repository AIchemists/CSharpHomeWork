using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleApp6;
namespace WindowsFormsApp1
{
    public partial class addForm2 : Form
    {
       public OrderItem orderItem = new OrderItem(new Product(),0);
        public bool isSuccess;
        public addForm2()
        {
            InitializeComponent(); 
            productNameBox.DataBindings.Add("Text", orderItem.Products, "Name");
            priceBox.DataBindings.Add("Text",orderItem,"price");
            numberBox.DataBindings.Add("Text", orderItem, "Number");
            
        }

        private void add_click(object sender, EventArgs e)
        {
            if (orderItem.Products.Name != null && orderItem.Products.Name != "" && orderItem.Products.Name[0].ToString() != " " )
            {
                if (orderItem.Number != 0)
                {
                    isSuccess = true;
                    orderItem.Total = orderItem.price * orderItem.Number;
                    this.Close();
                }
                else
                    warning.Text = "数量不能为0";
            }
            else { warning.Text = "商品名不能为空"; }
        }

        private void cancel_click(object sender, EventArgs e)
        {
            isSuccess = false;
            this.Close();
        }
    }
}
