using AspCore.Attributes;
using AspCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private const string Folder = "files";

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
            var folders = Directory.GetDirectories(Folder);

            var result = new List<FoldersModel>();

            foreach (var f in folders)
            {
                var date = Directory.GetCreationTime(f);

                result.Add(new FoldersModel(f, date));
            }

            return Ok(result.OrderByDescending(f => f.CreatedAt));
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