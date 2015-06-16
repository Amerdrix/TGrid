namespace Amerdrix.TGrid.Storage
{
    internal class TupleNode
    {
        public Tuple Tuple { get; set; }
        public TupleNode NextNode { get; set; }
    }
}