using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDbRepository.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Reflection;
using TimeReport.Configs;
using TimeReport.ModelBinders;
using TimeReport.Services;

namespace TimeReport
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
            services.AddMvc(options => 
                {
                    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.RegisterMongoDbRepository(Configuration, Environment.GetEnvironmentVariable("TimeReportMongoDbPassword", EnvironmentVariableTarget.Machine));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddTransient<IParseJiraTimeReport, SeleniumParseJiraTImeReport>();

            #region Options

            services.AddOptions();
            services.Configure<SeleniumConfig>(Configuration.GetSection("Selenium"));

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeReport API V1");
            });
        }
    }
}
