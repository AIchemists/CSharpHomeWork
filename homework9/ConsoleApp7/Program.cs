using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp7
{
    public class SimpleCrawler
    {
        public Hashtable urls = new Hashtable();
        private int count = 0;
        private int pagecount = 0;                                 //记录爬取页面数
        public string Url{get;set;}
        static void Main(string[] args)
        {
           /* SimpleCrawler myCrawler = new SimpleCrawler();
            string startUrl = "http://www.cnblogs.com/dstang2000/";
            if (args.Length >= 1) startUrl = args[0];
            myCrawler.urls.Add(startUrl, false);//加入初始页面
            new Thread(myCrawler.Crawl).Start();  
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private void Crawl()
        {
            Console.WriteLine("开始爬行了.... ");
            while (true)
            {
                string current = null;
                foreach (string url in urls.Keys)
                {
                    if ((bool)urls[url]) continue;
                    current = url;
                }

                if (current == null) break;
                Console.WriteLine("爬行" + current + "页面!");
                string html = DownLoad(current); // 下载
                urls[current] = true;
                count++;
                bool isHtml =(html.Length>=7)&&(html.Substring(html.Length - 7) == "</html>"); //判断是否为html
                if (isHtml)                                  //只爬取html文本
                {
                    if (pagecount<1)                          //只爬取起始页面的网页
                    {
                        Parse(html);//解析,并加入新的链接
                        pagecount++;
                    }
                    Console.WriteLine("爬行结束");
                }
                else
                    Console.WriteLine("（非html）爬行结束");  
            }
        }

        public string DownLoad(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string html = webClient.DownloadString(url);
                string fileName = count.ToString();
                File.WriteAllText(fileName, html, Encoding.UTF8);
                return html;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        private void Parse(string html)
        {
            string strRef = @"(href|HREF)[]*=[]*[""'][^""'#>]+[""']";
            MatchCollection matches = new Regex(strRef).Matches(html);
            foreach (Match match in matches)
            {
                strRef = match.Value.Substring(match.Value.IndexOf('=') + 1)
                          .Trim('"', '\"', '#', '>');
                if (strRef.Length == 0) continue;
                if (urls[strRef] == null)
                {   if (strRef.Substring(0, 4) == "http")              //绝对地址
                        urls[strRef] = false;
                    else if (strRef[0] == '/')                         //相对地址
                        urls["https://www.cnblogs.com"+strRef] = false;          //相对地址转绝对地址 
                }
            }
        }
    }
}
