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

namespace ConsoleApp7
{
    public partial class Form1 : Form
    {
        private int count = 0;
        private int pagecount = 0;                                 //记录爬取页面数
        private int crawlErrorCount = 0;
        private int crawlSuccessCount = 0;
        public static readonly string urlParseRegex = @"^(?<site>https?://(?<host>[\w\d.]+)(:\d+)?($|/))([\w\d]+/)*(?<file>[^#?]*)";
        public SimpleCrawler crawler = new SimpleCrawler();
          public string crawlingPocess { get; set; }
        Thread thread = null;


        public Form1()
        {

            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            textBox1.Text= "http://www.cnblogs.com/dstang2000/";
            crawlingPocess = "";
        }

        private void Crawl()
        {
          crawlingPocess+="开始爬行了.... \r\n";
            textBox2.Text = crawlingPocess;
            while (true)
            {
                string current = null;
                foreach (string url in crawler.urls.Keys)
                {
                    if ((bool)crawler.urls[url]) continue;
                    current = url;
                }

                if (current == null) break;
                crawlingPocess += ("爬行页面!\r\n"+current+"\r\n");
                textBox2.Text = crawlingPocess;
                string html = DownLoad(current); // 下载
                if (html == "") { crawler.urls[current] = true; crawlingPocess += "爬行失败\r\n\r\n"; textBox2.Text = crawlingPocess; }
                else
                {
                    crawler.urls[current] = true;
                    count++;
                    bool isHtml;
                    if (html.Length >= 15)
                        isHtml = html.Substring(0, 15).ToLower() == "<!doctype html>"; //判断是否为html
                    else
                        isHtml = false;
                    if (isHtml)                                  //只爬取html文本
                    {
                        if (pagecount < 1)                          //只爬取起始页面的网页
                        {
                            Parse(html,current);//解析,并加入新的链接
                            pagecount++;
                        }
                        crawlingPocess += "爬行结束\r\n\r\n";
                        textBox2.Text = crawlingPocess;
                    }
                    else
                    {
                        crawlingPocess += "（非html）爬行结束\r\n\r\n";
                        textBox2.Text = crawlingPocess;
                    }
                }
            }
            warning.Text = "起始页面爬取完成！\r\n"+$"成功:{crawlSuccessCount},失败：{crawlErrorCount}";
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
                crawlSuccessCount++;
                return html;
            }
            catch (Exception ex)
            {
                crawlingPocess +=(ex.Message);
                crawlingPocess += "\r\n";
                crawlErrorCount++;
                return "";
            }
        }

        private void Parse(string html,string current)
        {
            string strRef = @"(href|HREF)\s*=\s*[""'](?<url>[^""'#>]+)[""']";
            var matches = new Regex(strRef).Matches(html);
            foreach (Match match in matches)
            {
                string linkUrl = match.Groups["url"].Value;
                if (linkUrl == null || linkUrl == "") continue;
                linkUrl = FixUrl(linkUrl,current);//转绝对路径
                if ( !crawler.urls.ContainsKey(linkUrl))
                {
                   crawler. urls[linkUrl]=false;
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
        private void StartCrawl_click(object sender, EventArgs e)
        {  
            string urlForm = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
            crawler.Url = textBox1.Text;
            if (crawler.Url!=null&&crawler.Url!=""&&Regex.IsMatch(crawler.Url, urlForm))
            {
                crawlSuccessCount = 0;
                crawlErrorCount = 0;
                pagecount = 0;
                if (thread != null)
                {
                    thread.Abort();                    
                }
                crawlingPocess = "";
                textBox2.Text = crawlingPocess;
                warning.Text = "正在爬取...";
                crawler.urls.Clear();
                crawler.urls.Add(crawler.Url, false);
                thread =new Thread(this.Crawl);
                thread.Start();             
            }
            else
            {
                warning.Text = "起始Url格式错误";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.textBox2.SelectionStart = this.textBox2.Text.Length;
            this.textBox2.SelectionLength = 0;
            this.textBox2.ScrollToCaret();
        }
    }

}
