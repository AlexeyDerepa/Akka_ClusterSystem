using Akka.Actor;
using Akka.Cluster;
using Akka.Routing;
using CommonLibraries.MessagesClasses;
using PingService.Infostructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace PingService.Actors
{
    class PingCoordinatorActor : ReceiveActor
    {
        #region Cluster configuration
        protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);
        protected override void PreStart()
        {
            Cluster.Subscribe(Self,
                ClusterEvent.InitialStateAsEvents,
                new[] {
                    typeof(ClusterEvent.IMemberEvent),
                    typeof(ClusterEvent.UnreachableMember)
                });


            _pingWorker = Context
                            .ActorOf(Akka.Actor.Props.Create(() => new PingActor())
                            .WithRouter(new RoundRobinPool(10)));
        }
        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }
        #endregion

        private IActorRef _pingWorker;
        public PingCoordinatorActor()
        {
            Working();
        }

        //protected override void PreStart()
        //{
        //    _pingWorker = Context
        //        .ActorOf(Props.Create(() => new PingActor())
        //        .WithRouter(new RoundRobinPool(10)));
        //}

        private void Working()
        {
            Receive<BeginJob>(start =>
            {
                _pingWorker.Tell(new PingStart { Address = start.Address });
            });
        }
        public static Props Props() => Akka.Actor.Props.Create(() => new PingCoordinatorActor());

    }
}
