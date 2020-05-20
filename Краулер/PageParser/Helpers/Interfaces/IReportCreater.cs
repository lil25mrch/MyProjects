using System.Collections.Generic;
using System.Threading.Tasks;

namespace PageParser.Helpers {
    public interface IReportCreater {
        Task<Dictionary<ResultItem, string>> PageParse(string domain);
    }
}