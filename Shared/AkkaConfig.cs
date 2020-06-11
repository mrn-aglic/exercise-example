using Newtonsoft.Json;

namespace Shared
{
    public class AkkaConfig
    {
        [JsonProperty(PropertyName = "actor")]
        public ActorConfig Actor { get; set; }
        [JsonProperty(PropertyName = "remote")]
        public RemoteConfig Remote { get; set; }
        
        public class ActorConfig
        {
            [JsonProperty(PropertyName = "provider")]
            public string Provider { get; set; }
        }
        
        public class RemoteConfig
        {
            [JsonProperty(PropertyName = "dot-netty")]
            public DotNettyConfig DotNetty { get; set; }
            public class DotNettyConfig
            {
                [JsonProperty(PropertyName = "tcp")]
                public TcpConfig Tcp { get; set; }
                
                public class TcpConfig
                {
                    [JsonProperty(PropertyName = "hostname")]
                    public string Hostname { get; set; }
                    [JsonProperty(PropertyName = "port")]
                    public int Port { get; set; }
                }
            }
        }
    }
}