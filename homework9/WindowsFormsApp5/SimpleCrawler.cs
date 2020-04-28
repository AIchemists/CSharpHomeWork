using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    public class SimpleCrawler
    {
        public string HostFilter { get; set; } //主机过滤规则
        public string FileFilter { get; set; } //文件过滤规则
        public Hashtable urls = new Hashtable();
        private int count = 0;
        private int pagecount = 0;                                 //记录爬取页面数
        public int maxPage { get; set; }
        public string Url { get; set; }
    }
}