using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using PageParser.Modals;
using PageParser.Modals.Interfaces;

namespace PageParser.Helpers {
    public class ErrorHandlingMiddleware {
        private readonly RequestDelegate _next;
        private static readonly IPageAnalisisData _pageAnalisisData = new PageAnalisisData();
        private static readonly ILogger _logger = LogManager.GetLogger(nameof(ErrorHandlingMiddleware));
        public ErrorHandlingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            } catch (Exception ex) {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex) {
            var code = HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new {error = ex.Message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            _logger.Error($"Results is {JsonConvert.SerializeObject(_pageAnalisisData.Domains)} {result}");
            return context.Response.WriteAsync(result);
        }
    }
}