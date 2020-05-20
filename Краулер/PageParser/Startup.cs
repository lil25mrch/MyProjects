using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PageParser.Helpers;
using PageParser.Helpers.Interfaces;
using PageParser.Modals;
using PageParser.Modals.Interfaces;

namespace PageParser {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            services.AddSingleton<IReportCreater, ReportCreater>()
                .AddSingleton<IHtmlDocumentParser, HtmlDocumentParser>()
                .AddSingleton<IRegexHelper, RegexHelper>()
                .AddSingleton<IWebHelper, RestHelper>()
                .AddSingleton<IPageAnalisisData, PageAnalisisData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment en) {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();
        }
    }
}