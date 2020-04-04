using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        double th1 ;
        double th2 ;
        double per1 ;
        double per2 ;
        double leng;
        public Form1()
        {
            InitializeComponent();
            leftAngle.Maximum = 99;      //滚动条拖不到最大值，上网查了说是maximun=maximun+1-LargeChange，所以在这里将其设为99，在运行时就为90
            leftAngle.Minimum = 0;
            rightAngle.Maximum = 99;
            rightAngle.Minimum = 0;
            for(int i=5;i<=15;i++)
            deep.Items.Add($"{i}");                   //深度
            for (int i = 0; i <=9; i++)
                leftRatio.Items.Add($"0.{i}");           //左长度比例
            for (int i = 0; i <= 9; i++)
                rightRatio.Items.Add($"0.{i}");             //右长度比例
            colourChoose.Items.Add("蓝色");
            colourChoose.Items.Add("红色");
            colourChoose.Items.Add("黑色");
            colourChoose.Items.Add("绿色");
            colourChoose.Items.Add("紫色");
            colourChoose.Items.Add("粉色");
            colourChoose.Items.Add("橙色");
            colourChoose.Items.Add("黄色");
            colourChoose.Items.Add("紫色2");
        }
        private bool errorTest()                     //测试输入数据是否正确
        {
            if (double.TryParse(length.Text, out leng)&&leng>=0)
            {
                errorAlarm.Text = "   ";
            }
            else
            {
                errorAlarm.Text = "请输入正确的长度！";
                return false;
            }
            if(deep.SelectedItem!=null)
            {
                errorAlarm.Text = "   ";
            }
            else
            {
                errorAlarm.Text = "请选择深度！";
                return false;
            }
            if (leftRatio.SelectedItem == null)
            {
                if (double.TryParse(leftRatio.Text.ToString(), out per2)
                    && per2<=1
                    && per2>=0)              
                {
                    errorAlarm.Text = "   ";
                }
                else
                {
                    errorAlarm.Text = "请输入正确的比例！";
                    return false;
                }
            }
            else
            {
                per2 = Convert.ToDouble(leftRatio.SelectedItem.ToString());
            }
            if (rightRatio.SelectedItem == null)
            {
                if (double.TryParse(rightRatio.Text.ToString(), out per1)
                    && per1 <= 1
                    && per1 >= 0)
                {
                    errorAlarm.Text = "   ";
                }
                else
                {
                    errorAlarm.Text = "请输入正确的比例！";
                    return false;
                }
            }
            else
            {
                per1 = Convert.ToDouble(rightRatio.SelectedItem.ToString());
            }
            if (colourChoose.SelectedItem!=null)
            {
                errorAlarm.Text = "   ";
                return true;
            }
            else
            {
                errorAlarm.Text = "请选择颜色！";
                return false;
            }
        }
        private void drawTree(object sender, EventArgs e)
        {
            if (errorTest())                //测试输入
            {
                if (graphics == null)
                    graphics = this.panel1.CreateGraphics();
                drawCayleyTree(Convert.ToInt32(deep.SelectedItem.ToString()),150,300, leng, -Math.PI / 2);
            }
        }

        void drawCayleyTree(int n, double x0, double y0,double leng,double th)
        {
            if (n == 0) return;
            th1 = rightAngle.Value * Math.PI / 180;
            th2 = leftAngle.Value * Math.PI / 180;
            double x1 = x0 + leng * Math.Cos(th);
            double y1 = y0 + leng * Math.Sin(th);
                drawLine(x0, y0, x1, y1);
                drawCayleyTree(n - 1, x1, y1, per1 * leng, th + th1);
                drawCayleyTree(n - 1, x1, y1, per2 * leng, th - th2);
        }
        void drawLine(double x0, double y0, double x1, double y1)
        {
            int i = colourChoose.SelectedIndex;
            Pen[] color = { Pens.Blue,Pens.Red,Pens.Black,Pens.Green,Pens.Purple,Pens.Pink,Pens.Orange,Pens.Yellow,Pens.Violet};
            graphics.DrawLine(color[i],(int)x0,(int)y0,(int)x1,(int)y1);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void leftAngle_ValueChanged(object sender, EventArgs e)
        {
            leftAngleShow.Text = $"{leftAngle.Value}°";
        }

        private void rightAngle_ValueChanged(object sender, EventArgs e)
        {
            rightAngleShow.Text = $"{rightAngle.Value}°";
        }

        private void clear(object sender, EventArgs e)
        {
            panel1.Refresh();          //清除
        }
    }
}
