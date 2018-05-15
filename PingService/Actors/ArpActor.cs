using Akka.Actor;
using CommonLibraries.MessagesClasses;
using System;
using System.Collections.Generic;
using System.Text;
using PingService.Infostructure;
using Akka.Cluster;

namespace PingService.Actors
{
   public class ArpActor : ReceiveActor
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
        }
        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }
        #endregion

        private IActorRef _pingCoordinator;

        public ArpActor(IActorRef coordinator)
        {
            _pingCoordinator = coordinator;
            Working();

        }

        private void Working()
        {
            Receive<ArpStart>(start =>
            {
                var arp = new ArpRequest();

                foreach (string address in arp.GetIPAddresses())
                {
                    Console.WriteLine($"ArpActor sends address -> {address} to ping");
                    _pingCoordinator.Tell(new BeginJob { Address = address });
                }
            });
        }
        public static Props Props(IActorRef coordinator) => Akka.Actor.Props.Create(() => new ArpActor(coordinator));

    }
}
