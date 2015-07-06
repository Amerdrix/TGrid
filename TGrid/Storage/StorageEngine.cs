using System.Collections.Generic;
using System.Linq;
using Amerdrix.TGrid.Indexing;

namespace Amerdrix.TGrid.Storage
{
    internal class StorageEngine : IStorageEngine
    {
        private readonly BoxHashIndex _hashIndex = new BoxHashIndex();
        private readonly IList<ITupleIndex<ITuple>> _indices = new List<ITupleIndex<ITuple>>();

        public StorageEngine()
        {
            _indices.Add(this._hashIndex);
            _indices.Add(new BoxScanIndex());
        }


        public void Put(ITuple tuple)
        {
            var container = GetOrCreateContainer(tuple);
            container.DangouroslyIncrementWriteCount();
        }

        public TupleContainer<ITuple> GetOrCreateContainer(ITuple tuple)
        {
            var container = _hashIndex.Find(tuple);
            if (container == null)
            {
                container = new TupleContainer<ITuple>(tuple);
                foreach (var index in _indices)
                {
                    index.Add(container);
                }
            }

            return container;
        }

        public ITuple Read(MatchPattern match)
        {
            var container = GetContainer(match);

            if (!IsContainerEmpty(container))
            {
                return container.Tuple;
            }

            return null;
        }

        public TupleContainer<ITuple> GetContainer(MatchPattern match)
        {
            var bestIndex = SelectIndex(match);
            var container = bestIndex.Find(match);
            return container;
        }

        public ITuple Take(MatchPattern match)
        {
            var bestIndex = SelectIndex(match);
            var container = bestIndex.Find(match);

            if (!IsContainerEmpty(container))
            {
                container.DangouroslyIncrementRemoveCount();

                if (!IsContainerEmpty(container))
                {
                    foreach (var index in _indices)
                    {
                        index.Remove(container);
                    }
                }

                return container.Tuple;
            }

            return null;
        }

        private static bool IsContainerEmpty(TupleContainer<ITuple> location)
        {
            return location == null || location.WriteCount <= location.RemoveCount;
        }

        private ITupleIndex<ITuple> SelectIndex(MatchPattern pattern)
        {
            return _indices.OrderByDescending(x => x.Rank(pattern)).First();
        }
    }
}