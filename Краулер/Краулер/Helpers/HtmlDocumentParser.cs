using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Краулер.Helpers {
    public class HtmlDocumentParser {
        public int TagDiff(string selector,
                           string attribute,
                           IHtmlDocument htmlDoc,
                           IHtmlDocument htmlDocSP,
                           string contentS,
                           string contentM) {
            int diff;
            if (contentM != contentS) {
                int tag = 0;
                HashSet<string> list = new HashSet<string>();
                foreach (IElement element in htmlDoc.QuerySelectorAll(selector)) {
                    string count = element.GetAttribute(attribute);
                    list.Add(count);
                    tag++;
                }

                int tagSP = 0;
                foreach (IElement element in htmlDocSP.QuerySelectorAll(selector)) {
                    string countSP = element.GetAttribute(attribute);
                    if (list.Contains(countSP)) {
                        tagSP++;
                    }
                }

                diff = tag - tagSP;
                return diff; } 
            else {
                int tag = 0;
                foreach (IElement element in htmlDoc.QuerySelectorAll(selector)) {
                    string count = element.GetAttribute(attribute);
                    tag++;
                }

                diff = tag;
            }

            return diff; 
        }
        
        public int TagCount(string selector,
                            string attribute,
                            IHtmlDocument htmlDoc) {
            int tag = 0;
            foreach (IElement element in htmlDoc.QuerySelectorAll(selector)) {
                string count = element.GetAttribute(attribute);
                tag++;
            }

            return tag;
        }

        public int RegulStyle(string searchTense, IHtmlDocument htmlDoc, string content) {
            Regex regex = new Regex($@"background-image:(\s*)url");
            MatchCollection matches = regex.Matches(content);
            return matches.Count;
        }

        public int AverageValue(string selector, string attribute, IHtmlDocument htmlDoc) {
            int tagLenght = 0;
            int tagCount = 0;
            foreach (IElement element in htmlDoc.QuerySelectorAll(selector)) {
                string count = element.GetAttribute(attribute);
                tagLenght += count.Length;
                tagCount++;
            }

            decimal avlTag = tagLenght / tagCount;
            return (int) Math.Round(avlTag);
        }

        public string HtmlLangSearch(string selector, string attribute, IHtmlDocument htmlDoc) {
            foreach (IElement elementHtmlLang in htmlDoc.QuerySelectorAll("html")) {
                string countHtmlLang = elementHtmlLang.GetAttribute("xml:lang");
                if (countHtmlLang == null) {
                    return "missing";
                } else {
                    return countHtmlLang;
                }
            }

            return "";
        }

        public void LinksOnPage(string name, IHtmlDocument htmlDocument) {
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

        public int CountHNumforMessage(string tagname, IHtmlDocument htmlDoc) {
            int countHNum = htmlDoc.QuerySelectorAll(tagname).Length;
            return countHNum;
        }
    }
}