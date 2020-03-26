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

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
          

            
            
            var links = _htmlDocumentParser.TagCount("a", "href", htmlDocument);
            var linksDiff = _htmlDocumentParser.TagDiff("a", "href", htmlDocument, htmlDocumentSP, content, contentMain);
            var srcImg = _htmlDocumentParser.TagCount("img", "src", htmlDocument);
            var srcImgDiff = _htmlDocumentParser.TagDiff("img", "src", htmlDocument, htmlDocumentSP, content, contentMain);
            var bgImageUrl = _htmlDocumentParser.RegulStyle("\"background-image:(\\s*)url", content);

            int h2Count = _htmlDocumentParser.TagCount("h2", "h2", htmlDocument);
            int h3Count = _htmlDocumentParser.TagCount("h3", "h3", htmlDocument);
            int h4Count = _htmlDocumentParser.TagCount("h4", "h4", htmlDocument);
            int h5Count = _htmlDocumentParser.TagCount("h5", "h5", htmlDocument);
            int h6Count = _htmlDocumentParser.TagCount("h6", "h6", htmlDocument);
            
            int h2CountDiff = _htmlDocumentParser.TagDiff("h2", "h2", htmlDocument, htmlDocumentSP, content, contentMain);
            int h3CountDiff = _htmlDocumentParser.TagDiff("h3", "h3", htmlDocument, htmlDocumentSP, content, contentMain);
            int h4CountDiff = _htmlDocumentParser.TagDiff("h4", "h4", htmlDocument, htmlDocumentSP, content, contentMain);
            int h5CountDiff = _htmlDocumentParser.TagDiff("h5", "h5", htmlDocument, htmlDocumentSP, content, contentMain);
            int h6CountDiff = _htmlDocumentParser.TagDiff("h6", "h6", htmlDocument, htmlDocumentSP, content, contentMain);

            var linksHasImg = _htmlDocumentParser.InterLinkHasSmth("img", htmlDocument, uriHost);
            var linkHasImgDiff = _htmlDocumentParser.InterLinkHasSmthDiff("img", uriHost, htmlDocument, htmlDocumentSP, content,
                                                                           contentMain);
            var notUnicLink = _htmlDocumentParser.NotUniqInterLink(htmlDocument, uriHost);
            var uniqIntLink = _htmlDocumentParser.UniqInterLink(htmlDocument, uriHost);
            int title = _htmlDocumentParser.TagCount("[title]", "title", htmlDocument);
            var titrleAverage = _htmlDocumentParser.AverageValue("[title]", "title", htmlDocument);
            var tableDiff = _htmlDocumentParser.TagDiff("[table]", "table", htmlDocument, htmlDocumentSP, content, contentMain);
            var htmlLang = _htmlDocumentParser.HtmlLangSearch("html", "xml:lang", htmlDocument);
            
            string instagram = _htmlDocumentParser.LinksOnPage("instagram", htmlDocument);
            string twitter = _htmlDocumentParser.LinksOnPage("twitter", htmlDocument);
            string facebook = _htmlDocumentParser.LinksOnPage("facebook", htmlDocument);
            string youtube = _htmlDocumentParser.LinksOnPage("youtube", htmlDocument);
            string vk = _htmlDocumentParser.LinksOnPage("vk", htmlDocument);
            string google = _htmlDocumentParser.LinksOnPage("google", htmlDocument);

            Dictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>();
            dictionary.Add("The number of all links", links);
            dictionary.Add("The number of all links without those on the main page", linksDiff);
            dictionary.Add("Number of uniq internal links", uniqIntLink);
            dictionary.Add("Number of not uniq internal links", notUnicLink);
            
            
            dictionary.Add("Number of links that contain Img in the anchor", linksHasImg);
            dictionary.Add("Number of links that contain Img in the anchor, excluding those on the main page", linkHasImgDiff);
            dictionary.Add("Number of tags <img src> ", srcImg);
            dictionary.Add("The number of <img src> tags on the page, excluding those on the main page", srcImgDiff);
            dictionary.Add("Number of style <background-image> had url on the start page is", bgImageUrl);
            //dictionary.Add("The number of <> tags on the page, excluding those on the main page", bgImageDiff);
            dictionary.Add("The number of h2 headers on the start page is", h2Count);
            dictionary.Add("The number of h3 headers on the start page is", h3Count);
            dictionary.Add("The number of h4 headers on the start page is", h4Count);
            dictionary.Add("The number of h5 headers on the start page is", h5Count);
            dictionary.Add("The number of h6 headers on the start page is", h6Count);
            dictionary.Add("The number of h2 headers on the page, excluding those on the main page", h2CountDiff);
            dictionary.Add("The number of h3 headers on the page, excluding those on the main page", h3CountDiff);
            dictionary.Add("The number of h4 headers on the page, excluding those on the main page", h4CountDiff);
            dictionary.Add("The number of h5 headers on the page, excluding those on the main page", h5CountDiff);
            dictionary.Add("The number of h6 headers on the page, excluding those on the main page", h6CountDiff);
            dictionary.Add("The value of <html lang> is", htmlLang);
            dictionary.Add("Number of tags <title> ", title);
            dictionary.Add("The average length of the tag <title> value is", titrleAverage);
            dictionary.Add("Number of tags <table> ", tableDiff);

            dictionary.Add("instagram", instagram);
            dictionary.Add("twitter", twitter);
            dictionary.Add("facebook", facebook);
            dictionary.Add("youtube", youtube);
            dictionary.Add("vk", vk);
            dictionary.Add("google", google);
            
            foreach (KeyValuePair<string, dynamic> keyValue in dictionary) {
                Console.WriteLine(keyValue.Key + " = " + keyValue.Value);
            }
            
            
            Console.Read();
        }
    }
}