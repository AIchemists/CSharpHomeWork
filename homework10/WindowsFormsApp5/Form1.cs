using System;
using System.Collections.Concurrent;
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
    public partial class Form1 : Form                 //因为一开始在ui界面显示爬行过程的方式用的是textbox.text=xxx这种手动刷新的方式，所以把爬行过程放在了form里。。
    {                                                   //用数据绑定的话无法实时更新爬行信息
        private int count = 0;                             //记录爬取页面数
        private int crawlErrorCount = 0;
        private int crawlSuccessCount = 0;
        public ConcurrentQueue<string> pending = new ConcurrentQueue<string>();
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
            pending = new ConcurrentQueue<string>();
            pending.Enqueue(crawler.Url);
            crawlingPocess += "开始爬行了.... \r\n";
            textBox2.Text = crawlingPocess;
            List<Task> tasks = new List<Task>();
            while (count < crawler.maxPage)
            {
                string current = null;
                while (!pending.TryDequeue(out current))                
                {
                    if (tasks.Count < this.crawler.maxPage)
                        continue;
                    else break;
                }
                count++;
                int index = count;
                Task<string> task = Task.Run(() => DownloadandParse(current,index));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            warning.Text = "起始网站爬取完成！\r\n" + $"成功:{crawlSuccessCount},失败：{crawlErrorCount}";
            crawlingPocess += "爬行完成";
            textBox2.Text = crawlingPocess;
        }

        public string DownloadandParse(string url,int index)
        {
            try
            {
                System.Net.WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string html = webClient.DownloadString(url);
                string fileName = index.ToString();
                File.WriteAllText(fileName, html, Encoding.UTF8);
                crawlSuccessCount++;
                crawlingPocess += ("爬行页面!\r\n" + url + "\r\n");
                crawlingPocess += "爬行结束\r\n\r\n";
                textBox2.Text = crawlingPocess;
                Parse(html, url);
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
                count = 0;
                if (thread != null)
                {
                    thread.Abort();
                }
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
