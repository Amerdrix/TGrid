using System;
using Amerdrix.TGrid.Storage;

namespace Amerdrix.TGrid.Plugins
{
    public interface IAdapter : IDisposable
    {
        void Register(IStorageEngine binding);
    }
}