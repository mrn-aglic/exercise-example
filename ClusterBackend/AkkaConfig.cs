using Newtonsoft.Json;

namespace ClusterBackend
{
    public class AkkaConfig : Shared.AkkaConfig
    {
        [JsonProperty(PropertyName = "actor")]
        public new ActorConfig Actor { get; set; }
        
        [JsonProperty(PropertyName = "cluster")]
        public ClusterConfig Cluster { get; set; }

        [JsonProperty(PropertyName = "extensions")]
        public string[] Extensions { get; set; }

        public new class ActorConfig : Shared.AkkaConfig.ActorConfig
        {
            [JsonProperty(PropertyName = "deployment")]
            public DeploymentConfig Deployment { get; set; }
            
            public class DeploymentConfig
            {
                [JsonProperty(PropertyName = "/manager/router")]
                public RouterConfig Manager_Router { get; set; }

                public class RouterConfig
                {
                    [JsonProperty(PropertyName = "router")]
                    public string Router { get; set; }

                    [JsonProperty(PropertyName = "max-nr-of-instances-per-node")]
                    public int MaxNrOfInstancesPerNode { get; set; }

                    [JsonProperty(PropertyName = "cluster")]
                    public RouterClusterConfig Cluster { get; set; }

                    public class RouterClusterConfig
                    {
                        [JsonProperty(PropertyName = "enabled")]
                        public string Enabled { get; set; }

                        [JsonProperty(PropertyName = "use-role")]
                        public string UseRole { get; set; }

                        [JsonProperty(PropertyName = "allow-local-routees")]
                        public string AllowLocalRoutees { get; set; }
                    }
                }
            }
        }
        
        public class ClusterConfig
        {
            [JsonProperty(PropertyName = "seed-nodes")]
            public string[] SeedNodes { get; set; }

            [JsonProperty(PropertyName = "roles")] public string[] Roles { get; set; }
        }
    }
}