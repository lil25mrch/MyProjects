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
                //int diffTable = table - tableSP; //Число тегов table на странице, исключая число table на главной странице
                
            }

            
            var srcImg = _htmlDocumentParser.TagCount("img", "src", htmlDocument, htmlDocumentSP);
           
            int h2Count = _htmlDocumentParser.CountHNumforMessage("h2", htmlDocument);
            int h3Count = _htmlDocumentParser.CountHNumforMessage("h3", htmlDocument);
            int h4Count = _htmlDocumentParser.CountHNumforMessage("h4", htmlDocument);
            int h5Count = _htmlDocumentParser.CountHNumforMessage("h5", htmlDocument);
            int h6Count = _htmlDocumentParser.CountHNumforMessage("h6", htmlDocument);
            
            var table = _htmlDocumentParser.TagCount("[table]", "table", htmlDocument, htmlDocumentSP);

            var htmlLang = _htmlDocumentParser.HtmlLangSearch("html", "xml:lang", htmlDocument);

            _htmlDocumentParser.LinksForMessag("instagram", htmlDocument);
            _htmlDocumentParser.LinksForMessag("twitter", htmlDocument);
            _htmlDocumentParser.LinksForMessag("facebook", htmlDocument);
            _htmlDocumentParser.LinksForMessag("youtube", htmlDocument);
            _htmlDocumentParser.LinksForMessag("vk", htmlDocument);
            _htmlDocumentParser.LinksForMessag("google", htmlDocument);
            
            Dictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>();
            dictionary.Add("number of tags <img src> = ", srcImg);
            //dictionary.Add("tag <img src> difference between start page and main page", );
            dictionary.Add("", );
            dictionary.Add("The number of h2 headers on the start page is", h2Count);
            dictionary.Add("The number of h3 headers on the start page is", h3Count);
            dictionary.Add("The number of h4 headers on the start page is", h4Count);
            dictionary.Add("The number of h5 headers on the start page is", h5Count);
            dictionary.Add("The number of h6 headers on the start page is", h6Count);
            dictionary.Add("The value of <html lang> is", htmlLang);
            dictionary.Add(" ", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
       
            
            
            
            Console.Read();
        }
    }
}