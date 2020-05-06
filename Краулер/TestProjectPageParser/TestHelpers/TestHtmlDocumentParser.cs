using System;
using NUnit.Framework;
using PageParser.Helpers;

namespace Tests {
    public class Test {
        [SetUp]
        public void Setup() { }

        [Test]
        [TestCase("", "a", false)]
        [TestCase("a", "a", true)]
        [TestCase("http//:a.ru", "a.ru",true)]
        
        public void TestIsMainPage(string start, string main, bool b) {
            // Arrange
            HtmlDocumentParser htmlDocumentParser = new HtmlDocumentParser();
            
            // Act
            bool result = htmlDocumentParser.IsMainPage(start, main);
           
            // Assert
            Assert.AreEqual(result, b);
          
            



        }
    
    }
}