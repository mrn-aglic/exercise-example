using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RS_01.Actors;
using RS_01.DtoMappers;
using RS_01.Models;
using Shared;

namespace RS_01.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsApiController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<Student>>> Get()
        {
            var props = Props.Create(() => new ConnectionActor(AkkaService.CClientSettings));
            var actor = AkkaService.ActorSys.ActorOf(props);

            var result = await actor.Ask<List<Student>>(new GetAll());

            return Ok(result);
            // return _students;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var props = Props.Create(() => new ConnectionActor(AkkaService.CClientSettings));
            var actor = AkkaService.ActorSys.ActorOf(props);

            var result = await actor.Ask<GetResult>(new Get(id));
            var student = StudentDto.FromWierdAkkaJson(result.Json);

            return Ok(student);

            // return _students.Find(x => x.Id == id);
        }

        [HttpPost("save")]
        public ActionResult Save([FromBody] JObject json)
        {
            var student = StudentDto.FromJson(json);
            // _students.Add(student);
            return Ok();
        }
    }
}