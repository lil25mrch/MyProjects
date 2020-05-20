using System;
using NUnit.Framework;
using PageParser.Helpers;

namespace TestProjectPageParser.TestHelpers {
    public class Test {
        [SetUp]
        public void Setup() { }
        
        [TestCase("http://www.a.ru", true)]
        [TestCase("http://www.a.ru/", true)]
        [TestCase("http://a.ru", true)]
        [TestCase("http://a.ru/", true)]
        [TestCase("https://www.a.ru", true)]
        [TestCase("https://www.a.ru/", true)]
        [TestCase("https://a.ru", true)]
        [TestCase("https://a.ru/", true)]
        [TestCase("www.a.ru", true)]
        [TestCase("www.a.ru/", true)]
        [TestCase("a.ru", true)]
        [TestCase("a.ru/", true)]
        [TestCase("http://www.a.ru/b", false)]
        [TestCase("http://a.ru/b", false)]
        [TestCase("https://www.a.ru/b", false)]
        [TestCase("https://a.ru/b", false)]
        [TestCase("www.a.ru/b", false)]
        [TestCase("a.ru/b", false)]
        public void TestIsMainPage(string domain, bool b) {
            // Arrange
            HtmlDocumentParser htmlDocumentParser = new HtmlDocumentParser();

            // Act
            if (!domain.StartsWith("http")) {
                domain = Uri.UriSchemeHttp + Uri.SchemeDelimiter + domain;
            }

            bool result = htmlDocumentParser.IsMainPage(domain);

            // Assert
            Assert.AreEqual(result, b);
        }
        
        [TestCase("/b")]
        public void TestIsMainPageUriInvalid(string domain) {
            HtmlDocumentParser htmlDocumentParser = new HtmlDocumentParser();

            // Assert
            Assert.Throws<UriFormatException>(() => htmlDocumentParser.IsMainPage(domain));
        }
    }
}