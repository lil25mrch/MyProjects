using System.Collections.Generic;
using System.Text.RegularExpressions;
using PageParser.Helpers.Interfaces;

namespace PageParser.Helpers {
    public class RegexHelper : IRegexHelper {
        public List<string> RegexList(string searchTense, string content) {
            Regex regex = new Regex(searchTense);
            MatchCollection matches = regex.Matches(content);
            List<string> regexList = new List<string>();

            foreach (Match m in matches) {
                regexList.Add(m.Value);
            }

            return regexList;
        }
    }
}