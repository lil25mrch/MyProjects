using System.Threading.Tasks;

namespace PageParser.Helpers.Interfaces {
    public interface IRetryHelper {
        Task ExceptionHandling(string parsingPage);
    }
}