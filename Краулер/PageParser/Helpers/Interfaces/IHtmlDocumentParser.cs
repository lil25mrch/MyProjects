using System.Collections.Generic;
using AngleSharp.Html.Dom;

namespace PageParser.Helpers.Interfaces {
    public interface IHtmlDocumentParser {
        List<string> GetListAttributesFromSelector(string selector, string attribute, IHtmlDocument htmlDoc);
        bool IsMainPage(string startPageAdress, string mainPageAdress);
    }
}