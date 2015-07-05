namespace Amerdrix.TGrid.Storage
{
    internal class TupleNode
    {
        public ITuple Tuple { get; set; }
        public TupleNode NextNode { get; set; }
    }
}