using Application.Files.Models;
using Application.Files.Queries;
using AspCore.Attributes;
using AspCore.Extensions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : BaseController
    {
        private const string Folder = "files";
        private readonly IHostingEnvironment _appEnvironment;

        public FilesController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> Index()
        {
            FormValueProvider formModel = await Request.StreamFile(Path.Combine(Folder, Guid.NewGuid().ToString()));

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

            return Ok(viewModel);
        }

        [HttpGet]
        public IActionResult GetFolders()
        {
            if (!Directory.Exists(Folder)) return Ok();
            var folders = Directory.GetDirectories(Folder);

            var result = new List<FoldersModel>();

            foreach (var f in folders)
            {
                var date = Directory.GetCreationTime(f);

                result.Add(new FoldersModel(f.Replace(Folder + "\\", "", StringComparison.InvariantCultureIgnoreCase), date));
            }

            return Ok(result.OrderByDescending(f => f.CreatedAt));
        }

        [HttpGet("{id}/items")]
        public IActionResult GetItems(string id)
        {
            return Ok(Mediator.Send(new GetDirectoryItemsQuery(id)));
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var tempPath = $"temp";

            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

            var zipPath = $"{tempPath}/{id}.zip";

            if (!System.IO.File.Exists(zipPath))
            {
                ZipFile.CreateFromDirectory($"{Folder}/{id}", zipPath);
            }

            var fullZipPath = Path.Combine(_appEnvironment.ContentRootPath, zipPath);

            return PhysicalFile(fullZipPath, "application/zip", $"{id}.zip");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var path = $"{Folder}/{id}";

            if (Directory.Exists(path)) Directory.Delete(path, true);

            return Ok();
        }
    }
    public class MyViewModel
    {
        public string Username { get; set; }
    }

    public class FoldersModel
    {
        public FoldersModel(string name, DateTime createdAt)
        {
            Name = name;
            CreatedAt = createdAt;
        }

        public string Name { get; }

        public DateTime CreatedAt { get; }
    }
}