using System.Collections.Generic;
using AngleSharp.Html.Dom;

namespace PageParser.Helpers.Interfaces {
    public interface IHtmlDocumentParser {
        List<string> GetListSelectorWithAttribute(string selector, string attribute, IHtmlDocument parsedPage);

        bool IsMainPage(string startPageAdress);
        bool PresenceSocialNetworkLink(List<string> list, string nameSocialNetwork);
    }
}