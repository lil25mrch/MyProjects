using System;
using System.Net;

namespace PageParser.Helpers {
    public class WebHelper {
        private readonly WebClient _client = new WebClient();

        public string GetContent(string domain) {
            Uri uri = new Uri(domain);

            string content = _client.DownloadString(uri);
            return content;
        }
    }
}