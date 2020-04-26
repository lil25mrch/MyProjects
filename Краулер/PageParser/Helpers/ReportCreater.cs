using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using PageParser.Helpers.Interfaces;

namespace PageParser.Helpers {
    public class ReportCreater {
        private readonly IHtmlDocumentParser _htmlDocumentParser;
        private readonly IRegexHelper _regexHelper;
        private readonly IWebHelper _webHelper;

        public ReportCreater(IHtmlDocumentParser htmlDocumentParser, IRegexHelper regexHelper, IWebHelper webHelper) {
            _htmlDocumentParser = htmlDocumentParser;
            _regexHelper = regexHelper;
            _webHelper = webHelper;
        }

        public Dictionary<string, string> PageParse(string domain) {
            
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

            var links = _htmlDocumentParser.GetListAttributesFromSelector("a", "href", htmlDocument);
            var srcImg = _htmlDocumentParser.GetListAttributesFromSelector("img", "src", htmlDocument);
            var title = _htmlDocumentParser.GetListAttributesFromSelector("[title]", "title", htmlDocument);
            var titleAverage = (int) Math.Round(title.Select(e => e.Length).Average());

            var h2Count = _htmlDocumentParser.GetListAttributesFromSelector("h2", "h2", htmlDocument);
            var h3Count = _htmlDocumentParser.GetListAttributesFromSelector("h3", "h3", htmlDocument);
            var h4Count = _htmlDocumentParser.GetListAttributesFromSelector("h4", "h4", htmlDocument);
            var h5Count = _htmlDocumentParser.GetListAttributesFromSelector("h5", "h5", htmlDocument);
            var h6Count = _htmlDocumentParser.GetListAttributesFromSelector("h6", "h6", htmlDocument);

            var bgImageUrl = _regexHelper.RegexList("\"background-image:(\\s*)url", content).Count;

            var linkWithAncorImg = _htmlDocumentParser.GetListAttributesFromSelector("a", "href", htmlDocument)
                .Where(e => !string.IsNullOrWhiteSpace(e) && e.Contains("img"))
                .ToList();

            var listInternalLinks = links.Where(e => !string.IsNullOrWhiteSpace(e) && (e.Contains(uriHost) || e.StartsWith("/"))).ToList();

            var uniqueInternalLinks = listInternalLinks.ToHashSet();
            var nonUniqueInternalLinks = listInternalLinks.Count - uniqueInternalLinks.Count;

            var htmlLang = _htmlDocumentParser.GetListAttributesFromSelector("html", "xml:lang", htmlDocument).ToList().FirstOrDefault() ?? "missing";

            string instagram = links.FirstOrDefault(e => e.Contains("instagram")) ?? "missing";
            string twitter = links.FirstOrDefault(e => e.Contains("twitter")) ?? "missing";
            string facebook = links.FirstOrDefault(e => e.Contains("facebook")) ?? "missing";
            string youtube = links.FirstOrDefault(e => e.Contains("youtube")) ?? "missing";
            string vk = links.FirstOrDefault(e => e.Contains("vk")) ?? "missing";
            string google = links.FirstOrDefault(e => e.Contains("google")) ?? "missing";

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("The number of all links", links.Count.ToString());
            dictionary.Add("Number of links that contain Img in the anchor", linkWithAncorImg.Count.ToString());
            dictionary.Add("Number of all internal links", listInternalLinks.Count.ToString());
            dictionary.Add("Number of unique internal links", uniqueInternalLinks.Count.ToString());
            dictionary.Add("Number of not unique internal links", nonUniqueInternalLinks.ToString());
            dictionary.Add("The value of <html lang> is", htmlLang);
            dictionary.Add("Number of tags <img src> ", srcImg.Count.ToString());
            dictionary.Add("Number of style <background-image> had url on the start page is", bgImageUrl.ToString());
            dictionary.Add("Number of tags <title> ", title.Count.ToString());
            dictionary.Add("The average length of the tag <title> value is", titleAverage.ToString());
            dictionary.Add("The number of h2 headers on the start page is", h2Count.Count.ToString());
            dictionary.Add("The number of h3 headers on the start page is", h3Count.Count.ToString());
            dictionary.Add("The number of h4 headers on the start page is", h4Count.Count.ToString());
            dictionary.Add("The number of h5 headers on the start page is", h5Count.Count.ToString());
            dictionary.Add("The number of h6 headers on the start page is", h6Count.Count.ToString());
            dictionary.Add("instagram", instagram);
            dictionary.Add("twitter", twitter);
            dictionary.Add("facebook", facebook);
            dictionary.Add("youtube", youtube);
            dictionary.Add("vk", vk);
            dictionary.Add("google", google);

            if (!_htmlDocumentParser.IsMainPage(uri.LocalPath, uriStartPage.LocalPath)) {
                var linksExcludingThoseOnTheMainPage =
                    links.Count - links.Intersect(_htmlDocumentParser.GetListAttributesFromSelector("a", "href", htmlDocumentMainPage).ToList()).ToList().Count;
                var srcImgExcludingThoseOnTheMainPage = srcImg.Count - _htmlDocumentParser.GetListAttributesFromSelector("img", "src", htmlDocumentMainPage)
                                                            .Where(e => links.Contains(e))
                                                            .ToList()
                                                            .Count;
                int h2CountDiff = h2Count.Count - _htmlDocumentParser.GetListAttributesFromSelector("h2", "h2", htmlDocumentMainPage).Where(e => links.Contains(e)).ToList().Count;
                var h3CountDiff = h3Count.Count - _htmlDocumentParser.GetListAttributesFromSelector("h3", "h3", htmlDocumentMainPage).Where(e => links.Contains(e)).ToList().Count;
                var h4CountDiff = h4Count.Count - _htmlDocumentParser.GetListAttributesFromSelector("h4", "h4", htmlDocumentMainPage).Where(e => links.Contains(e)).ToList().Count;
                var h5CountDiff = h5Count.Count - _htmlDocumentParser.GetListAttributesFromSelector("h5", "h5", htmlDocumentMainPage).Where(e => links.Contains(e)).ToList().Count;
                var h6CountDiff = h6Count.Count - _htmlDocumentParser.GetListAttributesFromSelector("h6", "h6", htmlDocumentMainPage).Where(e => links.Contains(e)).ToList().Count;
                var tableDiff = _htmlDocumentParser.GetListAttributesFromSelector("[table]", "table", htmlDocument).Count - _htmlDocumentParser
                                    .GetListAttributesFromSelector("[table]", "table", htmlDocumentMainPage)
                                    .Where(e => links.Contains(e))
                                    .ToList()
                                    .Count;
                var linkWithAncorImgDiff = linkWithAncorImg.Count -
                                           linkWithAncorImg.Intersect(_htmlDocumentParser.GetListAttributesFromSelector("a", "href", htmlDocumentMainPage).ToList()).ToList().Count;
                ;

                dictionary.Add("The number of all links without those on the main page", linksExcludingThoseOnTheMainPage.ToString());
                dictionary.Add("Number of links that contain Img in the anchor, excluding those on the main page", linkWithAncorImgDiff.ToString());
                dictionary.Add("Number of <img src> tags on the page, excluding those on the main page", srcImgExcludingThoseOnTheMainPage.ToString());
                dictionary.Add("Number of tags <table> ", tableDiff.ToString());
                dictionary.Add("The number of h2 headers on the page, excluding those on the main page", h2CountDiff.ToString());
                dictionary.Add("The number of h3 headers on the page, excluding those on the main page", h3CountDiff.ToString());
                dictionary.Add("The number of h4 headers on the page, excluding those on the main page", h4CountDiff.ToString());
                dictionary.Add("The number of h5 headers on the page, excluding those on the main page", h5CountDiff.ToString());
                dictionary.Add("The number of h6 headers on the page, excluding those on the main page", h6CountDiff.ToString());
            }

            return dictionary;
        }
    }
}