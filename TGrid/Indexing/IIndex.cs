using Amerdrix.TGrid.Storage;

namespace Amerdrix.TGrid.Indexing
{
    internal interface ITupleIndex
    {
        void Add(Tuple tuple, TupleLocation index);
        void Remove(Tuple tuple);
        double Rank(MatchPattern pattern);
        TupleLocation Find(MatchPattern pattern);
    }
}