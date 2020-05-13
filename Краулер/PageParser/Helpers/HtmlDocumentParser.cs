using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;
using PageParser.Helpers.Interfaces;

namespace PageParser.Helpers {
    public class HtmlDocumentParser : IHtmlDocumentParser {
        public List<string> GetListSelectorWithAttribute(string selector, string attribute, IHtmlDocument parsedPage) {
            List<string> list = parsedPage.QuerySelectorAll(selector).Select(element => element.GetAttribute(attribute)).Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
            return list;
        }

        public bool IsMainPage(string domain) {
            Uri uri = new Uri(domain);
            if (uri.AbsolutePath == "/" || uri.AbsolutePath == null || uri.AbsolutePath == String.Empty) {
               
                return true;
            } else {
                return false;
            }
        }

        public string PresenceSocialNetworkLink(List<string> list, string nameSocialNetwork) {
            string firstOrDefault = list.FirstOrDefault(e => e.Contains(nameSocialNetwork));
            string report = firstOrDefault != null
                ? $" is present."
                : $" is missing.";

            return report;
        }
    }
}