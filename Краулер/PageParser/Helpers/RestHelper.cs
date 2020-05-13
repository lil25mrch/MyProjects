using System;
using System.Linq;
using PageParser.Helpers.Interfaces;
using RestSharp;

namespace PageParser.Helpers {
    public class RestHelper : IWebHelper {
        private readonly RestClient _restClient = new RestClient();

        public string GetContent(string domain) {
            
            _restClient.BaseUrl = new Uri(domain);
            var request = new RestRequest(domain);

            var content = _restClient.ExecuteGetAsync(request).GetAwaiter().GetResult();
            return content.Content;
        }
    }
}