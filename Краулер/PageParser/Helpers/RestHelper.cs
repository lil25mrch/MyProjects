using System;
using System.Threading.Tasks;
using PageParser.Helpers.Interfaces;
using RestSharp;

namespace PageParser.Helpers {
    public class RestHelper : IWebHelper {
        private static readonly RestClient _restClient = new RestClient();

        public async Task<string> GetContent(string domain) {
            _restClient.BaseUrl = new Uri(domain);
            var request = new RestRequest(domain);

            var content = await _restClient.ExecuteGetAsync(request);
            return content.Content;
        }
    }
}