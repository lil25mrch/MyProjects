using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PageParser.Helpers;
using PageParser.Modals;

namespace PageParser.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        private readonly ReportCreater _reportCreater;

        public ValuesController(ReportCreater reportCreater) {
            _reportCreater = reportCreater;
        }

        [HttpPost]
        public ActionResult<Dictionary<string, string>> Post(PageAnalisisData page) {
            Dictionary<string, string> reportDictionary = _reportCreater.PageParse(page.Domain);
            return reportDictionary;
        }
    }
}