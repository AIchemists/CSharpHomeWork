using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using ConsoleApp6;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Order> orderList = new List<Order>();
        public bool inquiryState;                                   //查询模式开关
        public List<Order> inquiryResultList;                         //用于查询模式下的修改订单
        public Form1()
        {
            InitializeComponent();
            InquiryChoose.Items.Add("按订单号查询");
            InquiryChoose.Items.Add("按客户名查询");
            InquiryChoose.Items.Add("按商品名查询");
            orderList = OrderService.GetOrderList();
            OrderDataSource.DataSource = orderList;
            inquiryState = false;
            quitInquiry.Enabled = false;
        }

        private void Inquire_Click(object sender, EventArgs e)
        {
            if (InquiryChoose.SelectedItem != null)
            {
                if (inquiryBox.Text == "")
                    warning.Text = "";
                else
                {
                    this.inquiryState = true;                            //进入查询模式
                    quitInquiry.Enabled = true;
                    Add.Enabled = false;                                       //查询模式下无法添加订单
                    switch (InquiryChoose.SelectedIndex)
                    {
                        case 0:
                            if (OrderService.InquireNo(orderList, inquiryBox.Text) != null)
                            {
                                inquiryResultList = orderList.Where(s => s.OrderNo == inquiryBox.Text).ToList();
                                OrderDataSource.DataSource = inquiryResultList; //更新查询结果
                                this.OrderDataSource.ResetBindings(false);
                            }
                            else { OrderDataSource.DataSource = null; ItemDataSource.DataSource = null; } //未找到则显示为空
                            break;
                        case 1:
                            if (OrderService.InquireClientName(orderList, inquiryBox.Text) != null)
                            {
                                inquiryResultList = OrderService.InquireClientName(orderList, inquiryBox.Text);
                                OrderDataSource.DataSource = inquiryResultList;
                                this.OrderDataSource.ResetBindings(false);
                            }
                            else { OrderDataSource.DataSource = null; ItemDataSource.DataSource = null; }
                            break;
                        case 2:
                            if (OrderService.InquireProductName(orderList, inquiryBox.Text) != null)
                            {
                                inquiryResultList = OrderService.InquireProductName(orderList, inquiryBox.Text);
                                OrderDataSource.DataSource = inquiryResultList;
                                this.OrderDataSource.ResetBindings(false);
                            }
                            else { OrderDataSource.DataSource = null; ItemDataSource.DataSource = null; }
                            break;
                    }

                }
            }
            else
                warning.Text = "请选择查询方式";
        }

        private void Add_Click(object sender, EventArgs e)
        {
            warning.Text = $"   ";
            addForm1 form2 = new addForm1();
            form2.ShowDialog();
            if (form2.isSuccess == true)
                if (OrderService.InquireNo(orderList, form2.order.OrderNo) != null)
                    warning.Text = $"已经存在订单{form2.order.OrderNo}";
                else
                {
                    OrderService.addOrder(orderList, form2.order);
                }


            this.OrderDataSource.ResetBindings(false);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (inquiryState == false)                            //非查询模式
            {
                warning.Text = $"   ";
                if (OrderDataSource.Current != null)
                {
                    Order currentOrder = OrderDataSource.Current as Order;
                    OrderService.deleteOrder(orderList, currentOrder.OrderNo);
                    this.OrderDataSource.ResetBindings(false);
                    this.ItemDataSource.ResetBindings(false);
                    if (orderList.Count == 0)             //如果此时orderGrid为空，则itemGrid显示为空
                        ItemDataSource.DataSource = null;
                }
                else warning.Text = $"未选中订单。";
            }
            else                                                    //查询模式
            {
                warning.Text = $"   ";
                if (OrderDataSource.Current != null)
                {
                    Order currentOrder = OrderDataSource.Current as Order;
                    OrderService.deleteOrder(orderList, currentOrder.OrderNo);                   //修改orderList
                    dataGridView1.Rows.Remove(dataGridView1.CurrentRow);              //修改当前inquiryList
                    if (OrderDataSource.Current == null)
                        ItemDataSource.DataSource = null;
                    this.ItemDataSource.ResetBindings(false);
                }
                else warning.Text = $"未选中订单。";
            }
        }

        private void Modify_Click(object sender, EventArgs e)
        {
            if (inquiryState == false)                                         //非查询模式
            {
                warning.Text = $"   ";
                Order newOrder = new Order();
                if (OrderDataSource.Current != null)
                {
                    Order currentOrder = OrderDataSource.Current as Order;
                    modifyForm1 form2 = new modifyForm1(currentOrder);
                    form2.ShowDialog();
                    if (form2.isSuccess == true)
                    {
                        string no = currentOrder.OrderNo;
                        currentOrder = form2.order;
                        OrderService.modifyOrder(orderList, no, currentOrder);                                 //orderService表格修改
                    }
                    this.OrderDataSource.ResetBindings(false);
                }
                else warning.Text = $"未选中订单。";
            }
            else
            {
                string textbox = inquiryBox.Text;
                warning.Text = $"   ";
                Order newOrder = new Order();
                if (OrderDataSource.Current != null)
                {
                    Order currentOrder = OrderDataSource.Current as Order;
                    modifyForm1 form2 = new modifyForm1(currentOrder);
                    form2.ShowDialog();
                    if (form2.isSuccess == true)
                    {
                        string no = currentOrder.OrderNo;
                        currentOrder = form2.order;
                        OrderService.modifyOrder(orderList, no, currentOrder);             //修改orderList
                        inquiryResultList[dataGridView1.CurrentCell.RowIndex] = currentOrder;  //同时修改当前inquiryList
                    }
                    this.OrderDataSource.ResetBindings(false);
                }
                else warning.Text = $"未选中订单。";
            }
        }
        private void DataGridView1_CurrentCellChanged(object sender, EventArgs e)  //orderGrid改变时刷新itemGrid
        {
            if (OrderDataSource.Current != null)
            {
                Order currentOrder = OrderDataSource.Current as Order;
                ItemDataSource.DataSource = currentOrder.OrderItemList;
                this.ItemDataSource.ResetBindings(false);
            }
            else
                this.ItemDataSource.ResetBindings(false);
        }

        /*   private void DeriveIn_Click(object sender, EventArgs e)            
           {
               if(openFileDialog1.ShowDialog()==DialogResult.OK)
               {
                   this.orderService.Import(openFileDialog1.FileName);
                   OrderDataSource.DataSource = orderService.orderList;
                   this.OrderDataSource.ResetBindings(false);

               }
           }*/

        private void DeriveOut_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OrderService.Export(orderList, saveFileDialog1.FileName);
            }
        }

        private void quitInquiry_Click(object sender, EventArgs e)
        {
            inquiryState = false;                               //退出查询模式
            quitInquiry.Enabled = false;
            Add.Enabled = true;
            inquiryBox.Clear();
            OrderDataSource.DataSource = orderList;
            if (inquiryResultList != null)                       //将查询结果表初始化
                inquiryResultList.Clear();
        }
    }
}
