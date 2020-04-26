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

        public bool IsHomePage(string startPageAdress, string mainPageAdress) {
            bool Bool = startPageAdress == mainPageAdress;
            return Bool;
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