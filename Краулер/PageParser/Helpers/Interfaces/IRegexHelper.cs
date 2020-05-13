using System.Collections.Generic;

namespace PageParser.Helpers.Interfaces {
    public interface IRegexHelper {
        List<string> RegexList(string searchTense, string content);
    }
}