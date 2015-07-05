using System.Collections.Generic;
using System.Linq;
using Amerdrix.TGrid.Storage;

namespace Amerdrix.TGrid.Indexing
{
    internal class ScanIndex : ITupleIndex<ITuple>
    {
        private readonly IList<TupleContainer<ITuple>> _tuples = new List<TupleContainer<ITuple>>();
        

        public void Add(TupleContainer<ITuple> container)
        {
            this._tuples.Add(container);
        }

        public void Remove(TupleContainer<ITuple> tuple)
        {
            this._tuples.Remove(tuple);
        }

        public TupleContainer<ITuple> Find(MatchPattern pattern)
        {
            var match = pattern.Pattern.ToList();
            return this._tuples
                    .FirstOrDefault(x => Match(x.Tuple.Content, match));
        }

        public double Rank(MatchPattern pattern)
        {
            return 0;
        }

        private bool Match(object[] tupleContent, IList<object> match)
        {
            for (var i = 0; i < tupleContent.Length; i++)
            {
                if (match[i] == MatchPattern.MatchAny)
                    continue;
                if (!tupleContent[i].Equals(match[i]))
                    return false;
            }
            return true;
        }
    }
}