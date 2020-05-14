using System.Threading.Tasks;

namespace PageParser.Helpers.Interfaces {
    public interface IWebHelper {
        Task<string> GetContent(string domain);
    }
}