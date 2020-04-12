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
    public partial class modifyForm1 : Form
    {
        public Order order = new Order();
       public bool isSuccess;
        public modifyForm1()
        {
            InitializeComponent();
        }
        public modifyForm1(Order order1)
        {
            InitializeComponent();
            order.OrderNo = order1.OrderNo;
            order.Clients.Name = order1.Clients.Name;
            order.OrderDate = order1.OrderDate;
            order.Total =0;
            foreach(OrderItem orderItem in order1.OrderItemList)
            {
                order.OrderItemList.Add(orderItem);
            }
            bindingSource1.DataSource = order.OrderItemList;
            textBox1.DataBindings.Add("Text",order,"OrderNo");
            textBox2.DataBindings.Add("Text", order.Clients, "Name");


        }

        private void ModifyItem_click(object sender, EventArgs e)
        {
            warning.Text = $"   ";
            if (dataGridView1.CurrentCell != null)
            {
                OrderItem currentItem =bindingSource1.Current as OrderItem;
                modifyForm3 form4 = new modifyForm3(currentItem);
                form4.ShowDialog();
                if(form4.isSuccess==true)
                {
                    this.order.OrderItemList[dataGridView1.CurrentCell.RowIndex] = form4.item;
                }
                this.bindingSource1.ResetBindings(false);
            }
            else { warning.Text = "未选中明细"; }
        
        }

        private void AddItem_click(object sender, EventArgs e)
        {
            modifyForm2 form3 = new modifyForm2();
            form3.ShowDialog();
            if (form3.isSuccess == true)
            {
                OrderItem orderitem = form3.orderItem;
                order.AddItem(orderitem);
                this.bindingSource1.ResetBindings(false);
            }
        }

        private void Modify_click(object sender, EventArgs e)
        {
            warning.Text = $"   ";
            if (order.OrderNo != null 
                && order.Clients.Name != null
                 && order.OrderNo != ""
                && order.Clients.Name != ""
                && order.OrderNo[0].ToString()!=" "
                &&order.Clients.Name[0].ToString()!=" ")
            {
                order.Total = 0;
                foreach (OrderItem oi in order.OrderItemList)
                {
                    order.Total += oi.Total;
                }
                isSuccess = true;
                this.Close();
            }
            else
            {
                warning.Text = "请输入正确的记录！";
            }
        }

        private void DeleteItem_click(object sender, EventArgs e)
        {
            warning.Text = $"   ";
            if (bindingSource1.Current != null)
            {
                Order currentOrder = bindingSource1.Current as Order;
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
            else warning.Text = $"未选中明细";
        }

        private void Cancel_click(object sender, EventArgs e)
        {
            isSuccess = false;
            this.Close();
        }
    }
}
