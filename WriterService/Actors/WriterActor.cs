using Akka.Actor;
using Akka.Cluster;
using CommonLibraries.ConstantData;
using CommonLibraries.MessagesClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace WriterService.Actors
{
    public class WriterActor : ReceiveActor
    {
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
        public WriterActor()
        {
            Working();
        }
        private void Working()
        {
            Receive<string>(message =>
            {
                Console.WriteLine(message);
                if(message =="ping")
                    Context.System.ActorSelection(ActorPaths.ArpActorMetaData.Path).Tell(new ArpStart());
            });
            Receive<ClusterEvent.MemberRemoved>(message =>
            {
                Console.WriteLine(message);
                var removed = (ClusterEvent.MemberRemoved)message;
                Console.WriteLine("!!!!!!!!!!!!!! >>>>>>>>>>>>>>>> Member is Removed: {0}", removed.Member);
            });
        }
        public static Props Props() => Akka.Actor.Props.Create(() => new WriterActor());

    }
}
