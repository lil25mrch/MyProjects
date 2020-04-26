using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PageParser.Helpers;
using PageParser.Helpers.Interfaces;

namespace PageParser {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            services.AddSingleton<ReportCreater>()
                .AddSingleton<IHtmlDocumentParser, HtmlDocumentParser>()
                .AddSingleton<IRegexHelper, RegexHelper>()
                .AddSingleton<IRetryHelper, RetryHelper>()
                .AddSingleton<IWebHelper, WebHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}