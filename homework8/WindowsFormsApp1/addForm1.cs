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
    public partial class addForm1 : Form
    {   
        public Order order = new Order(null,new Client(),new DateTime(),null);
        public double total = 0;
        public bool isSuccess;
        public addForm1()
        {
            InitializeComponent();
            
            this.orderNoBox.DataBindings.Add("Text",order,"OrderNo");
           this.clientNameBox.DataBindings.Add("Text", order.Clients, "Name");
            order.OrderDate = DateTime.UtcNow;
            order.Total = 0;
            bindingSource1.DataSource = order.OrderItemList;
        }

        private void addItem_click(object sender, EventArgs e)
        {          
            addForm2 form3 = new addForm2();
            form3.ShowDialog();
            if (form3.isSuccess == true)
            {
                warning.Text = "   ";
                OrderItem orderitem = form3.orderItem;
                order.AddItem(orderitem);              
                this.bindingSource1.ResetBindings(false);
                totalMoney.Text = $"总金额：{total+=orderitem.Total}"; 
            }
        }

        private void addorder_click(object sender, EventArgs e)
        {
            warning.Text = $"   ";
            if (order.OrderNo != null
                &&order.Clients.Name!= null
                && order.OrderNo != ""
                 && order.Clients.Name != ""
                && order.OrderNo[0].ToString() != " "
                && order.Clients.Name[0].ToString() != " ")
            {
                isSuccess = true;               
                this.Close();
            }
            else
            {
                warning.Text = "请输入正确的记录！";
            }
        }


        private void cancel_click(object sender, EventArgs e)
        {
            this.isSuccess = false;
            this.Close();
        }

        private void deleteItem_click(object sender, EventArgs e)
        {
            warning.Text = $"   ";
            if (bindingSource1.Current != null)
            {
                Order currentOrder = bindingSource1.Current as Order;
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
            else warning.Text = $"未选中订单。";
        }
    }
}
