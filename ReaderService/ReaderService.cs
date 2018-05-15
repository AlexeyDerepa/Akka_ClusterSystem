using Akka.Actor;
using CommonLibraries.Config;
using CommonLibraries.ConstantData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReaderService.Actors;

namespace ReaderService
{
    class ReaderService
    {
        protected ActorSystem ClusterSystem;

        public Task WhenTerminated => ClusterSystem.WhenTerminated;


        public bool Start()
        {
            //var config = HoconLoader.ParseConfig(CommonNames.ReaderHocon);
            //ClusterSystem = ActorSystem.Create(CommonNames.ActorSystemName, config);

            var actor = ActorPaths.ReaderActorMetaData;
            ClusterSystem = ActorSystem.Create(CommonNames.ActorSystemName, actor.Config);

            IActorRef readerActor;
            //ReaderActor = ClusterSystem.ActorOf(Props.Create(() => new ReaderActor()), CommonNames.Reader);
            readerActor = ClusterSystem.ActorOf(ReaderActor.Props(), actor.Name);
            
            readerActor.Tell(Actors.ReaderActor.DoWork);

            return true;
        }

        public Task Stop()
        {
            return CoordinatedShutdown.Get(ClusterSystem).Run();
        }

    }
}
