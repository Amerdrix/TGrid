using Amerdrix.TGrid.Storage;

namespace Amerdrix.TGrid.Indexing
{
    internal interface ITupleIndex<TTuple>
        where TTuple: ITuple
    {
        void Add(TupleContainer<TTuple> container);
        void Remove(TupleContainer<TTuple> tuple);
        double Rank(MatchPattern pattern);
        TupleContainer<TTuple> Find(MatchPattern pattern);
    }
}