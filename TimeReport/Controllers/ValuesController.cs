using Microsoft.AspNetCore.Mvc;
using MongoDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeReport.Domain;

namespace TimeReport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public IRepository<User> UserRepo { get; }

        public ValuesController(IRepository<User> userRepo)
        {
            UserRepo = userRepo;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = UserRepo.Get().ToArray();
            return users;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(string id)
        {
            var user = UserRepo.GetById(id);
            return user;
        }

        // POST api/values
        [HttpPost]
        public void Create([FromBody] string value)
        {
            UserRepo.Add(new User {
                Name = value,
                Age = new Random().Next(50)
            });
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            UserRepo.Delete(id);
        }
    }
}
