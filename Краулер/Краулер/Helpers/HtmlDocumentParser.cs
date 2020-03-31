using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;

namespace Краулер.Helpers {
    public class HtmlDocumentParser {
        public List<string> GetListAttributesFromSelector(string selector, string attribute, IHtmlDocument htmlDoc) {
            List<string> list = htmlDoc.QuerySelectorAll(selector).Select(element => element.GetAttribute(attribute)).ToList();
            return list;
        }
        
        public bool CheckingPageForPriority(string startPageAdress, string mainPageAdress) {
            bool Bool = startPageAdress != mainPageAdress;
            return Bool;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public List<string> GetInternalLinksList(IHtmlDocument htmlDoc, string uriHost) {
            List<string> list = GetListAttributesFromSelector("a", "href", htmlDoc);

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
            List<string> list = GetListAttributesFromSelector(selector, attribute, htmlDoc);

            List<string> links = list.Where(e => !string.IsNullOrWhiteSpace(e) && e.Contains(name)).ToList();

            if (links.Count > 0) {
                answer = "this link is here";
            } else {
                answer = "this link is not here";
            }

            return answer;
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