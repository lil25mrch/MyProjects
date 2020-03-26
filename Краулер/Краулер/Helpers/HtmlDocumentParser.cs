using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Краулер.Helpers {
    public class HtmlDocumentParser {
        public int TagCount(string selector,
                            string attribute,
                            IHtmlDocument htmlDoc,
                            IHtmlDocument htmlDocSP) {
            int tag = 0;
            HashSet<string> List = new HashSet<string>();
            foreach (IElement element in htmlDoc.QuerySelectorAll(selector)) {
                string count = element.GetAttribute(attribute);
                List.Add(count);
                tag++;
            }

            int tagSP = 0;
            foreach (IElement element in htmlDocSP.QuerySelectorAll(selector)) {
                string countSrc = element.GetAttribute(attribute);
                if (List.Contains(countSrc)) {
                    tagSP++;
                }
            }

            return tag;
            return List.Count;
        }

        public string HtmlLangSearch(string selector, string attribute, IHtmlDocument htmlDoc) {
            foreach (IElement elementHtmlLang in htmlDoc.QuerySelectorAll("html")) {
                string countHtmlLang = elementHtmlLang.GetAttribute("xml:lang");
                if (countHtmlLang == null) {
                    return " missing";
                } else {
                    return countHtmlLang;
                }
            }

            return "";
        }

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