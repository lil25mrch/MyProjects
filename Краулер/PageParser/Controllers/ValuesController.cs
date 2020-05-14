using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using PageParser.Helpers;
using PageParser.Modals;

namespace PageParser.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        private readonly IReportCreater _reportCreater;

        public ValuesController(IReportCreater reportCreater) {
            _reportCreater = reportCreater;
        }

        [HttpPost]
        public async Task<Dictionary<string, Dictionary<string, string>>> Post(PageAnalisisData page) {
            if (page.Domains.IsNullOrEmpty()) {
                throw new ArgumentException("Domains's array is empty");
            }
            Dictionary<string, Task<Dictionary<string, string>>> tasks = new Dictionary<string, Task<Dictionary<string, string>>>();

            foreach (var domain in page.Domains.Distinct()) {
                tasks.Add(domain, _reportCreater.PageParse(domain));
            }

            await Task.WhenAll(tasks.Values);

            return tasks.ToDictionary(e => e.Key, e => e.Value.Result);
        }
    }
}