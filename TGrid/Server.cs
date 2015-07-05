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
            var engine = new StorageEngine(100);
            var factory = new TupleFactory();
            var logger = new Logger();
            new AdapterLoader().Load(engine, factory, logger);
            Console.ReadLine();
        }
    }
}