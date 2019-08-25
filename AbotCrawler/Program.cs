using System;
using System.IO;
using Abot.Crawler;
using Abot.Poco;

namespace AbotCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 크롤러 인스턴스 생성
                // IWebCrawler crawler = new PoliteWebCrawler();

                // 옵션과 함께 크롤러 인스턴스 생성할 경우
                var crawlConfig = new CrawlConfiguration();
                crawlConfig.CrawlTimeoutSeconds = 5000;
//                crawlConfig.MaxConcurrentThreads = 10;

                crawlConfig.MaxConcurrentThreads = 1;
//                crawlConfig.MaxPagesToCrawl = 10;
                crawlConfig.MaxPagesToCrawl = 50;
                crawlConfig.UserAgentString = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:51.0) Gecko/20100101 Firefox/51.0";
                IWebCrawler crawler = new PoliteWebCrawler(crawlConfig);

                // 이벤트 핸들러 셋업
                crawler.PageCrawlStartingAsync += (s, e) =>
                {
                    Console.WriteLine($"Starting : {e.PageToCrawl}");
                };

                crawler.PageCrawlCompletedAsync += (s, e) =>
                {
                    CrawledPage pg = e.CrawledPage;
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

                    string fn = pg.Uri.Segments[pg.Uri.Segments.Length - 1];
                    string path = @"C:\Users\yjs3694\source\repos\AbotCrawler\AbotCrawler\bin\Debug\crawl.txt";
//                    File.WriteAllText(fn, pg.Content.Text);
//                    File.WriteAllText(directory, pg.Content.Text);

                    doc.LoadHtml(pg.Content.Text);


                    HtmlAgilityPack.HtmlNode singleNode = doc.GetElementbyId("mArticle"); // 태그의 ID=tagId 인것

                    //singleNode 노드의 자식중 a 태그들 *(.) 이 있어야 현재 노드부터 찾는다
                    // HtmlNodeCollection anchors = singleNode.SelectNodes(".//a");

                    //singleNode 노드의 프로퍼티(클래스)값을 리턴한다.
                    //  string className = singleNode.GetAttributeValue("class", "");

//                    HtmlAgilityPack.HtmlNodeCollection article = doc.DocumentNode.SelectNodes("//div[@class='articles']");
//                    HtmlAgilityPack.HtmlNodeCollection article = doc.DocumentNode.SelectNodes("div[@class='hotissue_builtin']");


                    if (singleNode != null)
                    {
//                        File.WriteAllText(directory, singleNode.SelectSingleNode(".//article/div[1]/div[0]/div[2]").InnerText);

                        //*[@id="mArticle"]/div[2]/div[1]/div[2]/div[1]/ol
                        //*[@id="mArticle"]/div[2]/div[1]/div[3]/div[1]/ol

//                        var content = singleNode.SelectSingleNode("//div[2]/div[1]/div[2]/div[1]/ol")?.InnerText;
                        var content = singleNode.SelectSingleNode("//div[2]/div[1]/div[3]/div[1]/ol")?.InnerText;

                        if (content != null)
                        {
                            var bbb = content.Replace("\n\n\n", "");

//                            File.WriteAllText(path, bbb);
//                            File.AppendAllText(path, "\n\n\n");
                            File.AppendAllText(path, bbb);
                            File.AppendAllText(path, "\n\n\n");
                        }
                    }

                    //var hdoc = pg.HtmlDocument; //HtmlAgilityPack HtmlDocument

                    Console.WriteLine("Completed : {0}", pg.Uri.AbsoluteUri);
                };

                // 크롤 시작
                string siteUrl = "http://www.daum.net";

                Uri uri = new Uri(siteUrl);

                for (int i = 0; i < 5; i++)
                {
                    crawler.Crawl(uri);
                    System.Threading.Thread.Sleep(300);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}