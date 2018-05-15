using Akka.Actor;
using Akka.Cluster;
using CommonLibraries.ConstantData;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReaderService.Actors
{
    public class ReaderActor : ReceiveActor
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

        public static string DoWork => "work";
        public ReaderActor()
        {
            Working();
        }
        private void Working()
        {
            Receive<string>(start =>
            {
                //Console.Clear();
                Console.WriteLine("Enter your command ->" + ActorPaths.WriterActorMetaData.Path);
                string command = Console.ReadLine();
                //Context.System.ActorSelection("akka.tcp://IFactorySistem@localhost:2551/user/writer").Tell(command);
                Context.System.ActorSelection(ActorPaths.WriterActorMetaData.Path).Tell(command);
                Self.Tell(ReaderActor.DoWork);
            });
        }
        public static Props Props() => Akka.Actor.Props.Create(() => new ReaderActor());

    }
}
