using System.Linq;

namespace Amerdrix.TGrid
{
    using System;

    using Amerdrix.TGrid.Logging;
    using Amerdrix.TGrid.Plugins;
    using Amerdrix.TGrid.Storage;

    internal class Server
    {
        private static void Main(string[] args)
        {
            var configs = new[]
            {
                new PaxosNode.PaxosConfig {AcceptPort = 13370, AcceptedPort = 13371, PreparePort = 13372, PromisePort = 13373},
                new PaxosNode.PaxosConfig {AcceptPort = 13380, AcceptedPort = 13381, PreparePort = 13382, PromisePort = 13383},
                new PaxosNode.PaxosConfig {AcceptPort = 13390, AcceptedPort = 13391, PreparePort = 13392, PromisePort = 13393},
            };

            var nodes = configs.Select(p => new PaxosNode(p, configs)).ToList();
            
            var factory = new TupleFactory();
            var logger = new Logger();
            new AdapterLoader().Load(nodes[0].Engine, factory, logger);
            Console.ReadLine();
        }
    }
}