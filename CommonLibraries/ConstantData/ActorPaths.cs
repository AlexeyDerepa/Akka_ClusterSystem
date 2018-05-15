using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonLibraries.ConstantData
{


    /// <summary>
    /// Static helper class used to define paths to fixed-name actors
    /// (helps eliminate errors when using <see cref="ActorSelection"/>)
    /// </summary>
    public static class ActorPaths
    {
        public static readonly ActorMetaData PingCoordinatorActorMetaData = new ActorMetaData("pingcoordinator", "pingcoordinator", new int[] { 2551, 2552 }, 2552);
        public static readonly ActorMetaData PingActorMetaData = new ActorMetaData("ping", "ping", new int[] { 2551, 2552 }, 2552);
        public static readonly ActorMetaData ArpActorMetaData = new ActorMetaData("arp", "ping", new int[] { 2551, 2552 }, 2552);
        public static readonly ActorMetaData WriterActorMetaData = new ActorMetaData("writer", "writer", new int[] { 2551, 2552 }, 2551);
        public static readonly ActorMetaData ReaderActorMetaData = new ActorMetaData("reader", "reader", new int[] { 2551, 2552 }, 0);
    }

    /// <summary>
    /// Meta-data class
    /// </summary>
    public class ActorMetaData
    {
        #region Common config

        #endregion
        private string CommonAkkaConfig2()
        {
            string str = @"
                                 akka {
                                  actor {
                                   provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
                                     }
                                     remote {
log-remote-lifecycle-events = off
                                   dot-netty.tcp {
                                    hostname = """ + CommonNames.Hostname + @"""
                                    port = " + Port + @"
                                    maximum-frame-size = 256000b
                                         }
                                  }
                                  cluster {
                                   seed-nodes = [";

            for (int i = 0; i < this.Seed_nodes.Length; i++)
            {
                if (i != 0) str += ",";
                str += @"""akka.tcp://" + CommonNames.ActorSystemName + @"@" + CommonNames.Hostname + @":" + Seed_nodes[i] + @"""";
            }

                  str +=@"]
                        roles = [" + Role + @"]
                        seed - node - timeout = 5s

                    }
                 } 
                ";

            return str;
        }
        public ActorMetaData(string name, string role, int[] seed_nodes, int port = 0, bool isCommonAkkaConfig = true)
        {
            Name = name;
            Seed_nodes = seed_nodes;
            Port = port;
            Role = role;
            HoconFileName = $"{name}.hocon";
            Path = $"akka.tcp://{CommonNames.ActorSystemName}@{CommonNames.Hostname}:{port}/user/{Name}";
            AkkaConfig = isCommonAkkaConfig
                ? CommonAkkaConfig2()//String.Format(CommonAkkaConfig, CommonNames.Hostname, port, CommonNames.ActorSystemName, role)
                : File.ReadAllText(HoconFileName);
        }

        int Port;
        private int [] Seed_nodes = null;

        private string AkkaConfig;
        private string Role;
        public string Name { get; private set; }
        public Akka.Configuration.Config Config => Akka.Configuration.ConfigurationFactory.ParseString(AkkaConfig);
        public string HoconFileName { get; private set; }
        public string Path { get; private set; }
    }
}
