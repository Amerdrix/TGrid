namespace Amerdrix.TGrid.Storage
{
    internal class TupleContainer<TTuple>
    {
        public readonly TTuple Tuple;

        public uint RemoveCount;

        public uint WriteCount;

        public TupleContainer(TTuple tuple)
        {
            this.Tuple = tuple;
        }
    }
}