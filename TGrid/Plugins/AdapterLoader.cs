using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Amerdrix.TGrid.Storage;

namespace Amerdrix.TGrid.Plugins
{
    internal class AdapterLoader
    {
        public void Load()
        {
            var binding = new StorageEngine(1);

            foreach (var adapter in Scan().SelectMany(t => t.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i == typeof (IAdapter)))
                .Select(t => Activator.CreateInstance(t) as IAdapter))
            {
                adapter.Register(binding);
            }
        }

        private IEnumerable<Assembly> Scan()
        {
            var path = Path.GetFullPath(@"..\..\..\TGrid.Rest\bin\Debug\Amerdrix.TGrid.Rest.Dll");
            yield return Assembly.LoadFile(path);
        }
    }
}