using System;
using System.IO;
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Akka.Configuration;
using Microsoft.Extensions.Configuration;
using Shared;

namespace ClusterBackend
{
    class Program
    {
        private static Config GetAkkaConfig(IConfiguration configuration)
        {
            var port = configuration.GetValue<int?>("port") ?? 0;

            var config = configuration.GetSection("akka").Get<AkkaConfig>();

            var fullConfig = new {akka = config};
            var akkaConfig = ConfigurationFactory.FromObject(fullConfig);
            return ConfigurationFactory.ParseString($"akka.remote.dot-netty.tcp.port={port}")
                .WithFallback(akkaConfig);
        }
        
        static void Main(string[] args)
        {
            var currDir = Directory.GetCurrentDirectory();
            var binIndex = currDir.LastIndexOf("bin", StringComparison.InvariantCultureIgnoreCase);
            var path = binIndex >= 0 ? currDir.Substring(0, binIndex) : currDir;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .Build();

            var akkaConfig = GetAkkaConfig(configuration);

            using (var system = ActorSystem.Create("Cluster", akkaConfig))
            {
                var props = Props.Create(() => new ManagerActor());
                var manager = system.ActorOf(props, "manager");

                var receptionist = ClusterClientReceptionist.Get(system);
                receptionist.RegisterService(manager);
                
                // manager.Tell(new Get(10000));
                
                Console.ReadLine();
                CoordinatedShutdown.Get(system).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance)
                    .Wait();
            }
        }
    }
}