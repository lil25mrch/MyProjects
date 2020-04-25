using System;
using System.Threading.Tasks;

namespace Краулер.Helpers {
    
    public class RetryHelper : IRetryHelper {
        readonly ReportCreater _reportCreater;
        private readonly TimeSpan _delay = TimeSpan.FromSeconds(5);
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