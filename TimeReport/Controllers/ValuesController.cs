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
        
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = UserRepo.Get().ToArray();
            return users;
        }
        
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public ActionResult<User> Get(string id)
        {
            var user = UserRepo.GetById(id);
            return user;
        }
        
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public void Create([FromBody] string value)
        {
            UserRepo.Add(new User {
                Name = value,
                Age = new Random().Next(50)
            });
        }
        
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public void Delete(string id)
        {
            UserRepo.Delete(id);
        }
    }
}
