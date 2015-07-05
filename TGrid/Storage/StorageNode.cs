namespace Amerdrix.TGrid.Storage
{
    using System.Collections.Generic;
    using System.Linq;

    using Amerdrix.TGrid.Indexing;

    internal class StorageNode : IStorageEngine
    {
        private readonly BoxHashIndex _hashIndex = new BoxHashIndex();

        private readonly IList<ITupleIndex<ITuple>> _indices = new List<ITupleIndex<ITuple>>();

        
        public StorageNode()
        {
            _indices.Add(this._hashIndex);
            _indices.Add(new BoxScanIndex());
        }

        public void Put(ITuple tuple)
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

            container.WriteCount ++;
        }

        public ITuple Read(MatchPattern match)
        {
            var bestIndex = SelectIndex(match);
            var location = bestIndex.Find(match);

            if (!IsContainerEmpty(location))
            {
                return location.Tuple;
            }

            return null;
        }

        public ITuple Take(MatchPattern match)
        {
            var bestIndex = SelectIndex(match);
            var container = bestIndex.Find(match);

            if (!IsContainerEmpty(container))
            {
                container.RemoveCount ++;

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