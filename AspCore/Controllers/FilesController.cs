using Application.Files;
using Application.Files.Queries;
using AspCore.Attributes;
using AspCore.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AspCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IHubContext<FilesHub> FileHub;

        public FilesController(IHostingEnvironment appEnvironment, IHubContext<FilesHub> fileHub)
        {
            _appEnvironment = appEnvironment;
            FileHub = fileHub;
        }

        [HttpGet]
        public async Task<IActionResult> GetFolders() => Ok(await Mediator.Send(new GetDirectoryQuery()));

        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetItems(string id) => Ok(await Mediator.Send(new GetDirectoryItemsQuery(id)));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) =>
            PhysicalFile(await Mediator.Send(new GetFileQuery(id, _appEnvironment.ContentRootPath)), "application/zip", $"{id}.zip");

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Ok(await Mediator.Send(new DeleteDirectoryQuery(id)));

        [HttpPost]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> Index()
        {
            FormValueProvider formModel = await Request.StreamFile(Path.Combine(FilesConsts.FilesDir, Guid.NewGuid().ToString()));

            var viewModel = new MyViewModel();

            var bindingSuccessful = await TryUpdateModelAsync(viewModel, prefix: "",
               valueProvider: formModel);

            if (!bindingSuccessful)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            await FileHub.Clients.All.SendAsync("Updated");

            return Ok(viewModel);
        }
    }
    public class MyViewModel
    {
        public string Username { get; set; }
    }
}