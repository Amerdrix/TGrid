namespace Amerdrix.TGrid.Indexing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Amerdrix.TGrid.Storage;

    internal class HashIndex : ITupleIndex<ITuple>
    {
        private readonly Dictionary<object, TupleContainer<ITuple>> _lookup =
            new Dictionary<object, TupleContainer<ITuple>>();

        public void Add(TupleContainer<ITuple> container)
        {
            var key = GetKey(container.Tuple.Content);
            _lookup[key] = container;
        }

        public void Remove(TupleContainer<ITuple> tuple)
        {
            // once in, it's always in
        }

        public double Rank(MatchPattern pattern)
        {
            if (pattern.Pattern.Any(x => x == MatchPattern.MatchAny))
            {
                return -1;
            }
            return 1000;
        }

        public TupleContainer<ITuple> Find(MatchPattern pattern)
        {
            return this.GetTupleContainer(pattern.Pattern);
        }

        private static object GetKey(IEnumerable<object> content)
        {
            return content.Aggregate(Tuple.Create);
        }

        private TupleContainer<ITuple> GetTupleContainer(IEnumerable<object> tupleObjects)
        {
            var key = GetKey(tupleObjects);
            TupleContainer<ITuple> container;
            this._lookup.TryGetValue(key, out container);
            return container;
        }

        public TupleContainer<ITuple> Find(ITuple tuple)
        {
            return this.GetTupleContainer(tuple.Content);
        }
    }
}