using System.Collections.Generic;
using AngleSharp.Html.Dom;

namespace PageParser.Helpers.Interfaces {
    public interface IHtmlDocumentParser {
        List<string> GetListSelectorWithAttribute(string selector, string attribute, IHtmlDocument parsedPage);
        bool IsHomePage(string startPageAdress, string mainPageAdress);
        string PresenceSocialNetworkLink(List<string> list, string nameSocialNetwork);
    }
}