using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    [Serializable]
    public  class Client
    {
        public string Name;
        public string Address;
        public Client(){}
        public Client(string name,string address)
        {
            this.Name = name;
            this.Address = address;
        }
        public override string ToString()
        {
            return  Name + "  地址：" + Address;
        }
    }
}
