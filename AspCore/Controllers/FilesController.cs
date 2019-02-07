using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCore.Attributes;
using AspCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpPost]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> Index()
        {
            FormValueProvider formModel = await Request.StreamFile("files");

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
    }
    public class MyViewModel
    {
        public string Username { get; set; }
    }
}