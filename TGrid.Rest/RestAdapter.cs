using System;
using System.Linq;
using Amerdrix.TGrid.Plugins;
using Amerdrix.TGrid.Storage;
using Nancy;
using Nancy.Hosting.Self;
using Tuple = Amerdrix.TGrid.Storage.Tuple;

namespace Amerdrix.TGrid.Rest
{
    public class RestAdapter : NancyModule, IAdapter
    {
        private NancyHost _host;

        public RestAdapter()
        {
            Put["/"] = _ =>
            {
                var t = ParseTuple();
                Binding.Put(t);
                return t.Content;
            };
            Post["/"] = _ =>
            {
                var t = ParseTuple();
                Binding.Put(t);
                return t.Content;
            };

            Delete["/"] = _ =>
            {
                var match = ParseMatchPattern();
                var tuple = Binding.Take(match);

                if (tuple == null)
                    return HttpStatusCode.NotFound;
                return tuple.Content;
            };

            Get["/"] = _ =>
            {
                var match = ParseMatchPattern();
                var tuple = Binding.Read(match);

                if (tuple == null)
                    return HttpStatusCode.NotFound;
                return tuple.Content;
            };
        }

        private static IStorageEngine Binding { get; set; }

        public void Dispose()
        {
            if (_host != null)
                _host.Dispose();
        }

        public void Register(IStorageEngine binding)
        {
            Binding = binding;

            var config = new HostConfiguration {UrlReservations = {CreateAutomatically = true}};

            _host = new NancyHost(config, new Uri("http://localhost:1337"));

            _host.Start();
        }

        private MatchPattern ParseMatchPattern()
        {
            string[] q = Request.Query.T.Value.Split(',');

            var p = q.Select(i =>
            {
                if (i == "")
                    return MatchPattern.MatchAny;
                return int.Parse(i);
            });

            var match = new MatchPattern(p);
            return match;
        }

        private Tuple ParseTuple()
        {
            string[] q = Request.Query.T.Value.Split(',');
            var p = q.Select(int.Parse).Cast<object>();
            return new Tuple(p);
        }
    }
}