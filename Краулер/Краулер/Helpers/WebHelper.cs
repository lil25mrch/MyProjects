using System;
using System.Net;

namespace Краулер.Helpers {
    public class WebHelper {
        private readonly WebClient _client = new WebClient();

        public string GetContent(string domain) {
            Uri uri = new Uri(domain);

            //Документ сайта, который мы проверяем
            string content = _client.DownloadString(uri);
            return content;
        }
    }
}