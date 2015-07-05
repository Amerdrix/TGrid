namespace Amerdrix.TGrid.Storage
{
    public interface IStorageEngine
    {
        void Put(ITuple tuple);
        ITuple Read(MatchPattern match);
        ITuple Take(MatchPattern match);
    }
}