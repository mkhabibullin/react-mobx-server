﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGetway
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .WithOrigins(
                    "http://localhost:3000",
                    "https://localhost:3000",
                    "http://i2x2.net",
                    "https://i2x2.net",
                    "http://localhost",
                    "http://localhost:80"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            }));

            // Enables injection of service
            // dependencies of Ocelot.
            services.AddOcelot();

            services.AddSwaggerForOcelot(Configuration);

            services.AddMediatR(typeof(Startup));

            services.AddHttpClient();
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
            app.UseCors("CorsPolicy");

            //app.UseHttpsRedirection();
            app.UseMvc();

            app.UseStaticFiles();

            app.UseSwaggerForOcelotUI(Configuration);

            app.UseWebSockets();
            app.UseOcelot().Wait();
        }
    }
}
