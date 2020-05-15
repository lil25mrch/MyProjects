using System;
using System.Threading;
using System.Threading.Tasks;
using PageParser.Helpers.Interfaces;
using RestSharp;

namespace PageParser.Helpers {
    public class RestHelper : IWebHelper {
        private static readonly RestClient _restClient = new RestClient();
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(0, 10);

        public async Task<string> GetContent(string domain) {
            await _semaphoreSlim.WaitAsync();
            try {
                _restClient.BaseUrl = new Uri(domain);
                var request = new RestRequest(domain);
                
                var content = await _restClient.ExecuteGetAsync(request);
                return content.Content;
            } finally {
                _semaphoreSlim.Release();
            }
        }
    }
}

