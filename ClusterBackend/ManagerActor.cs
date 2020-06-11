using System;
using Akka.Actor;
using Akka.Routing;
using Shared;

namespace ClusterBackend
{
    public class ManagerActor : ReceiveActor
    {
        private IActorRef _router;

        public ManagerActor()
        {
            var props = Props.Create(() => new StorageActor()).WithRouter(FromConfig.Instance);
            _router = Context.ActorOf(props, "router");

            Receive<Get>(get =>
                
                // tako da StorageActor od pool rutera može direktno odgovorit, da ne moramo
                // u ManagerActoru primat odgovore i ponovno slat
                _router.Forward(get)
                );

            Receive<string>(c => Console.WriteLine(c));
        }
    }
}