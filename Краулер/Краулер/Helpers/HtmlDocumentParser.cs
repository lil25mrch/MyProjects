using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Краулер.Helpers {
    public class HtmlDocumentParser {
        public List<string> GetListAttributeWithSelector(string selecror, string attribute, IHtmlDocument htmlDoc) {
            List<string> list = new List<string>();
            foreach (IElement element in htmlDoc.QuerySelectorAll(selecror)) {
                string value = element.GetAttribute(attribute);
                list.Add(value);
            }

            return list;
        }

        public int Count(string selector, string attribute, IHtmlDocument htmlDoc) {
            int count = GetListAttributeWithSelector(selector, attribute, htmlDoc).Count;

            return count;
        }

        public int CountDiff(string selector,
                             string attribute,
                             IHtmlDocument htmlDoc,
                             IHtmlDocument htmlDocSP,
                             string contentS,
                             string contentM) {
            int diff;
            List<string> listStartPage = GetListAttributeWithSelector(selector, attribute, htmlDoc);
            if (contentM != contentS) {
                List<string> listMainPage = GetListAttributeWithSelector(selector, attribute, htmlDocSP);
                List<string> listDiff = new List<string>();
                foreach (var element in listMainPage) {
                    if (listStartPage.Contains(element)) {
                        listDiff.Add(element);
                    }
                }

                diff = listStartPage.Count - listDiff.Count;
                return diff;
            } else {
                diff = listStartPage.Count;
            }

            return diff;
        }

        public int AverageValue(string selector, string attribute, IHtmlDocument htmlDoc) {
            List<string> list = GetListAttributeWithSelector(selector, attribute, htmlDoc);
            int averageValue = (int) Math.Round(list.Select(e => e.Length).Average());

            return averageValue;
        }

        public int RegulStyle(string searchTense, string cont) {
            Regex regex = new Regex(searchTense);
            MatchCollection matches = regex.Matches(cont);
            return matches.Count;
        }

        public string FindAttributeValue(string selector, string attribute, IHtmlDocument htmlDoc) {
            List<string> list = GetListAttributeWithSelector(selector, attribute, htmlDoc);
            foreach (var element in list) {
                if (element == null) {
                    return "missing";
                } else {
                    return element;
                }
            }

            return "";
        }

        public List<string> GetInternalLinksList(IHtmlDocument htmlDoc, string uriHost) {
            List<string> list = GetListAttributeWithSelector("a", "href", htmlDoc);

            List<string> links = list.Where(e => !string.IsNullOrWhiteSpace(e) && (e.Contains(uriHost) || e.StartsWith("/"))).ToList();
            return links;
        }

        public int CountInternalLinks(IHtmlDocument htmlDoc, string uriHost) {
            string answer;
            List<string> list = GetInternalLinksList(htmlDoc, uriHost);

            return list.Count;
        }

        public string FindSpecificLink(string selector,
                                       string attribute,
                                       string name,
                                       IHtmlDocument htmlDoc) {
            string answer;
            List<string> list = GetListAttributeWithSelector(selector, attribute, htmlDoc);

            List<string> links = list.Where(e => !string.IsNullOrWhiteSpace(e) && e.Contains(name)).ToList();

            if (links.Count > 0) {
                answer = "this link is here";
            } else {
                answer = "this link is not here";
            }

            return answer;
        }

        public List<string> GetSpecificLinkList(string selector,
                                                string attribute,
                                                string name,
                                                IHtmlDocument htmlDoc) {
            List<string> list = GetListAttributeWithSelector(selector, attribute, htmlDoc);

            List<string> links = list.Where(e => !string.IsNullOrWhiteSpace(e) && e.Contains(name)).ToList();

            return links;
        }

        public int CountSpecificLink(string selector,
                                     string attribute,
                                     string name,
                                     IHtmlDocument htmlDoc) {
            List<string> list = GetSpecificLinkList(selector, attribute, name, htmlDoc);

            List<string> links = list.Where(e => !string.IsNullOrWhiteSpace(e) && e.Contains(name)).ToList();

            return links.Count;
        }

        public int CountSpecificLinkDiff(string selector,
                                         string attribute,
                                         string name,
                                         IHtmlDocument htmlDoc,
                                         IHtmlDocument htmlDocMainPage,
                                         string contentStartPage,
                                         string contentMainPage) {
            int diff;
            List<string> list = GetSpecificLinkList(selector, attribute, name, htmlDoc);

            if (contentMainPage != contentStartPage) {
                List<string> listMainPage = GetSpecificLinkList(selector, attribute, name, htmlDocMainPage);
                List<string> diffList = listMainPage.Where(e => list.Contains(e)).ToList();

                diff = list.Count - diffList.Count;
            } else {
                diff = list.Count;
            }

            return diff;
        }

        public int FindSpecificInternalLink(string name, IHtmlDocument htmlDoc, string uriHost) {
            List<string> list = GetInternalLinksList(htmlDoc, uriHost);
            List<string> links = list.Where(e => !string.IsNullOrWhiteSpace(e) && e.Contains(name)).ToList();

            return links.Count;
        }

        public int UniqueInternalLinks(IHtmlDocument htmlDoc, string uriHost) {
            List<string> list = GetInternalLinksList(htmlDoc, uriHost);

            HashSet<string> uniqueList = list.Where(e => list.Contains(e)).ToHashSet();

            return uniqueList.Count;
        }

        public int NonUniqueInternalLinks(IHtmlDocument htmlDoc, string uriHost) {
            List<string> list = GetInternalLinksList(htmlDoc, uriHost);

            int nonUniqueList = list.Count - UniqueInternalLinks(htmlDoc, uriHost);

            return nonUniqueList;
        }
    }
}