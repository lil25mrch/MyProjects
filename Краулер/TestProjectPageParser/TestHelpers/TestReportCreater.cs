using System.Collections.Generic;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Moq;
using NUnit.Framework;
using PageParser.Helpers;
using PageParser.Helpers.Interfaces;

namespace Tests {
    public class TestReportCreater {
        [SetUp]
        public void Setup() { }

        [Test]
        public void PageParse() {
            // Arrange
            Mock<IHtmlDocumentParser> _mockIHtmlDocumentParser = new Mock<IHtmlDocumentParser>();
            Mock<IRegexHelper> _mockIRegexHelper = new Mock<IRegexHelper>();
            Mock<IWebHelper> _mockIWebHelper = new Mock<IWebHelper>();

            string content =
                "<!DOCTYPE html> <html lang=\"ru\"> <head> <title>Астролог </title> />  <link href=\"https://www.ellegirl.ru/articles/astrolog-preduprezhdaet-goroskop-na-13-maya-2020/\" /> <link rel=\"image_src\" href=\"https://n1s1.hsmedia.ru/cd/60/fd/cd60fde9873f60da870c83c940da6bd0/2083x1250_0xac120003_18411474411588940410.jpg\" />  <link title=\"ElleGirl.ru \" href=\"//www.ellegirl.ru/rss-feeds/rss.xml\"> <link href=\"https://cdn.hsmedia.ru/public/favicon/ellegirl/favicon.ico\"> <link rel=\"preconnect\" href=\"https://top-fwz1.mail.ru\" crossorigin> <script src=\"https://cdn.hsmedia.ru/dist/ellegirl/ArticlesPage.78355c017b3b20b79fee.js\"</script> <script src=\"https://cdn.hsmedia.ru/dist/ellegirl/app.c031001193ca9b6e02e0.js\" defer></script>  <h4 class=\"block-title\" data-v-1bb1fc10>Материалы по теме</h4>  <link rel=\"preconnect\" href=\"https://cdn.onthe.io\" crossorigin> <link rel=\"preconnect\" href=\"https://tt.onthe.io\" crossorigin> <link rel=\"preconnect\" href=\"https://px.hsmedia.ru\" crossorigin> <link rel=\"preconnect\" href=\"https://mc.webvisor.ru\" crossorigin> <link rel=\"preconnect\" href=\"https://ads.adfox.ru\" crossorigin> <link rel=\"preconnect\" href=\"https://banners.adfox.ru\" crossorigin> <link rel=\"preconnect\" href=\"https://yastatic.net\" crossorigin> <link rel=\"preconnect\" href=\"https://avatars.mds.yandex.net\" crossorigin> <link rel=\"preconnect\" href=\"https://www.google.com\" crossorigin> <li class=\"related-entities__item related-entities__item\" data-v-1bb1fc10> <a href=\"/articles/kakaya-ty-pesnya-k-pop-po-znaku-zodiaka/\" target=\"_self\" </a><h4 class=\"block-title\" data-v-fe920032>Теги</h4> <svg class=\"svg-icon\"><use xlink:href=\"#ellegirl_colored\"></use></svg> <a href=\"https://www.ellegirl.ru/articles/elle-girl-v-mae-zhizn-posle-stayhome/ style=\"background-image:url(https://n1s1.hsmedia.ru/7d/b9/19/7db91921b53713fdffcf876bd050b97a/12x15_21_b63539051767bf9c7f9ffd10fd2fdbd0@1540x1958_0xac120003_489103431588856573.jpg) <a href=\"http://www.psychologies.ru/\" t></a> <a href=\"https://www.parents.ru/\"</a </body> </html>";
            string domain = "http://testsite.com";
            _mockIWebHelper.Setup(a => a.GetContent(It.IsAny<string>())).Returns(content);
            _mockIRegexHelper.Setup(a => a.RegexList(It.IsAny<string>(),  content)).Returns(new List<string> {"test1", "test2", "test3"});

            _mockIHtmlDocumentParser.Setup(a => a.GetListSelectorWithAttribute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IHtmlDocument>()))
                .Returns(new List<string>() {"test1", "test2", "test3", "test4", "test5", "test6"});
            _mockIHtmlDocumentParser.Setup(a => a.IsMainPage(domain)).Returns(true);

            ReportCreater reportCreater = new ReportCreater(_mockIHtmlDocumentParser.Object, _mockIRegexHelper.Object, _mockIWebHelper.Object);

            // Act
            Dictionary<string, string> result1 = reportCreater.PageParse(domain);

            // Assert
            
            _mockIWebHelper.Verify(e => e.GetContent(It.Is<string>(m => m == domain)), Times.Once );
            //Assert.AreEqual(result1, null);
        }
    }
}