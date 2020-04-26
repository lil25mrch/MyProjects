using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;
using PageParser.Helpers.Interfaces;

namespace PageParser.Helpers {
    public class HtmlDocumentParser : IHtmlDocumentParser {
        public List<string> GetListAttributesFromSelector(string selector, string attribute, IHtmlDocument htmlDoc) {
            List<string> list = htmlDoc.QuerySelectorAll(selector).Select(element => element.GetAttribute(attribute)).Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
            return list;
        }

        public bool IsMainPage(string startPageAdress, string mainPageAdress) {
            bool Bool = startPageAdress == mainPageAdress;
            return Bool;
        }
    }
}