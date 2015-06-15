using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amerdrix.TGrid;

namespace Amerdrix.TGrid.Indexing
{
    interface ITupleIndex
    {
        void Add(Tuple tuple, TupleLocation index);
        void Remove(Tuple tuple);

        double Rank(MatchPattern pattern);

        TupleLocation Find(MatchPattern pattern);
    }

}
