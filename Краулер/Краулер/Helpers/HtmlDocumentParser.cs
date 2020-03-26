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
                return diff;
            } else {
                int tag = 0;
                foreach (IElement element in htmlDoc.QuerySelectorAll(selector)) {
                    string count = element.GetAttribute(attribute);
                    tag++;
                }

                diff = tag;
            }

            return diff;
        }

        public int TagCount(string selector, string attribute, IHtmlDocument htmlDoc) {
            int tag = 0;
            foreach (IElement element in htmlDoc.QuerySelectorAll(selector)) {
                string count = element.GetAttribute(attribute);
                tag++;
            }

            return tag;
        }

        public int RegulStyle(string searchTense, string cont) {
            Regex regex = new Regex(searchTense);
            MatchCollection matches = regex.Matches(cont);
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

        public string LinksOnPage(string name, IHtmlDocument htmlDocument) {
            string answer;
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
                answer = "this tag is here";
            } else {
                answer = "this tag is not here";
            }

            return answer;
        }

        public int InterLinkHasSmth(string smth, IHtmlDocument htmlDoc, string uriHost) {
            HashSet<string> list = new HashSet<string>();
            foreach (IElement element in htmlDoc.QuerySelectorAll("a")) {
                string count = element.GetAttribute("href");
                if (string.IsNullOrWhiteSpace(count)) {
                    continue;
                }

                if ((count.StartsWith("/") || count.Contains(uriHost)) && count.Contains(smth)) {
                    list.Add(count);
                }
            }

            return list.Count;
        }

        public int InterLinkHasSmthDiff(string smth,
                                        string uriHost,
                                        IHtmlDocument htmlDoc,
                                        IHtmlDocument htmlDocSP,
                                        string contentS,
                                        string contentM) {
            int diff;
            HashSet<string> list = new HashSet<string>();
            HashSet<string> listSP = new HashSet<string>();
            if (contentM != contentS) {
                foreach (IElement element in htmlDoc.QuerySelectorAll("a")) {
                    string count = element.GetAttribute("href");
                    if (string.IsNullOrWhiteSpace(count)) {
                        continue;
                    }

                    if ((count.StartsWith("/") || count.Contains(uriHost)) && count.Contains(smth)) {
                        list.Add(count);
                    }
                }

                foreach (IElement element in htmlDocSP.QuerySelectorAll("a")) {
                    string countSP = element.GetAttribute("href");
                    if (list.Contains(countSP)) {
                        listSP.Add(countSP);
                    }
                }

                diff = list.Count - listSP.Count;
                return diff;
            } else {
                diff = list.Count;
            }

            return diff;
        }

        public int UniqInterLink(IHtmlDocument htmlDoc, string uriHost) {
            HashSet<string> list = new HashSet<string>();
            foreach (IElement element in htmlDoc.QuerySelectorAll("a")) {
                string count = element.GetAttribute("href");
                if (string.IsNullOrWhiteSpace(count)) {
                    continue;
                }

                if (count.StartsWith("/") || count.Contains(uriHost)) {
                    if (list.Contains(count) != true) {
                        list.Add(count);
                    } 
                }
            }
            return list.Count;
        }
        
        public int NotUniqInterLink(IHtmlDocument htmlDoc, string uriHost) {
            HashSet<string> list = new HashSet<string>();
            HashSet<string> notUniqList = new HashSet<string>();
            foreach (IElement element in htmlDoc.QuerySelectorAll("a")) {
                string count = element.GetAttribute("href");
                if (string.IsNullOrWhiteSpace(count)) {
                    continue;
                }

                if (count.StartsWith("/") || count.Contains(uriHost)) {
                    if (list.Contains(count) != true) {
                        list.Add(count);
                    } else {
                        notUniqList.Add(count);
                    }
                }
            }
            return notUniqList.Count;
        }
    }
}


