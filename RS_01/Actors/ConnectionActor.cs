using System;
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Akka.Serialization;
using Newtonsoft.Json.Linq;
using Shared;

namespace RS_01.Actors
{
    public class ConnectionActor : ReceiveActor
    {
        private readonly ClusterClientSettings _clusterClientSettings;
        private IActorRef _clusterClient;

        public ConnectionActor(ClusterClientSettings clusterClientSettings)
        {
            _clusterClientSettings = clusterClientSettings;

            // Receive<JObject>(json => Console.WriteLine(json));
            Receive<GetResult>(result =>
            {
                Console.WriteLine(result.Json);
                Sender.Tell(result);
                Self.Tell(PoisonPill.Instance);
            });
            Receive<GetAllResult>(result =>
            {
                Sender.Tell(result.JArray);
                Self.Tell(PoisonPill.Instance);
            });
            Receive<GetAll>(all => GetAll());
            Receive<Get>(one => GetOne(one));
            
            Receive<Save>(save => HandleSave(save));
        }

        protected override void PreStart()
        {
            var props = ClusterClient.Props(_clusterClientSettings);
            _clusterClient = Context.ActorOf(props);
            base.PreStart();
        }

        private void GetOne(Get one)
        {
            
            // _clusterClient.Ask(new ClusterClient.SendToAll("/user/manager", "hello world"))
            //     .Wait();
            _clusterClient.Ask<GetResult>(new ClusterClient.Send("/user/manager", one))
                .PipeTo(Self, Sender); // rezultat prosljedujemo Sebi, ali onoga koji nam je poslao Get poruku
            // registriramo kao Sendera kako bi mu u sljedecem koraku poslali odgovor i sebe ugasili.
        }

        private void GetAll()
        {
            // objasnjenje kao i za metoud Get
            _clusterClient.Ask<GetAllResult>(new ClusterClient.Send("/user/manager", new GetAll()))
                .PipeTo(Self, Sender);
        }

        private void HandleSave(Save save)
        {
            // problem... želimo samo reći klasteru da spremi studenta,
            // naravno, u pravom scenariju bi željeli dobiti odgovor o uspješnosti,
            // ali ajmo samo zbog diskusije pretpostaviti da necemo dobiti odgovor
            // dakle, koristimo Tell... a želimo ugasiti ovog actora
            // onda kada se poruka pošalje. Budući da je potrebno neko vrijeme da se
            // ClusterClient spoji na cluster, moze se dogoditi da ga ugasimo,
            // a da nije stigao poslati poruku klasteru. Ovo je upravo problem
            // koji se dogodio tijekom vjezbi 08.06.2020. 
            // Sto u tom slucaju napraviti? kako mozemo znati da je spajanje gotovo?
            // Najjednostavniji nacin je poslati ActorIdentify poruku na koju svaki actor
            // automatski odgovori. I nju poslati uz pomoć Aska. Za ovo rješenje nam je
            // potreban ReceiveAsync... https://petabridge.com/blog/akkadotnet-async-actors-using-pipeto/
            // Drugo rješenje je imati jednog ClusterClienta koji će uvijek biti aktivan
            // Kao treće rješenje, možda bi radilo sa PoolRouterima, ali to još nisam stigao
            // testirat. Ideja bi bila da ruter stvara klaster klijente kada mu zatrebaju,
            // a da samog rutera imamo jednu instancu koju ne gasimo nego je dostupna svima u sustavu.
        }
    }
}