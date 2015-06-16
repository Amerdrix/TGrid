namespace Amerdrix.TGrid.Storage
{
    public interface IStorageEngine
    {
        void Put(Tuple tuple);
        Tuple Read(MatchPattern match);
        Tuple Take(MatchPattern match);
    }
}