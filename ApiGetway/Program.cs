using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TimeReport;

namespace ApiGetway
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            Configuration = configBuilder.Build();

            var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());

            var host = builder.Build();

            if (isService)
            {
                // To run the app without the CustomWebHostService change the
                // next line to host.RunAsService();
                host.RunAsCustomService();
            }
            else
            {
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(Configuration.GetValue<string>("Urls"))
                // Add the code below to the
                // Program.cs file @ BuildWebHost method
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddJsonFile("ocelot.json")
                        .AddEnvironmentVariables();
                })
                .UseStartup<Startup>();
                //.UseKestrel((options) =>
                //{
                //    X509Store x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                //    x509Store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                //    X509Certificate2Collection collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, "i2x2.net", true);
                //    //X509Certificate2Collection collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, "localhost", true);

                //    if (collection.Count > 0)
                //    {
                //        options.ConfigureHttpsDefaults(httpsOptions =>
                //        {
                //            // certificate is an X509Certificate2
                //            httpsOptions.ServerCertificate = collection[0];
                //        });
                //    }
                //});
    }
}
