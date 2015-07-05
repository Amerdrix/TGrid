using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Amerdrix.TGrid.Storage;

namespace Amerdrix.TGrid.Plugins
{
    using Amerdrix.TGrid.Logging;

    internal class AdapterLoader
    {
        public void Load(IStorageEngine engine, ITupleFactory tupleFactory, ILogger logger)
        {

            foreach (var adapter in Scan().SelectMany(t => t.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i == typeof (IAdapter)))
                .Select(t => Activator.CreateInstance(t) as IAdapter))
            {
                adapter.Register(engine, tupleFactory, logger);
            }
        }

        private IEnumerable<Assembly> Scan()
        {
            var path = Path.GetFullPath(@"..\..\..\TGrid.Rest\bin\Debug\Amerdrix.TGrid.Rest.Dll");
            yield return Assembly.LoadFile(path);
        }
    }
}