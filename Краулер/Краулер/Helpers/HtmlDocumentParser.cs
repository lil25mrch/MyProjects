using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Краулер.Helpers {
    public class HtmlDocumentParser {
        public int TagCount(string selector,
                            string attribute,
                            IHtmlDocument htmlDocument,
                            IHtmlDocument htmlDocumentSP) {
            int tag = 0;
            HashSet<string> List = new HashSet<string>();
            foreach (IElement element in htmlDocument.QuerySelectorAll(selector)) {
                string count = element.GetAttribute(attribute);
                List.Add(count);
                tag++;
            }

            int tagSP = 0;
            foreach (IElement element in htmlDocumentSP.QuerySelectorAll(selector)) {
                string countSrc = element.GetAttribute(attribute);
                if (List.Contains(countSrc)) {
                    tagSP++;
                }
            }

            return tag - tagSP;
        }

        public void ImgSrcCount(IHtmlDocument htmlDocument, IHtmlDocument htmlDocumentSP) {
            int src = 0;
            HashSet<string> srcList = new HashSet<string>();
            foreach (IElement elementSrc in htmlDocument.QuerySelectorAll("img")) {
                string countSrc = elementSrc.GetAttribute("src");
                srcList.Add(countSrc);
                src++;
            }

            //число тегов <img src>, исключая теги, которые есть на главной странице
            int srcSP = 0;
            foreach (IElement elementSrcSP in htmlDocumentSP.QuerySelectorAll("img")) {
                string countSrc = elementSrcSP.GetAttribute("src");
                if (srcList.Contains(countSrc)) {
                    srcSP++;
                }
            }
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