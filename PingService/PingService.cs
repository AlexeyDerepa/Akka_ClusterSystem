using Akka.Actor;
using CommonLibraries.Config;
using CommonLibraries.ConstantData;
using PingService.Actors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PingService
{
    class PingService
    {
        protected IActorRef _pingCoordinatorActor;
        protected ActorSystem ClusterSystem;
        protected IActorRef _arpActor;

        public Task WhenTerminated => ClusterSystem.WhenTerminated;


        public bool Start()
        {
            var config = HoconLoader.ParseConfig(CommonNames.PingHocon);
            ClusterSystem = ActorSystem.Create(CommonNames.ActorSystemName, ActorPaths.PingActorMetaData.Config);

            _pingCoordinatorActor = ClusterSystem.ActorOf(PingCoordinatorActor.Props(), ActorPaths.PingActorMetaData.Name);
            _arpActor = ClusterSystem.ActorOf(Props.Create(() => new ArpActor(_pingCoordinatorActor)), ActorPaths.ArpActorMetaData.Name);

            return true;
        }

        public Task Stop()
        {
            return CoordinatedShutdown.Get(ClusterSystem).Run();
        }

    }
}
