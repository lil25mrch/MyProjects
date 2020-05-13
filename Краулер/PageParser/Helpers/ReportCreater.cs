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
        private readonly IWebHelper _restHelper;

        public ReportCreater(IHtmlDocumentParser htmlDocumentParser, IRegexHelper regexHelper, IWebHelper restHelper) {
            _htmlDocumentParser = htmlDocumentParser;
            _regexHelper = regexHelper;
            _restHelper = restHelper;
        }

        public Dictionary<string, string> PageParse(string domain) {
            if (!domain.StartsWith("http")) {
                domain = Uri.UriSchemeHttp + Uri.SchemeDelimiter + domain;
            }
            Uri uri = new Uri(domain);
            string contentStartPage = _restHelper.GetContent(domain);

            string uriHost = uri.Host;
            File.WriteAllText(uri.Host + ".txt", contentStartPage);
            HtmlParser xDoc = new HtmlParser();
            IHtmlDocument parsedStartPage = xDoc.ParseDocument(contentStartPage);

            List<string> listLinks = _htmlDocumentParser.GetListSelectorWithAttribute("a", "href", parsedStartPage);
            List<string> listInternalLinks = listLinks.Where(e => !string.IsNullOrWhiteSpace(e) && (e.Contains(uriHost) || e.StartsWith("/"))).ToList();
            HashSet<string> uniqueInternalLinks = listInternalLinks.ToHashSet();
            int nonUniqueInternalLinks = listInternalLinks.Count - uniqueInternalLinks.Count;
            List<string> listInternalLinksWithImgAnchor = listInternalLinks.Where(e => !string.IsNullOrWhiteSpace(e) && e.Contains("img")).ToList();
            string htmlLangValue = _htmlDocumentParser.GetListSelectorWithAttribute("html", "xml:lang", parsedStartPage).ToList().FirstOrDefault() ?? "does not exist";
            List<string> imgSrcTagList = _htmlDocumentParser.GetListSelectorWithAttribute("img", "src", parsedStartPage);
            List<string> listTitles = _htmlDocumentParser.GetListSelectorWithAttribute("[title]", "title", parsedStartPage);
            int titleAverageLength = (int) Math.Round(listTitles.Select(e => e.Length).Average());
            int numberBackgroundImageWithUrl = _regexHelper.RegexList("\"background-image:(\\s*)url", contentStartPage).Count;
            var listH2Header = _htmlDocumentParser.GetListSelectorWithAttribute("h2", "h2", parsedStartPage);
            var listH3Header = _htmlDocumentParser.GetListSelectorWithAttribute("h3", "h3", parsedStartPage);
            var listH4Header = _htmlDocumentParser.GetListSelectorWithAttribute("h4", "h4", parsedStartPage);
            var listH5Header = _htmlDocumentParser.GetListSelectorWithAttribute("h5", "h5", parsedStartPage);
            var listH6Header = _htmlDocumentParser.GetListSelectorWithAttribute("h6", "h6", parsedStartPage);
            string instagramLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "instagram");
            string twitterLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "twitter");
            string facebookLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "facebook");
            string youtubeLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "youtube");
            string vkLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "vk");
            string googleLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "google");

            string isPageHome;
            if (_htmlDocumentParser.IsMainPage(domain)) {
                isPageHome = "main";
            } else {
                isPageHome = "start";
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("This page is ", isPageHome);
            dictionary.Add("Number of all links per page: ", listLinks.Count.ToString());
            dictionary.Add("Number of all internal links per page: ", listInternalLinks.Count.ToString());
            dictionary.Add("Number of unique internal links per page: ", uniqueInternalLinks.Count.ToString());
            dictionary.Add("Number of not unique internal links per page: ", nonUniqueInternalLinks.ToString());
            dictionary.Add("The number of internal links which instead of the anchor <img> per page: ", listInternalLinksWithImgAnchor.Count.ToString());
            dictionary.Add("The value of <html lang> is", htmlLangValue);
            dictionary.Add("Number of <img src> tags per page: ", imgSrcTagList.Count.ToString());
            dictionary.Add("Number of tags <title> per page: ", listTitles.Count.ToString());
            dictionary.Add("The average length of the tag <title> value is: ", titleAverageLength.ToString());
            dictionary.Add("Number of style <background-image: url> per page: ", numberBackgroundImageWithUrl.ToString());

            dictionary.Add("Link to the social network instagram ", instagramLink);
            dictionary.Add("Link to the social network twitter ", twitterLink);
            dictionary.Add("Link to the social network facebook ", facebookLink);
            dictionary.Add("Link to the social network youtube", youtubeLink);
            dictionary.Add("Link to the social network vk ", vkLink);
            dictionary.Add("Link to the social network google ", googleLink);

            dictionary.Add("Number of h2 headers per page: ", listH2Header.Count.ToString());
            dictionary.Add("Number of h3 headers per page: ", listH3Header.Count.ToString());
            dictionary.Add("Number of h4 headers per page: ", listH4Header.Count.ToString());
            dictionary.Add("Number of h5 headers per page: ", listH5Header.Count.ToString());
            dictionary.Add("Number of h6 headers per page: ", listH6Header.Count.ToString());

            if (!_htmlDocumentParser.IsMainPage(domain)) {
                string contentMainPage = _restHelper.GetContent((uri.Scheme + "://" + uri.Host));
                HtmlParser xDocMain = new HtmlParser();
                IHtmlDocument parsedMainPage = xDocMain.ParseDocument(contentMainPage);

                var linksWithoutHomePage =
                    listLinks.Count - listLinks.Intersect(_htmlDocumentParser.GetListSelectorWithAttribute("a", "href", parsedMainPage).ToList()).ToList().Count;
                var imgSrcTagWithoutHomePage = imgSrcTagList.Count - _htmlDocumentParser.GetListSelectorWithAttribute("img", "src", parsedMainPage)
                                                   .Where(e => listLinks.Contains(e))
                                                   .ToList()
                                                   .Count;
                int h2HeaderWithoutHomePage = listH2Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h2", "h2", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                var h3HeaderWithoutHomePage = listH3Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h3", "h3", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                var h4HeaderWithoutHomePage = listH4Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h4", "h4", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                var h5HeaderWithoutHomePage = listH5Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h5", "h5", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                var h6HeaderWithoutHomePage = listH6Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h6", "h6", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                var tableWithoutHomePage = _htmlDocumentParser.GetListSelectorWithAttribute("[table]", "table", parsedStartPage).Count - _htmlDocumentParser
                                               .GetListSelectorWithAttribute("[table]", "table", parsedMainPage)
                                               .Where(e => listLinks.Contains(e))
                                               .ToList()
                                               .Count;
                var internalLinksWithImgAnchorWithoutHomePage = listInternalLinksWithImgAnchor.Count - listInternalLinksWithImgAnchor
                                                                    .Intersect(_htmlDocumentParser.GetListSelectorWithAttribute("a", "href", parsedMainPage).ToList())
                                                                    .ToList()
                                                                    .Count;

                dictionary.Add("Number of all links per page, excluding matches to the home page: ", linksWithoutHomePage.ToString());
                dictionary.Add("The number of internal links which instead of the anchor <img> per page, excluding matches to the home page: ",
                               internalLinksWithImgAnchorWithoutHomePage.ToString());
                dictionary.Add("Number of <img src> tags per page, excluding matches to the home page: ", imgSrcTagWithoutHomePage.ToString());
                dictionary.Add("Number of <table> tags per page, excluding matches to the home page: ", tableWithoutHomePage.ToString());

                dictionary.Add("Number of h2 headers per page, excluding matches to the home page: ", h2HeaderWithoutHomePage.ToString());
                dictionary.Add("Number of h3 headers per page, excluding matches to the home page: ", h3HeaderWithoutHomePage.ToString());
                dictionary.Add("Number of h4 headers per page, excluding matches to the home page: ", h4HeaderWithoutHomePage.ToString());
                dictionary.Add("Number of h5 headers per page, excluding matches to the home page: ", h5HeaderWithoutHomePage.ToString());
                dictionary.Add("Number of h6 headers per page, excluding matches to the home page: ", h6HeaderWithoutHomePage.ToString());
            }

            return dictionary;
        }
    }
}