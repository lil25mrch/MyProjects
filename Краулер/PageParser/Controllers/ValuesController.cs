using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using PageParser.Helpers;
using PageParser.Modals;

namespace PageParser.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        private readonly ILogger _logger = LogManager.GetLogger(nameof(ValuesController));
        private readonly IReportCreater _reportCreater;

        public ValuesController(IReportCreater reportCreater) {
            _reportCreater = reportCreater;
        }

        [HttpPost]
        public async Task<Dictionary<string, Dictionary<ResultItem, string>>> Post(PageAnalisisData page) {
            if (page.Domains.IsNullOrEmpty()) {
                throw new ArgumentException("Domains's array is empty");
            }
            Dictionary<string, Task<Dictionary<ResultItem, string>>> tasks = new Dictionary<string, Task<Dictionary<ResultItem, string>>>();

            foreach (var domain in page.Domains.Distinct()) {
                tasks.Add(domain, _reportCreater.PageParse(domain));
            }

            await Task.WhenAll(tasks.Values);
            Dictionary<string, Dictionary<string, string>> result = tasks.ToDictionary(e => e.Key, e => e.Value.Result);
            _logger.Info($"Results for  {JsonConvert.SerializeObject(page)} is {JsonConvert.SerializeObject(result)}");
            return result;
        }
    }
}