using System.Threading.Tasks;

namespace Краулер.Helpers {
    public interface IRetryHelper {
        Task ExceptionHandling(string parsingPage);
    }
}