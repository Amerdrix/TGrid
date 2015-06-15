using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Amerdrix.TGrid.Indexing;

namespace Amerdrix.TGrid
{
    internal class Server
    {
        static void Main(string[] args)
        {
            new AdapterLoader().Load();
            Console.ReadLine();
        }
    }

    class AdapterLoader
    {
        public void Load()
        {

            var binding = new StorageEngine(1);

            foreach (var adapter in Scan().SelectMany(t => t.GetTypes())
                                        .Where(t => t.GetInterfaces().Any(i => i == typeof(IAdapter)))
                                        .Select(t => Activator.CreateInstance(t) as IAdapter))
            {
                adapter.Register(binding);
            }
        }

        private IEnumerable<Assembly> Scan()
        {
            var path = System.IO.Path.GetFullPath(@"..\..\..\TGrid.Rest\bin\Debug\Amerdrix.TGrid.Rest.Dll");
            yield return System.Reflection.Assembly.LoadFile(path);
        }
    }



    public class Tuple
    {
        public object[] Content { get; private set; }
        public Tuple(IEnumerable<object> content)
        {
            Content = content.ToArray();
        }
    }

    public class MatchPattern
    {
        private class Any { }

        public readonly static object MatchAny = new Any();

        internal IEnumerable<object> Pattern { get; private set; }

        public MatchPattern(IEnumerable<object> pattern)
        {
            Pattern = pattern;
        }

    }

    public interface IAdapter : IDisposable
    {
        void Register(IStorageEngine binding);
    }


    public interface IStorageEngine
    {
        void Put(Tuple tuple);

        Tuple Read(MatchPattern match);

        Tuple Take(MatchPattern match);
    }

    class TupleNode
    {
        public Tuple Tuple { get; set; }
        public TupleNode NextNode { get; set; }
    }

    struct TupleLocation
    {
        public static readonly TupleLocation None = new TupleLocation() { TableOffset = -1, Depth = -1 };

        public int TableOffset;
        public int Depth;

        public bool Present
        {
            get
            {
                return !this.Equals(None);
            }
        }

        private bool Equals(TupleLocation other)
        {
            return this.TableOffset == other.TableOffset && this.Depth == other.Depth;
        }

    }


    class StorageEngine : IStorageEngine
    {

        private readonly IList<ITupleIndex> _indices = new List<ITupleIndex>();

        private readonly TupleNode[] _space;
        readonly uint _size;

        public StorageEngine(uint size)
        {
            _indices.Add(new ScanIndex());

            _size = size;
            _space = new TupleNode[size];
        }

        public void Put(Tuple tuple)
        {
            var hash = tuple.GetHashCode();
            var offset = (int)(((hash % _size) + _size) % _size);

            var location = new TupleLocation() { TableOffset = offset };

            
            var node = _space[offset];
            if (node == null)
            {
                var newNode = new TupleNode() { Tuple = tuple };
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
                    var newNode = new TupleNode() { Tuple = tuple };
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
    }
}
