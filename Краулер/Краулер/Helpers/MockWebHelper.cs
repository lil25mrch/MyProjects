using System;
using System.Net;

namespace Краулер.Helpers {
    public class MockWebHelper : IWebHelper {

        public string GetContent(string domain) {
            return "";
        }
        
    }
}