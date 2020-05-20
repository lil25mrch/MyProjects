using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using PageParser.Helpers.Interfaces;

namespace PageParser.Helpers {
    public class ReportCreater : IReportCreater {
        private readonly IHtmlDocumentParser _htmlDocumentParser;
        private readonly IRegexHelper _regexHelper;
        private readonly IWebHelper _restHelper;

        public ReportCreater(IHtmlDocumentParser htmlDocumentParser, IRegexHelper regexHelper, IWebHelper restHelper) {
            _htmlDocumentParser = htmlDocumentParser;
            _regexHelper = regexHelper;
            _restHelper = restHelper;
        }

        public async Task<Dictionary<ResultItem, string>> PageParse(string domain) {
            if (!domain.StartsWith("http")) {
                domain = Uri.UriSchemeHttp + Uri.SchemeDelimiter + domain;
            }

            Uri uri = new Uri(domain);

            string contentStartPage = await _restHelper.GetContent(domain);

            string uriHost = uri.Host;
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
            List<string> listH2Header = _htmlDocumentParser.GetListSelectorWithAttribute("h2", "h2", parsedStartPage);
            List<string> listH3Header = _htmlDocumentParser.GetListSelectorWithAttribute("h3", "h3", parsedStartPage);
            List<string> listH4Header = _htmlDocumentParser.GetListSelectorWithAttribute("h4", "h4", parsedStartPage);
            List<string> listH5Header = _htmlDocumentParser.GetListSelectorWithAttribute("h5", "h5", parsedStartPage);
            List<string> listH6Header = _htmlDocumentParser.GetListSelectorWithAttribute("h6", "h6", parsedStartPage);
            bool instagramLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "instagram");
            bool twitterLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "twitter");
            bool facebookLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "facebook");
            bool youtubeLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "youtube");
            bool vkLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "vk");
            bool googleLink = _htmlDocumentParser.PresenceSocialNetworkLink(listLinks, "google");
            
            bool isMainPage = _htmlDocumentParser.IsMainPage(domain);
            var dictionary = new Dictionary<ResultItem, string>();
            
            dictionary.Add(ResultItem.ThisPageIsMain, isMainPage.ToString());
            dictionary.Add(ResultItem.LinksCount, listLinks.Count.ToString());
            dictionary.Add(ResultItem.InternalLinksCount, listInternalLinks.Count.ToString());
            dictionary.Add(ResultItem.UniqueInternalLinksCount, uniqueInternalLinks.Count.ToString());
            dictionary.Add(ResultItem.NorUniqueInternalLinksCount, nonUniqueInternalLinks.ToString());
            dictionary.Add(ResultItem.InternalLinksHasImgAnchor, listInternalLinksWithImgAnchor.Count.ToString());
            dictionary.Add(ResultItem.HtmlLangValue, htmlLangValue);
            dictionary.Add(ResultItem.ImgSrcTagCount, imgSrcTagList.Count.ToString());
            dictionary.Add(ResultItem.TitleTagCount, listTitles.Count.ToString());
            dictionary.Add(ResultItem.AverageTitleTag, titleAverageLength.ToString());
            dictionary.Add(ResultItem.StyleBackgroundImageInUrlCount, numberBackgroundImageWithUrl.ToString());

            dictionary.Add(ResultItem.InstagramLinkExist, instagramLink.ToString());
            dictionary.Add(ResultItem.TwitterLinkExist, twitterLink.ToString());
            dictionary.Add(ResultItem.FacebookLinkExist, facebookLink.ToString());
            dictionary.Add(ResultItem.YoutubeLinkExist, youtubeLink.ToString());
            dictionary.Add(ResultItem.VkLinkExist, vkLink.ToString());
            dictionary.Add(ResultItem.GoogleLinkExist, googleLink.ToString());

            dictionary.Add(ResultItem.H2HeaderCount, listH2Header.Count.ToString());
            dictionary.Add(ResultItem.H3HeaderCount, listH3Header.Count.ToString());
            dictionary.Add(ResultItem.H4HeaderCount, listH4Header.Count.ToString());
            dictionary.Add(ResultItem.H5HeaderCount, listH5Header.Count.ToString());
            dictionary.Add(ResultItem.H6HeaderCount, listH6Header.Count.ToString());

            if (!isMainPage) {
                string contentMainPage = _restHelper.GetContent((uri.Scheme + "://" + uri.Host)).ToString();
                HtmlParser xDocMain = new HtmlParser();
                IHtmlDocument parsedMainPage = xDocMain.ParseDocument(contentMainPage);

                int linksWithoutHomePage =
                    listLinks.Count - listLinks.Intersect(_htmlDocumentParser.GetListSelectorWithAttribute("a", "href", parsedMainPage).ToList()).ToList().Count;
                int imgSrcTagWithoutHomePage = imgSrcTagList.Count - _htmlDocumentParser.GetListSelectorWithAttribute("img", "src", parsedMainPage)
                                                   .Where(e => listLinks.Contains(e))
                                                   .ToList()
                                                   .Count;
                int h2HeaderWithoutHomePage = listH2Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h2", "h2", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                int h3HeaderWithoutHomePage = listH3Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h3", "h3", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                int h4HeaderWithoutHomePage = listH4Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h4", "h4", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                int h5HeaderWithoutHomePage = listH5Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h5", "h5", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                int h6HeaderWithoutHomePage = listH6Header.Count - _htmlDocumentParser.GetListSelectorWithAttribute("h6", "h6", parsedMainPage)
                                                  .Where(e => listLinks.Contains(e))
                                                  .ToList()
                                                  .Count;
                int tableWithoutHomePage = _htmlDocumentParser.GetListSelectorWithAttribute("[table]", "table", parsedStartPage).Count - _htmlDocumentParser
                                               .GetListSelectorWithAttribute("[table]", "table", parsedMainPage)
                                               .Where(e => listLinks.Contains(e))
                                               .ToList()
                                               .Count;
                int internalLinksWithImgAnchorWithoutHomePage = listInternalLinksWithImgAnchor.Count - listInternalLinksWithImgAnchor
                                                                    .Intersect(_htmlDocumentParser.GetListSelectorWithAttribute("a", "href", parsedMainPage).ToList())
                                                                    .ToList()
                                                                    .Count;

                dictionary.Add(ResultItem.LinksCountWithoutMainPageMatches, linksWithoutHomePage.ToString());
                dictionary.Add(ResultItem.InternalLinksHasImgAnchorWithoutMainPageMatches, internalLinksWithImgAnchorWithoutHomePage.ToString());
                dictionary.Add(ResultItem.ImgSrcTagCountWithoutMainPageMatches, imgSrcTagWithoutHomePage.ToString());
                dictionary.Add(ResultItem.TableTagCountWithoutMainPageMatches, tableWithoutHomePage.ToString());

                dictionary.Add(ResultItem.H2HeaderCountWithoutMainPageMatches, h2HeaderWithoutHomePage.ToString());
                dictionary.Add(ResultItem.H3HeaderCountWithoutMainPageMatches, h3HeaderWithoutHomePage.ToString());
                dictionary.Add(ResultItem.H4HeaderCountWithoutMainPageMatches, h4HeaderWithoutHomePage.ToString());
                dictionary.Add(ResultItem.H5HeaderCountWithoutMainPageMatches, h5HeaderWithoutHomePage.ToString());
                dictionary.Add(ResultItem.H6HeaderCountWithoutMainPageMatches, h6HeaderWithoutHomePage.ToString());
            }

            return dictionary;
        }
    }
}