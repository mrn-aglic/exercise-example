using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Shared;

namespace ClusterBackend
{
    public class StorageActor : ReceiveActor
    {
        private List<Student> _students;

        public StorageActor()
        {
            Receive<Get>(get => HandleGet(get));
            Receive<GetAll>(get => HandleGetAll());
        }

        private void HandleGet(Get get)
        {
            var student = _students.FirstOrDefault(s => s.Id == get.Id);
            var json = student == null ? new JObject() : JObject.FromObject(student,
                new JsonSerializer
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            Console.WriteLine(json);
            Sender.Tell(new GetResult(json));
        }

        private void HandleGetAll()
        {
            Sender.Tell(new GetAllResult(JArray.FromObject(_students)));
        }

        protected override void PreStart()
        {
            // ovdje cu napuniti neke podatke samo da imam
            _students = new List<Student>
            {
                new Student
                (
                    1,
                    "ante",
                    "antic",
                    "1234",
                    "email@email.email",
                    5,
                    true
                ),
                new Student
                (
                    2,
                    "ivo",
                    "ivic",
                    "1432",
                    "email@pmfst.com",
                    40,
                    true
                ),
                new Student
                (
                    3,
                    "mate",
                    "matic",
                    "4444",
                    "mate@email.email",
                    60,
                    true
                )
            };

            base.PreStart();
        }
    }
}