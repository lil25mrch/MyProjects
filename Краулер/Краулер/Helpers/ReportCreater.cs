using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
            
            //Документ главной страницы сайта, который мы проверяем
            string uriMain = uri.Scheme + "://" + uri.Host;

            Uri uriStartPage = new Uri(uriMain);

            string contentMain = _webHelper.GetContent(uriMain);
            File.WriteAllText(uriStartPage.Host + "start.txt", contentMain);
            HtmlParser xDocStart = new HtmlParser();
            IHtmlDocument htmlDocumentSP = xDocStart.ParseDocument(contentMain);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            
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

            // TODO Число внутренних ссылок, которые на странице встречаются более 1 раза (в связке URL+анкор)

          
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////// START

            if (domain != uriMain) {
          
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
       
            
           
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var links = _htmlDocumentParser.TagCount("a", "href", htmlDocument);
            var linksDiff = _htmlDocumentParser.TagDiff("a", "href", htmlDocument, htmlDocumentSP, content, contentMain);
            var srcImg = _htmlDocumentParser.TagCount("img", "src", htmlDocument);
            var srcImgDiff = _htmlDocumentParser.TagDiff("img", "src", htmlDocument, htmlDocumentSP, content, contentMain);
            var bgImageUrl = _htmlDocumentParser.RegulStyle("$@\"background-image:(\\s*)url\"", htmlDocument, contentMain);
            
            int h2Count = _htmlDocumentParser.CountHNumforMessage("h2", htmlDocument);
            int h3Count = _htmlDocumentParser.CountHNumforMessage("h3", htmlDocument);
            int h4Count = _htmlDocumentParser.CountHNumforMessage("h4", htmlDocument);
            int h5Count = _htmlDocumentParser.CountHNumforMessage("h5", htmlDocument);
            int h6Count = _htmlDocumentParser.CountHNumforMessage("h6", htmlDocument);

            int title = _htmlDocumentParser.TagCount("[title]", "title", htmlDocument);
            var titrleAverage = _htmlDocumentParser.AverageValue("[title]", "title", htmlDocument);
            var tableDiff = _htmlDocumentParser.TagDiff("[table]", "table", htmlDocument, htmlDocumentSP, content, contentMain);
 
            
            var htmlLang = _htmlDocumentParser.HtmlLangSearch("html", "xml:lang", htmlDocument);

            _htmlDocumentParser.LinksOnPage("instagram", htmlDocument);
            _htmlDocumentParser.LinksOnPage("twitter", htmlDocument);
            _htmlDocumentParser.LinksOnPage("facebook", htmlDocument);
            _htmlDocumentParser.LinksOnPage("youtube", htmlDocument);
            _htmlDocumentParser.LinksOnPage("vk", htmlDocument);
            _htmlDocumentParser.LinksOnPage("google", htmlDocument);
            
            Dictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>();
            dictionary.Add("The number of all links", links);
            dictionary.Add("The number of all links without those on the main page", linksDiff);
            dictionary.Add("Number of tags <img src> ", srcImg);
            dictionary.Add("The number of <img src> tags on the page, excluding those on the main page", srcImgDiff);
            dictionary.Add("Number of style <background-image> had url on the start page is", bgImageUrl);
            //dictionary.Add("The number of <> tags on the page, excluding those on the main page", bgImageUrl);
            dictionary.Add("The number of h2 headers on the start page is", h2Count);
            dictionary.Add("The number of h3 headers on the start page is", h3Count);
            dictionary.Add("The number of h4 headers on the start page is", h4Count);
            dictionary.Add("The number of h5 headers on the start page is", h5Count);
            dictionary.Add("The number of h6 headers on the start page is", h6Count);
            dictionary.Add("The value of <html lang> is", htmlLang);
            dictionary.Add("Number of tags <title> ", title);
            dictionary.Add("The average length of the tag <title> value is", titrleAverage);
            dictionary.Add("Number of tags <table> ", tableDiff);
            /*dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );
            dictionary.Add("", );*/
       
            foreach (KeyValuePair<string, dynamic> keyValue in dictionary) {
                Console.WriteLine(keyValue.Key + " = " + keyValue.Value);
            }
            
            
            Console.Read();
        }
    }
}