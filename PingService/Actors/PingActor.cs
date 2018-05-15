using Akka.Actor;
using Akka.Cluster;
using CommonLibraries.MessagesClasses;
using PingService.Infostructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace PingService.Actors
{
    public class PingActor : ReceiveActor
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


        public PingActor()
        {
            Working();
        }
        private void Working()
        {
            Receive<PingStart>(start =>
            {
                var ping = new PingRequest();
                Console.WriteLine( "===========" +Self.Path + "===========\n" +ping.Request(start.Address));
            });
        }
        public static Props Props() => Akka.Actor.Props.Create(() => new PingActor());

    }
}
