using System;
using System.Collections.Generic;
using System.IO;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Краулер.Helpers {
    public class ReportCreater {
        private readonly HtmlDocumentParser _htmlDocumentParser = new HtmlDocumentParser();
        private readonly WebHelper _webHelper = new WebHelper();

        public string PageParse(string domain) {
            //Start Page
            Uri uri = new Uri(domain);
            string uriHost = uri.Host;
            string content = _webHelper.GetContent(domain);
            File.WriteAllText(uri.Host + ".txt", content);
            HtmlParser xDoc = new HtmlParser();
            IHtmlDocument htmlDocument = xDoc.ParseDocument(content);

            //Main Page
            string uriMain = uri.Scheme + "://" + uri.Host;

            Uri uriStartPage = new Uri(uriMain);

            string contentMain = _webHelper.GetContent(uriMain);
            File.WriteAllText(uriStartPage.Host + "start.txt", contentMain);
            HtmlParser xDocStart = new HtmlParser();
            IHtmlDocument htmlDocumentMainPage = xDocStart.ParseDocument(contentMain);

            var links = _htmlDocumentParser.Count("a", "href", htmlDocument);
            var linksDiff = _htmlDocumentParser.CountDiff("a", "href", htmlDocument, htmlDocumentMainPage, content,
                                                          contentMain);
            var srcImg = _htmlDocumentParser.Count("img", "src", htmlDocument);
            var srcImgDiff = _htmlDocumentParser.CountDiff("img", "src", htmlDocument, htmlDocumentMainPage, content,
                                                           contentMain);
            var bgImageUrl = _htmlDocumentParser.RegulStyle("\"background-image:(\\s*)url", content);

            int h2Count = _htmlDocumentParser.Count("h2", "h2", htmlDocument);
            int h3Count = _htmlDocumentParser.Count("h3", "h3", htmlDocument);
            int h4Count = _htmlDocumentParser.Count("h4", "h4", htmlDocument);
            int h5Count = _htmlDocumentParser.Count("h5", "h5", htmlDocument);
            int h6Count = _htmlDocumentParser.Count("h6", "h6", htmlDocument);

            int h2CountDiff = _htmlDocumentParser.CountDiff("h2", "h2", htmlDocument, htmlDocumentMainPage, content,
                                                            contentMain);
            int h3CountDiff = _htmlDocumentParser.CountDiff("h3", "h3", htmlDocument, htmlDocumentMainPage, content,
                                                            contentMain);
            int h4CountDiff = _htmlDocumentParser.CountDiff("h4", "h4", htmlDocument, htmlDocumentMainPage, content,
                                                            contentMain);
            int h5CountDiff = _htmlDocumentParser.CountDiff("h5", "h5", htmlDocument, htmlDocumentMainPage, content,
                                                            contentMain);
            int h6CountDiff = _htmlDocumentParser.CountDiff("h6", "h6", htmlDocument, htmlDocumentMainPage, content,
                                                            contentMain);

            var linkWithAncorImg = _htmlDocumentParser.CountSpecificLink("a", "href", "img", htmlDocument);
            var linkWithAncorImgDiff = _htmlDocumentParser.CountSpecificLinkDiff("a", "href", "img", htmlDocument, htmlDocumentMainPage,
                                                                                 content, contentMain);

            var countInternalLinks = _htmlDocumentParser.CountInternalLinks(htmlDocument, uriHost);

            var notUnicLink = _htmlDocumentParser.NonUniqueInternalLinks(htmlDocument, uriHost);
            var uniqIntLink = _htmlDocumentParser.UniqueInternalLinks(htmlDocument, uriHost);

            int title = _htmlDocumentParser.Count("[title]", "title", htmlDocument);
            var titrleAverage = _htmlDocumentParser.AverageValue("[title]", "title", htmlDocument);
            var tableDiff = _htmlDocumentParser.CountDiff("[table]", "table", htmlDocument, htmlDocumentMainPage, content,
                                                          contentMain);
            var htmlLang = _htmlDocumentParser.FindAttributeValue("html", "xml:lang", htmlDocument);

            string instagram = _htmlDocumentParser.FindSpecificLink("a", "href", "instagram", htmlDocument);
            string twitter = _htmlDocumentParser.FindSpecificLink("a", "href", "twitter", htmlDocument);
            string facebook = _htmlDocumentParser.FindSpecificLink("a", "href", "facebook", htmlDocument);
            string youtube = _htmlDocumentParser.FindSpecificLink("a", "href", "youtube", htmlDocument);
            string vk = _htmlDocumentParser.FindSpecificLink("a", "href", "vk", htmlDocument);
            string google = _htmlDocumentParser.FindSpecificLink("a", "href", "google", htmlDocument);

            //dictionary
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("The number of all links", links.ToString());
            dictionary.Add("The number of all links without those on the main page", linksDiff.ToString());

            dictionary.Add("Number of links that contain Img in the anchor", linkWithAncorImg.ToString());
            dictionary.Add("Number of links that contain Img in the anchor, excluding those on the main page", linkWithAncorImgDiff.ToString());

            dictionary.Add("Number of all internal links", countInternalLinks.ToString());

            dictionary.Add("Number of unique internal links", uniqIntLink.ToString());
            dictionary.Add("Number of not unique internal links", notUnicLink.ToString());

            dictionary.Add("The value of <html lang> is", htmlLang);
            dictionary.Add("Number of tags <img src> ", srcImg.ToString());
            dictionary.Add("Number of <img src> tags on the page, excluding those on the main page", srcImgDiff.ToString());
            dictionary.Add("Number of style <background-image> had url on the start page is", bgImageUrl.ToString());
            dictionary.Add("Number of tags <title> ", title.ToString());
            dictionary.Add("The average length of the tag <title> value is", titrleAverage.ToString());
            dictionary.Add("Number of tags <table> ", tableDiff.ToString());

            dictionary.Add("The number of h2 headers on the start page is", h2Count.ToString());
            dictionary.Add("The number of h3 headers on the start page is", h3Count.ToString());
            dictionary.Add("The number of h4 headers on the start page is", h4Count.ToString());
            dictionary.Add("The number of h5 headers on the start page is", h5Count.ToString());
            dictionary.Add("The number of h6 headers on the start page is", h6Count.ToString());
            dictionary.Add("The number of h2 headers on the page, excluding those on the main page", h2CountDiff.ToString());
            dictionary.Add("The number of h3 headers on the page, excluding those on the main page", h3CountDiff.ToString());
            dictionary.Add("The number of h4 headers on the page, excluding those on the main page", h4CountDiff.ToString());
            dictionary.Add("The number of h5 headers on the page, excluding those on the main page", h5CountDiff.ToString());
            dictionary.Add("The number of h6 headers on the page, excluding those on the main page", h6CountDiff.ToString());

            dictionary.Add("instagram", instagram);
            dictionary.Add("twitter", twitter);
            dictionary.Add("facebook", facebook);
            dictionary.Add("youtube", youtube);
            dictionary.Add("vk", vk);
            dictionary.Add("google", google);

            string s = "";
            foreach (KeyValuePair<string, string> keyValue in dictionary) {
                s += keyValue.Key + " = " + keyValue.Value + "\r\n";
            }

            return s;
        }
    }
}