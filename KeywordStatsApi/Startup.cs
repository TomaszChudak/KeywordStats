using AutoMapper;
using KeywordStatsApi.Services.Implementation;
using KeywordStatsApi.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KeywordStatsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IWebClient, WebClientAdapter>();
            services.AddScoped<IMetaKeywordsTagParser, MetaKeywordsTagParser>();
            services.AddScoped<IBodyParser, BodyParser>();
            services.AddScoped<IHtmlParser, HtmlParser>();
            services.AddScoped<IStatsService, StatsService>();
            services.AddScoped<IHtmlDocumentFactory, HtmlDocumentFactory>();
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}