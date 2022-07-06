using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NotificationCenter.Core;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Hubs;
using NotificationCenter.Core.Models;
using System;
using System.IO;
using System.Reflection;

namespace NotificationCenter.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
              .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification Center", Version = "v1", });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddNotificationCenterContext(Configuration, Assembly.GetExecutingAssembly());
            services.AddNotificationCenter(Configuration);
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<NotificationTemplate, NotificationTemplateRequest>().ReverseMap();
                cfg.CreateMap<NotificationTemplate, NotificationTemplateResponse>().ReverseMap();
                cfg.CreateMap<NotificationDataDetails, NotificationTemplateParamResponse>().ReverseMap();
                cfg.CreateMap<NotificationDataDetails, NotificationTemplateParamRequest>().ReverseMap();
                cfg.CreateMap<NotificationDataDetails, NotificationTemplateParamResponse>().ReverseMap();
                cfg.CreateMap<TemplateType, TemplateTypeResponse>().ReverseMap();
                cfg.CreateMap<TemplateType, TemplateTypeRequest>().ReverseMap();
                cfg.CreateMap<Client, ClientToRegisterRequest>().ReverseMap();
            });

            services.AddSignalR(options => {
                options.EnableDetailedErrors = true;
            });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification Center"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(opt => opt.AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .WithOrigins("http://localhost:4200"));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<MessengerHub>("/hubs/messenger");
                });
            });
        }
    }
}
