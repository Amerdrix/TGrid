using System.Collections.Generic;
using System.Linq;
using Amerdrix.TGrid.Indexing;

namespace Amerdrix.TGrid.Storage
{
    internal class StorageEngine : IStorageEngine
    {
        private readonly IList<ITupleIndex> _indices = new List<ITupleIndex>();
        private readonly uint _size;
        private readonly TupleNode[] _space;

        public StorageEngine(uint size)
        {
            _indices.Add(new ScanIndex());

            _size = size;
            _space = new TupleNode[size];
        }

        public void Put(Tuple tuple)
        {
            var hash = tuple.GetHashCode();
            var offset = (int) (((hash%_size) + _size)%_size);

            var location = new TupleLocation {TableOffset = offset};


            var node = _space[offset];
            if (node == null)
            {
                var newNode = new TupleNode {Tuple = tuple};
                _space[offset] = newNode;
            }
            else
            {
                while (node.Tuple != null && node.NextNode != null)
                {
                    location.Depth++;
                    node = node.NextNode;
                }

                if (node.Tuple == null)
                {
                    node.Tuple = tuple;
                }
                else
                {
                    location.Depth ++;
                    var newNode = new TupleNode {Tuple = tuple};
                    node.NextNode = newNode;
                }
            }

            foreach (var index in _indices)
                index.Add(tuple, location);
        }

        public Tuple Read(MatchPattern match)
        {
            var bestIndex = SelectIndex(match);
            var location = bestIndex.Find(match);

            if (location.Present)
            {
                return GetNode(location).Tuple;
            }

            return null;
        }

        public Tuple Take(MatchPattern match)
        {
            var bestIndex = SelectIndex(match);
            var location = bestIndex.Find(match);

            if (location.Present)
            {
                var node = GetNode(location);

                var tuple = node.Tuple;
                node.Tuple = null;

                foreach (var index in _indices)
                    index.Remove(tuple);

                return tuple;
            }

            return null;
        }

        private TupleNode GetNode(TupleLocation location)
        {
            var node = _space[location.TableOffset];
            for (var i = 0; i < location.Depth; i++)
            {
                node = node.NextNode;
            }

            return node;
        }

        private ITupleIndex SelectIndex(MatchPattern pattern)
        {
            return _indices.OrderByDescending(x => x.Rank(pattern)).First();
        }
    }
}