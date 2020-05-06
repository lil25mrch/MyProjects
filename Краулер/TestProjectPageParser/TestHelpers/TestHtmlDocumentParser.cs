using System;
using NUnit.Framework;
using PageParser.Helpers;

namespace Tests {
    public class Test {
        [SetUp]
        public void Setup() { }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        [TestCase(true)]
        
        public void TestIsMainPage(bool b) {
            // Arrange
            HtmlDocumentParser htmlDocumentParser = new HtmlDocumentParser();
            
            // Act
            bool result = htmlDocumentParser.IsMainPage("", "a");
             result = htmlDocumentParser.IsMainPage("a", "a");
             result = htmlDocumentParser.IsMainPage("http//:a.ru", "a.ru");
            // Assert
            Assert.AreEqual(result, b);
          
            



        }
    
    }
}