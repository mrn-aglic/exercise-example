using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Akka.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shared;

namespace RS_01
{
    public class AkkaService : IHostedService
    {
        public static ActorSystem ActorSys { get; private set; }
        public static ImmutableHashSet<ActorPath> InitialContacts { get; private set; }
        public static ClusterClientSettings CClientSettings { get; private set; }

        private readonly IConfiguration _configuration;

        public AkkaService(IConfiguration configuration)
        {
            _configuration = configuration;

            InitialContacts = ImmutableHashSet<ActorPath>.Empty
                .Add
                (
                    ActorPath.Parse("akka.tcp://Cluster@localhost:12000/system/receptionist")
                );
            // .Add
            // (
            //     ActorPath.Parse("akka.tcp://cluster@localhost:12001/system/receptionist")
            // );
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = _configuration.GetSection("akka").Get<AkkaConfig>();
            var fullConfig = new {akka = config};
            var akkaConfig = ConfigurationFactory.FromObject(fullConfig);

            ActorSys = ActorSystem.Create("webapi", akkaConfig);

            CClientSettings = ClusterClientSettings.Create(ActorSys)
                .WithInitialContacts(InitialContacts);

            // Zakomentirani kod sam iskoristio za testiranje, i nije dio primjera
            // Ostavljam ga samo da vidite što mi je pomoglo da dođem do zaključka o pogrešci
            // var tempClient = ActorSys.ActorOf(
            //     ClusterClient.Props(
            //         ClusterClientSettings.Create(ActorSys).WithInitialContacts(InitialContacts)));
            // tempClient.Tell(new ClusterClient.SendToAll("/user/manager", "Hello world"));
            // Task.Delay(1000).Wait();
            // tempClient.Tell(PoisonPill.Instance);

            Console.WriteLine($"[{DateTime.Now}] ActorSys started!");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return ActorSys.Terminate().ContinueWith(_ =>
                Console.WriteLine($"[{DateTime.Now}] ActorSys terminated!"), cancellationToken);
            // return CoordinatedShutdown.Get(ActorSys).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance);
        }
    }
}