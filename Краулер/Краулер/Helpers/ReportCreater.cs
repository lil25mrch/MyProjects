using System;
using System.Collections.Generic;
using System.IO;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Краулер.Helpers {
    public class ReportCreater {
        private readonly HtmlDocumentParser _htmlDocumentParser = new HtmlDocumentParser();
        private readonly WebHelper _webHelper = new WebHelper();

        public void PageParse(string domain) {
            //Документ сайта, который мы проверяем
            Uri uri = new Uri(domain);
            string uriHost = uri.Host;
            string content = _webHelper.GetContent(domain);
            File.WriteAllText(uri.Host + ".txt", content);
            HtmlParser xDoc = new HtmlParser();
            IHtmlDocument htmlDocument = xDoc.ParseDocument(content);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            //Документ главной страницы сайта, который мы проверяем
            string uriStart = uri.Scheme + "://" + uri.Host;

            Uri uriStartPage = new Uri(uriStart);

            string contentStart = _webHelper.GetContent(uriStart);
            File.WriteAllText(uriStartPage.Host + "start.txt", contentStart);
            HtmlParser xDocStart = new HtmlParser();
            IHtmlDocument htmlDocumentSP = xDocStart.ParseDocument(contentStart);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            //число всех ссылок на странице
            int AllaHref = 0;
            foreach (IElement elementAllaHref in htmlDocument.QuerySelectorAll("a")) {
                string allaHref = elementAllaHref.GetAttribute("href");
                AllaHref++;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////
            var srcImg = _htmlDocumentParser.TagCount("img", "src", htmlDocument, htmlDocumentSP);
            
            
            
            //////////////////////////////////////////////////////////////////////////////////////
           

            //число стилей "background-image: url" 

            /*int bgurl = 0;
            string bgurlReg = contentStart;
            Regex regex = new Regex(@"background-image: url");
            MatchCollection matches = regex.Matches(bgurlReg);
            foreach (Match match in matches)
            {
                bgurl++;
            }*/
            /*
            int bgurl = 0;
            foreach (IElement elementBgImgSP in htmlDocument.QuerySelectorAll("img"))
            {
                string countSrc = elementBgImgSP.GetAttribute("src");
                if (srcList.Contains(countSrc))
                {
                    srcSP++;
                }
            }
           
            string bgurlReg = contentStart;
            Regex regex = new Regex(@"background-image: url");
            MatchCollection matches = regex.Matches(bgurlReg);
            foreach (Match match in matches)
            {
                bgurl++;
            }
            */
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //число внутренних ссылок <a href> у которых вместо анкора <img>
            int aHrefImg = 0;
            HashSet<string> aHrefImgList = new HashSet<string>();
            foreach (IElement elementaHrefImg in htmlDocument.QuerySelectorAll("a")) {
                string countaHrefImg = elementaHrefImg.GetAttribute("href");
                if (string.IsNullOrWhiteSpace(countaHrefImg)) {
                    continue;
                }

                if ((countaHrefImg.StartsWith("/") || countaHrefImg.Contains(uriHost)) && countaHrefImg.Contains("img")) {
                    aHrefImg++;
                    aHrefImgList.Add(countaHrefImg);
                }
            }

            int aHrefImgSP = 0;
            foreach (IElement elementaHrefSP in htmlDocumentSP.QuerySelectorAll("a")) {
                string aHrefSP = elementaHrefSP.GetAttribute("href");
                if (aHrefImgList.Contains(aHrefSP)) {
                    aHrefImgSP++;
                }
            }

            //Общее число внутренних ссылок на странице 
            //Общее число уникальных внутренних ссылок на странице (по URL)

            int aInSite = 0;
            int aMoreOne = 0;

            HashSet<string> UniclistAInSyte = new HashSet<string>();
            HashSet<string> CloneAInSyte = new HashSet<string>();

            foreach (IElement elementaInSite in htmlDocument.QuerySelectorAll("a")) {
                string countaHrefImg = elementaInSite.GetAttribute("href");
                if (string.IsNullOrWhiteSpace(countaHrefImg)) {
                    continue;
                }

                if (countaHrefImg.StartsWith("/") || countaHrefImg.Contains(uriHost)) {
                    aInSite++;
                    if (UniclistAInSyte.Contains(countaHrefImg) != true) {
                        UniclistAInSyte.Add(countaHrefImg);
                    } else {
                        CloneAInSyte.Add(countaHrefImg); //Число внутренних ссылок, которые на странице встречаются более 1 раза(по URL)
                        aMoreOne++;
                    }
                }
            }

            //Число внутренних ссылок, которые на странице встречаются более 1 раза (в связке URL+анкор)

            //Значение параметра <html lang="">, или пометка об его отсутствии
            foreach (IElement elementHtmlLang in htmlDocument.QuerySelectorAll("html")) {
                string countHtmlLang = elementHtmlLang.GetAttribute("xml:lang");
                if (countHtmlLang == null) {
                    Console.WriteLine("Нет присвоенного значения < html lang >");
                } else {
                    Console.WriteLine("Значение параметра < html lang > = {0}", countHtmlLang);
                }
            }

            //общее количество атрибутов title на странице
            int title = 0;
            int TitleLength = 0;
            foreach (IElement elementTitle in htmlDocument.QuerySelectorAll("[title]")) {
                string countTitle = elementTitle.GetAttribute("title");
                TitleLength += countTitle.Length;
                title++;
            }

            //средняя длина значения атрибутов title на странице
            decimal avlTitle = TitleLength / title;
            Console.WriteLine(Math.Ceiling(avlTitle));

            //наличие ссылки на каждую из указанных соцсетей
            _htmlDocumentParser.LinksForMessag("instagram", htmlDocument);
            _htmlDocumentParser.LinksForMessag("twitter", htmlDocument);
            _htmlDocumentParser.LinksForMessag("facebook", htmlDocument);
            _htmlDocumentParser.LinksForMessag("youtube", htmlDocument);
            _htmlDocumentParser.LinksForMessag("vk", htmlDocument);
            _htmlDocumentParser.LinksForMessag("google", htmlDocument);

            //общее количество атрибутов table на странице
            int table = 0;
            HashSet<string> listTable = new HashSet<string>();
            foreach (IElement elementTable in htmlDocument.QuerySelectorAll("[table]")) {
                string countTable = elementTable.GetAttribute("table");
                listTable.Add(countTable);
                table++;
            }

            int tableSP = 0;
            foreach (IElement elementTableSP in htmlDocumentSP.QuerySelectorAll("[table]")) {
                string countTableSP = elementTableSP.GetAttribute("table");
                if (listTable.Contains(countTableSP)) {
                    tableSP++;
                }
            }

            //число заголовков h2-h6 (отдельно)
            int h2Count = _htmlDocumentParser.CountHNumforMessage("h2", htmlDocument);
            int h3Count = _htmlDocumentParser.CountHNumforMessage("h3", htmlDocument);
            int h4Count = _htmlDocumentParser.CountHNumforMessage("h4", htmlDocument);
            int h5Count = _htmlDocumentParser.CountHNumforMessage("h5", htmlDocument);
            int h6Count = _htmlDocumentParser.CountHNumforMessage("h6", htmlDocument);

            Console.WriteLine("Количество заголовков h2 равно {0}", h2Count);
            Console.WriteLine("Количество заголовков h3 равно {0}", h3Count);
            Console.WriteLine("Количество заголовков h4 равно {0}", h4Count);
            Console.WriteLine("Количество заголовков h5 равно {0}", h5Count);
            Console.WriteLine("Количество заголовков h6 равно {0}", h6Count);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////// START

            if (domain != uriStart) {
                //int diffSrc = src - srcSP; //число тегов <img src>	
                //число стилей "background-image: url" 
                /*
                int diffh2Count = h2Count - h2CountSR;         //число заголовков h2-h6 (отдельно)
                int diffh3Count = h3Count - h3CountSR;
                int diffh4Count = h4Count - h/4CountSR;
                int diffh5Count = h5Count - h5CountSR;
                int diffh6Count = h6Count - h6CountSR;
                */

                int diffAHrefImg = aHrefImg - aHrefImgSP; //число внутренних ссылок <a href> у которых вместо анкора <img>
                int diffTable = table - tableSP; //Число тегов table на странице, исключая число table на главной странице

                Console.WriteLine(diffTable);
            }

            Console.WriteLine("Count all a href = {0}", AllaHref);
            
            //Console.WriteLine("Count background-image: url = {0}", bgurl);
            Console.WriteLine("Count a href у которых вместо анкора img = {0}", aHrefImg);
            Console.WriteLine("Общее число внутренних ссылок на странице = {0}", aInSite);
            Console.WriteLine("Количество уникальных внутренних ссылок на странице: {0}", UniclistAInSyte.Count);
            Console.WriteLine("Количество внутренних ссылок, встречающихся более 1 раза на странице: {0}", CloneAInSyte.Count);
            Console.WriteLine("Count title = {0}", title);

            /*
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            dictionary.Add("number of tags <img src> = ", src);
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            */
            
            Console.Read();
        }
    }
}