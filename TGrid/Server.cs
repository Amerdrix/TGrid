using System;
using Amerdrix.TGrid.Plugins;

namespace Amerdrix.TGrid
{
    internal class Server
    {
        private static void Main(string[] args)
        {
            new AdapterLoader().Load();
            Console.ReadLine();
        }
    }
}