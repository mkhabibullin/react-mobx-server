using ApiGetway.Dto;
using ApiGetway.Enums;
using CodeSuperior.PipelineStyle;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiGetway.Queries
{
    public class GetStatusesQueryHandler : IRequestHandler<GetStatusesQuery, ServiceStatusDto[]>
    {
        private readonly IHostingEnvironment _env;
        private readonly IHttpClientFactory _clientFactory;

        public GetStatusesQueryHandler(IHostingEnvironment env, IHttpClientFactory clientFactory)
        {
            _env = env;
            _clientFactory = clientFactory;
        }

        public async Task<ServiceStatusDto[]> Handle(GetStatusesQuery request, CancellationToken cancellationToken)
        {
            var services = new List<ServiceStatusDto>();

            var ocelotFileJson = Path.Combine(_env.ContentRootPath, "ocelot.json")
                .ToIf(File.Exists, File.ReadAllText, f => throw new Exception($"File {f} does not exists"))
                .To(JsonConvert.DeserializeObject<dynamic>);

            foreach(var s in ocelotFileJson.ReRoutes)
            {
                foreach(var d in s.DownstreamHostAndPorts)
                {
                    var host = $"{d.Host}:{d.Port}";
                    var url = new Uri($"http://{host}/api/heartbeat");

                    bool isAvailable;
                    try
                    {
                        var client = _clientFactory.CreateClient();

                        var response = await client.GetAsync(url);

                        isAvailable = response.ToIf(r => r.IsSuccessStatusCode, r => r.Content.ReadAsStringAsync().Result)
                            .To(JsonConvert.DeserializeObject<bool>);
                    }
                    catch(HttpRequestException)
                    {
                        isAvailable = false;
                    }

                    services.Add(new ServiceStatusDto(host, isAvailable ? StatusType.Available : StatusType.NotAvailable));
                }
            }

            return services.ToArray();
        }
    }
}
