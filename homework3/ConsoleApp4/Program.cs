using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{   public interface isShape                                 
    {
        double CountArea();
        bool IsLegal();
    }

    public class Rectangle : isShape
    {
        public double height;
        public double width;
        public Rectangle() { }
        public Rectangle(double width, double height)
        {
            this.width = width;
            this.height = height;
        }
        public double CountArea()
        {
            if (height <= 0 || width <= 0)
            {
                Console.WriteLine("非法长方形");
                return 0;
            }
            else
            {
                double area = height * width;
                return area;
            }
        }
        public bool IsLegal()
        {
            return !(height <= 0 || width <= 0);

        }
    }
    public class Square : Rectangle                               //继承长方形
    {
        public double side;
        public Square(double side)
        {
            height = side;
            width = side;
            this.side = side;
        }
    }
    public class Triangle : isShape
    {
        public double a, b, c;
        public Triangle(double a)                   //等边三角形
        {
            this.a = a;
            this.b = a;
            this.c = a;
        }
        public Triangle(double a, double b, double c)  //常规三角形
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
        public double CountArea()
        {
            if (a <= 0 || b <= 0 || c <= 0 || a + b < c || a + c < b || b + c < a)
            {
                Console.WriteLine("非法三角形");
                return 0;
            }
            else
            {
                double p = (a + b + c) / 2;
                double area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
                return area;
            }
        }
        public bool IsLegal()
        {
            return !(a <= 0 || b <= 0 || c <= 0 || a + b < c || a + c < b || b + c < a);
        }
    }
    public class shapeFactory
    {
        public static isShape Createshape(int shape)
        {
            Random rd = new Random();
            switch (shape)
            {
                case 1:
                       Rectangle rectangle = new Rectangle(rd.Next(0,100), rd.Next(0, 100));
                    while(!rectangle.IsLegal())
                    {
                         rectangle = new Rectangle(rd.Next(0, 100), rd.Next(0, 100));
                    }
                    Console.WriteLine($"生成了Rectangle({rectangle.height},{rectangle.width})");
                    return rectangle;
                case 2:
                    Square square = new Square(rd.Next(0, 100));
                    while(!square.IsLegal())
                    {
                        square = new Square(rd.Next(0, 100));
                    }
                    Console.WriteLine($"生成了Square({square.side})");
                    return square;  
                default:
                    Triangle triangle = new Triangle(rd.Next(0, 100), rd.Next(0, 100), rd.Next(0, 100));
                    while (!triangle.IsLegal())
                    {
                        triangle = new Triangle(rd.Next(0, 100), rd.Next(0, 100), rd.Next(0, 100));
                    }
                    Console.WriteLine($"生成了Triangle({triangle.a},{triangle.b},{triangle.c})");
                    return triangle; 
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Random rd = new Random();
            isShape[] shapes = new isShape[10];
            for (int i = 0; i < 10; i++)
                shapes[i] = shapeFactory.Createshape(rd.Next(1,4));         //逐帧调试的时候随机数是不重复的，但是直接运行随机数就会重复，网上说好像是运行速度太快的原因，可以加延时解决
            double sumArea=0;
            foreach (isShape s in shapes)
                sumArea += s.CountArea();
            Console.WriteLine(sumArea);
        }
    }


}
