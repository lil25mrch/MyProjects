using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PageParser.Modals;
using Краулер.Helpers;

namespace PageParser.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        private readonly ReportCreater _reportCreater;

        public ValuesController(ReportCreater reportCreater) {
            _reportCreater = reportCreater;
        }

        [HttpPost]
        public ActionResult<Dictionary<string, string>> Post(Request s) {
            Dictionary<string, string> dictionary = _reportCreater.PageParse(s._request);
            return dictionary;
        }
    }
}