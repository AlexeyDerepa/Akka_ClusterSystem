using Akka.Actor;
using CommonLibraries.Config;
using CommonLibraries.ConstantData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WriterService.Actors;

namespace WriterService
{
    class WriterService
    {
        protected ActorSystem ClusterSystem;
        public Task WhenTerminated => ClusterSystem.WhenTerminated;


        public bool Start()
        {
            var config = HoconLoader.ParseConfig(CommonNames.WriterHocon);
            ClusterSystem = ActorSystem.Create(CommonNames.ActorSystemName, config);
            var _actor = ClusterSystem.ActorOf(WriterActor.Props(), CommonNames.Writer);

            _actor.Tell("Writer is starting work !!!");

            return true;
        }

        public Task Stop()
        {
            return CoordinatedShutdown.Get(ClusterSystem).Run();
        }

    }
}
