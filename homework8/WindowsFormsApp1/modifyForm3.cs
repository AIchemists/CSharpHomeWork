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
    
    public partial class modifyForm3 : Form
    {
        public OrderItem item=new OrderItem();
        public bool isSuccess;
        public modifyForm3()
        {
            InitializeComponent();
            
        }
        public modifyForm3(OrderItem o)
        {
            InitializeComponent();
            this.item.Products.Name = o.Products.Name;
            this.item.price = o.price;
            this.item.Number = o.Number;
            textBox1.DataBindings.Add("Text",item.Products,"Name");
            textBox2.DataBindings.Add("Text", item,"price");
            textBox3.DataBindings.Add("Text",item,"Number");

        }

        private void modifyItem_click(object sender, EventArgs e)
        {
            if (item.Products.Name!=null  && item.Products.Name != "" && item.Products.Name[0].ToString() != " ")
            {
                if (item.Number != 0)
                {
                    this.item.Total = this.item.Number * this.item.price;
                    this.isSuccess = true;
                    this.Close();
                }
                else
                    label4.Text = "数量不能为0";
            }
            else { label4.Text = "商品名不能为空"; }
        }

        private void cancel_click(object sender, EventArgs e)
        {
            this.isSuccess = false;
            this.Close();
        }
    }
}
