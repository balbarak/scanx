using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScanX.Core;
using ScanX.Protocol.Protocol;

namespace ScanX.Protocol
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
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("*");
                }));

            services.AddSignalR();

            services.AddControllersWithViews();

            services.AddTransient<IPrinterClient, PrinterClient>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();


            //app.UseCors("CorsPolicy");

            app.UseCors(builder =>
            {
                builder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ScanXProtocol>("/scanx");
            });
        }
    }
}
