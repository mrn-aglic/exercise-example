using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Shared
{
    public class GetAll
    {
    }

    public class Get
    {
        public int Id { get; }

        public Get(int id)
        {
            Id = id;
        }
    }

    public class GetResult
    {
        public JObject Json { get; }

        public GetResult(JObject json)
        {
            Json = json;
        }
    }

    public class GetAllResult
    {
        public JArray JArray { get; }

        public GetAllResult(JArray jArray)
        {
            JArray = jArray;
        }
    }

    public class Save
    {
        public JObject Json { get; }

        public Save(JObject json)
        {
            Json = json;
        }
    }
}