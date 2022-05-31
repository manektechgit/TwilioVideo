using VideoCall.Hubs;
using VideoCall.Options;
using VideoCall.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoCall
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<TwilioSettings>(settings =>
            {
                settings.AccountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
                settings.ApiSecret = Environment.GetEnvironmentVariable("TWILIO_API_SECRET");
                settings.ApiKey = Environment.GetEnvironmentVariable("TWILIO_API_KEY");
            })
                    .AddTransient<IVideoService, VideoService>()
                    .AddSpaStaticFiles(config => config.RootPath = "ClientApp/dist");

            services.AddSignalR();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VideoCall", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VideoCall v1"));
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("EnableCORS");
            app.UseCors(x => x.AllowAnyHeader()
                      .AllowAnyMethod()
                      .WithOrigins("https://localhost:4200"));

            app.UseEndpoints(endpoints =>
            {
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHub<NotificationHub>("/notificationHub");
            }
                //endpoints.MapControllers();
            });

        }
    }
}
