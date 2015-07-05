namespace Amerdrix.TGrid.Plugins
{
    using System;

    using Amerdrix.TGrid.Logging;
    using Amerdrix.TGrid.Storage;

    public interface IAdapter : IDisposable
    {
        void Register(IStorageEngine storageEngine, ITupleFactory tupleFactory, ILogger logger);
    }
}