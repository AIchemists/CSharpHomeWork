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
        public string Name { get; set; }
        public string Address { get; set; }
        public Client(){}
        public Client(string name,string address)
        {
            this.Name = name;
            this.Address = address;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
