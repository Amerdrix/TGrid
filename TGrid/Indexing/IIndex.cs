using Amerdrix.TGrid.Storage;

namespace Amerdrix.TGrid.Indexing
{
    internal interface ITupleIndex
    {
        void Add(ITuple tuple, TupleLocation index);
        void Remove(ITuple tuple);
        double Rank(MatchPattern pattern);
        TupleLocation Find(MatchPattern pattern);
    }
}