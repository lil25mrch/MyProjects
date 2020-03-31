using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;

namespace Краулер.Helpers {
    public class HtmlDocumentParser {
        public List<string> GetListAttributesFromSelector(string selector, string attribute, IHtmlDocument htmlDoc) {
            List<string> list = htmlDoc.QuerySelectorAll(selector).Select(element => element.GetAttribute(attribute)).ToList();
            return list;
        }

        public bool IsMainPage(string startPageAdress, string mainPageAdress) {
            bool Bool = startPageAdress == mainPageAdress;
            return Bool;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
    }
}