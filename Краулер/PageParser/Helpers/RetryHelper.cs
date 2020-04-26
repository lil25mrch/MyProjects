using System;
using System.Threading.Tasks;
using PageParser.Helpers.Interfaces;

namespace PageParser.Helpers {
    public class RetryHelper : IRetryHelper {
        private readonly TimeSpan _delay = TimeSpan.FromSeconds(5);
        readonly ReportCreater _reportCreater;
        private readonly int retryCount = 3;

        public RetryHelper(ReportCreater reportCreater) {
            _reportCreater = reportCreater;
        }

        public async Task ExceptionHandling(string parsingPage) {
            int currentRetry = 0;
            for (;;) {
                try {
                    _reportCreater.PageParse(parsingPage);
                    ;
                } catch {
                    currentRetry++;
                    if (currentRetry > retryCount) {
                        throw;
                    }
                } finally {
                    await Task.Delay(_delay);
                }
            }
        }
    }
}