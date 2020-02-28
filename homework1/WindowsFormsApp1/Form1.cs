using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void oprate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void data1_TextChanged(object sender, EventArgs e)
        {

        }

        private void data2_TextChanged(object sender, EventArgs e)
        {

        }

        private void count_Click(object sender, EventArgs e)
        {
            double x, y;
            if (!double.TryParse(data1.Text, out x)||!double.TryParse(data2.Text,out y))
            {
                result.Text = "请输入合法数据！";
            }
            else
            {
                switch(oprate.Text)
                {
                    case "+": result.Text = $"{ x+y}";break;
                    case "-": result.Text = $"{ x - y}"; break;
                    case "*": result.Text = $"{ x * y}"; break;
                    case "/":
                        if (y == 0)
                            result.Text = "除数不能为0！";
                        else
                        result.Text = $"{ x / y}"; break;
                }
            }
            result.Visible = true;
        }
        private void result_Click(object sender, EventArgs e)
        {
           
        }
    }
}
