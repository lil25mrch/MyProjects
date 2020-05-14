using System.Collections.Generic;
using System.Threading.Tasks;

namespace PageParser.Helpers {
    public interface IReportCreater {
        Task<Dictionary<string, string>> PageParse(string domain);
    }
}