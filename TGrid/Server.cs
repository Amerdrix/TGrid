using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace TGrid
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
        public void Load(){

            var binding = new StorageEngine();

            foreach(var adapter in Scan().SelectMany(t => t.GetTypes())
                                        .Where(t => t.GetInterfaces().Any(i => i == typeof(IAdapter)))
                                        .Select(t => Activator.CreateInstance(t) as IAdapter))
            {
                adapter.Register(binding);
            }
        }

        private IEnumerable<Assembly> Scan()
        {
            var path = System.IO.Path.GetFullPath( @"..\..\..\TGrid.Rest\bin\Debug\Amerdrix.TGrid.Rest.Dll");
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

    class StorageEngine : IStorageEngine 
    {
        private readonly IList<Tuple> _tuples = new List<Tuple>();

        public void Put(Tuple tuple)
        {
            _tuples.Add(tuple);
        }

        public Tuple Read(MatchPattern match)
        {
            var pattern = match.Pattern.ToList();
           return  _tuples.FirstOrDefault(t => Match(t, pattern));
            
        }

        public Tuple Take(MatchPattern match)
        {
            var tuple = Read(match);
            if(tuple != null)
                _tuples.Remove(tuple);
            return tuple;
        }

        private bool Match(Tuple tuple, IList<object> match)
        {
            for (var i = 0; i < tuple.Content.Length; i ++){
                if (match[i] == MatchPattern.MatchAny)
                    continue;
                if (!tuple.Content[i].Equals(match[i]))
                    return false;
            }
            return true;
        }
    }
}
