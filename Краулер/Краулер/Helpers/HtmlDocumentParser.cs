using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Краулер.Helpers {
    public class HtmlDocumentParser {
        public void LinksForMessag(string name, IHtmlDocument htmlDocument) {
            IHtmlCollection<IElement> list = htmlDocument.QuerySelectorAll("a");

            HashSet<string> links = new HashSet<string>();

            foreach (IElement elementNameSS in list) {
                string countNameSS = elementNameSS.GetAttribute("href");
                if (string.IsNullOrWhiteSpace(countNameSS)) {
                    continue;
                }

                if (countNameSS.Contains(name)) {
                    links.Add(countNameSS);
                }
            }

            if (links.Count > 0) {
                Console.WriteLine("this tag is here: {0}", name);
            } else {
                Console.WriteLine("This tag is not here: {0}", name);
            }
        }

        public int CountHNumforMessage(string tagname, IHtmlDocument htmlDocument) {
            int countHNum = htmlDocument.QuerySelectorAll(tagname).Length;
            return countHNum;
        }
    }
}