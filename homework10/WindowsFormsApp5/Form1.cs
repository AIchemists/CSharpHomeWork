using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        private int count = 0;
        private int pagecount = 0;                                 //记录爬取页面数
        private int crawlErrorCount = 0;
        private int crawlSuccessCount = 0;
        public Queue<string> pending = new Queue<string>();
       public Queue<string> downloadPending = new Queue<string>();
        public static readonly string urlParseRegex = @"^(?<site>https?://(?<host>[\w\d.]+)(:\d+)?($|/))([\w\d]+/)*(?<file>[^#?]*)";
        public SimpleCrawler crawler = new SimpleCrawler();
        public string crawlingPocess { get; set; }
        Thread thread = null;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            textBox1.Text = "http://www.cnblogs.com/dstang2000/";
            crawlingPocess = "";
            textBox3.DataBindings.Add("Text",crawler,"maxPage");
        }
        private void Crawl()
        {
            crawlingPocess += "开始爬行了.... \r\n";
            textBox2.Text = crawlingPocess;
            while (pending.Count > 0 && count < crawler.maxPage)
            {
                string current = null;
                string html="";
                int downloadCount = pending.Count;
                while (pending.Count > 0&&count<crawler.maxPage)
                {
                    current = pending.Dequeue();
                    Task<string> task = Task.Run(() => DownLoad(current));
                    count++;
                  Thread.Sleep(200);                                //不知道为什么下载时会随机出现下面这一句，而且有时候数量也不对
                             //文件“C:\Users\Deng\source\repos\WindowsFormsApp5\WindowsFormsApp5\bin\Debug\6”正由另一进程使用，因此该进程无法访问此文件。
                }
                while (downloadPending.Count<downloadCount && crawlSuccessCount+crawlErrorCount<crawler.maxPage)
                { continue; }

                if(count<crawler.maxPage)
                {
                    while (downloadPending.Count>0)
                    {
                        html = downloadPending.Dequeue();
                        if (html == "") { crawler.urls[current] = true;  }
                        else
                        {

                            crawler.urls[current] = true;
                            bool isHtml;
                            if (html.Length >= 15)
                                isHtml = html.Substring(0, 15).ToLower() == "<!doctype html>"; //判断是否为html
                            else
                                isHtml = false;
                            if (isHtml)                                  //只爬取html文本
                                Parse(html, current);//解析,并加入新的链接
                        }
                    }
                }
            }
            warning.Text = "起始网站爬取完成！\r\n" + $"成功:{crawlSuccessCount},失败：{crawlErrorCount}";
            crawlingPocess += "爬行完成";
            textBox2.Text = crawlingPocess;
        }

        public string DownLoad(string url)
        {
            try
            {
                System.Net.WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string html = webClient.DownloadString(url);
                string fileName = count.ToString();
                File.WriteAllText(fileName, html, Encoding.UTF8);
                crawlSuccessCount++;
                downloadPending.Enqueue(html);                      //将url加入已下载队列中（html待解析）
                crawlingPocess += ("爬行页面!\r\n" + url + "\r\n");
                crawlingPocess += "爬行结束\r\n\r\n";
                textBox2.Text = crawlingPocess;
                return html;
            }
            catch (Exception ex)
            {
                crawlingPocess += ("爬行页面!\r\n" + url + "\r\n");
                crawlingPocess += (ex.Message);
                crawlingPocess += "\r\n";
                crawlingPocess += "爬行失败\r\n\r\n"; 
                textBox2.Text = crawlingPocess;
                crawlErrorCount++;
                downloadPending.Enqueue("");
                return "";
            }
        }

        private void Parse(string html, string current)
        {
            string strRef = @"(href|HREF)\s*=\s*[""'](?<url>[^""'#>]+)[""']";
            var matches = new Regex(strRef).Matches(html);
            foreach (Match match in matches)
            {
                string linkUrl = match.Groups["url"].Value;
                if (linkUrl == null || linkUrl == "") continue;
                linkUrl = FixUrl(linkUrl, current);//转绝对路径
                                                   //解析出host和file两个部分，进行过滤
                Match linkUrlMatch = Regex.Match(linkUrl, urlParseRegex);
                string host = linkUrlMatch.Groups["host"].Value;
                string file = linkUrlMatch.Groups["file"].Value;
                if (file == "") file = "index.html";

                if (Regex.IsMatch(host, crawler.HostFilter) && Regex.IsMatch(file, crawler.FileFilter)
                  && !crawler.urls.ContainsKey(linkUrl))
                {
                    pending.Enqueue(linkUrl);
                    crawler.urls.Add(linkUrl, false);
                }
            }
        }
        static private string FixUrl(string url, string baseUrl)
        {
            if (url.Contains("://"))
            {
                return url;
            }
            if (url.StartsWith("//"))
            {
                return "http:" + url;
            }
            if (url.StartsWith("/"))
            {
                Match urlMatch = Regex.Match(baseUrl, urlParseRegex);
                String site = urlMatch.Groups["site"].Value;
                return site.EndsWith("/") ? site + url.Substring(1) : site + url;
            }

            if (url.StartsWith("../"))
            {
                url = url.Substring(3);
                int idx = baseUrl.LastIndexOf('/');
                return FixUrl(url, baseUrl.Substring(0, idx));
            }

            if (url.StartsWith("./"))
            {
                return FixUrl(url.Substring(2), baseUrl);
            }

            int end = baseUrl.LastIndexOf("/");
            return baseUrl.Substring(0, end) + "/" + url;
        }

        private void crawler_start_click(object sender, EventArgs e)
        { 
            string urlForm = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
            crawler.Url = textBox1.Text;
            if (crawler.Url != null && crawler.Url != "" && Regex.IsMatch(crawler.Url, urlForm))
            {
                Match match = Regex.Match(crawler.Url, urlParseRegex);
                if (match.Length == 0) return;
                string host = match.Groups["host"].Value;
                crawler.HostFilter = "^" + host + "$";
                crawler.FileFilter = ".html?$";
                crawlSuccessCount = 0;
                crawlErrorCount = 0;
                pagecount = 0;
                count = 0;
                if (thread != null)
                {
                    thread.Abort();
                }
                pending.Clear();
                downloadPending.Clear();
                pending.Enqueue(crawler.Url);
                crawlingPocess = "";
                textBox2.Text = crawlingPocess;
                warning.Text = "正在爬取...";
                crawler.urls.Clear();
                crawler.urls.Add(crawler.Url, false);
                thread = new Thread(this.Crawl);
                thread.Start();
            }
            else
            {
                warning.Text = "起始Url格式错误";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.textBox2.SelectionStart = this.textBox2.Text.Length;
            this.textBox2.SelectionLength = 0;
            this.textBox2.ScrollToCaret();
        }
    }
}
