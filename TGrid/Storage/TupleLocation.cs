namespace Amerdrix.TGrid.Storage
{
    internal struct TupleLocation
    {
        public static readonly TupleLocation None = new TupleLocation {TableOffset = -1, Depth = -1};
        public int Depth;
        public int TableOffset;

        public bool Present
        {
            get { return !Equals(None); }
        }

        private bool Equals(TupleLocation other)
        {
            return TableOffset == other.TableOffset && Depth == other.Depth;
        }
    }
}