using System;
using Amerdrix.TGrid.Plugins;

namespace Amerdrix.TGrid
{
    internal class Server
    {
        private static void Main(string[] args)
        {
            var engine = new Storage.StorageEngine(100);
            new AdapterLoader().Load(engine);
            Console.ReadLine();
        }
    }
}