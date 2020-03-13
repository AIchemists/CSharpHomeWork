using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public class Node<T>                         //泛型结点
    {
        public Node<T> Next { get; set; }
        public T Data { get; set; }
        public Node(T t)
        {
            Next = null;
            Data = t;
        }
    }
    public class GenericList<T>                   //泛型链表
    {
        private Node<T> head;
        private Node<T> tail;
        
        public GenericList()
        {
            tail = head = null;
        }
        public Node<T> Head
        {
            get => head;
        }
        public void Add(T t)
        {
            Node<T> n = new Node<T>(t);
            if(tail==null)
            {
                head = tail = n;
            }
            else
            {
                tail.Next = n;
                tail = n;
            }
        }
        public void Foreach(Action<T> action)
        {
            for (Node<T> x = head; x != null; x = x.Next)
                action(x.Data);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            GenericList<int> list = new GenericList<int>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(i);
            }
            int maximun = -10000, minimun = 10000, sum = 0;
            list.Foreach(data => Console.WriteLine(data));
            list.Foreach(data => { if (data > maximun) maximun = data; });
            list.Foreach(data => { if (data < minimun) minimun = data; });
            list.Foreach(data => sum += data );
            Console.WriteLine($"maximun:{maximun}, minimun:{minimun}, sum:{sum}");
        }
    }
}
