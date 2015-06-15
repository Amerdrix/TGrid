using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amerdrix.TGrid.Indexing
{

    class ScanIndex : ITupleIndex
    {
        private readonly Dictionary<Tuple, TupleLocation> _tuples = new Dictionary<Tuple, TupleLocation>();

        public void Add(Tuple tuple, TupleLocation index)
        {
            _tuples.Add(tuple, index);
        }

        public void Remove(Tuple tuple)
        {
            _tuples.Remove(tuple);
        }

        public TupleLocation Find(MatchPattern pattern)
        {
            var match = pattern.Pattern.ToList();
            return _tuples.Where(x => Match(x.Key.Content, match)).Select(x => x.Value).DefaultIfEmpty(TupleLocation.None).First();
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

        public double Rank(MatchPattern pattern)
        {
            return 0;
        }
    }
}
    