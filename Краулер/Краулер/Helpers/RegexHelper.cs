using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Краулер.Helpers {
    public class RegexHelper {
        public int RegulStyle(string searchTense, string cont) {
            Regex regex = new Regex(searchTense);
            MatchCollection matches = regex.Matches(cont);
            return matches.Count;
        }
    }
}