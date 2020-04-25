using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;

namespace Краулер.Helpers {
    public class HtmlDocumentParser : IHtmlDocumentParser {
        public List<string> GetListAttributesFromSelector(string selector, string attribute, IHtmlDocument htmlDoc) {
            List<string> list = htmlDoc.QuerySelectorAll(selector).Select(element => element.GetAttribute(attribute)).ToList();
            return list;
        }

        public bool IsMainPage(string startPageAdress, string mainPageAdress) {
            bool Bool = startPageAdress == mainPageAdress;
            return Bool;
        }
 
    }
}