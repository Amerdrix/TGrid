using System.Threading;

namespace Amerdrix.TGrid.Storage
{
    internal class TupleContainer<TTuple>
    {
        public readonly TTuple Tuple;
        private int _removeCount;
        private int _writeCount;

        public TupleContainer(TTuple tuple)
        {
            Tuple = tuple;
        }

        private TupleContainer(TTuple tuple, int writeCount, int removeCount)
        {
            Tuple = tuple;
            _writeCount = writeCount;
            _removeCount = removeCount;
        }

        public int RemoveCount
        {
            get { return _removeCount; }
        }

        public int WriteCount
        {
            get { return _writeCount; }
        }

        public void DangouroslyIncrementWriteCount()
        {
            Interlocked.Increment(ref _writeCount);
        }

        public void DangouroslyIncrementRemoveCount()
        {
            Interlocked.Increment(ref _removeCount);
        }

        public TupleContainer<TTuple> IncrementWriteCount()
        {
            return new TupleContainer<TTuple>(Tuple, WriteCount + 1, RemoveCount);
        }

        public TupleContainer<TTuple> IncrementRemoveCount()
        {
            return new TupleContainer<TTuple>(Tuple, WriteCount + 1, RemoveCount);
        }
    }
}