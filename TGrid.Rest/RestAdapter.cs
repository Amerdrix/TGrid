namespace Amerdrix.TGrid.Rest
{
    using System;
    using System.Linq;

    using Amerdrix.TGrid.Logging;
    using Amerdrix.TGrid.Plugins;
    using Amerdrix.TGrid.Storage;

    using Nancy;
    using Nancy.Hosting.Self;

    public class RestAdapter : NancyModule, IAdapter
    {
        private NancyHost _host;

        public static ITupleFactory Factory { get; private set; }

        public static ILogger Logger { get; private set; }

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
                    {
                        return HttpStatusCode.NotFound;
                    }
                    return tuple.Content;
                };

            Get["/"] = _ =>
                {
                    var match = ParseMatchPattern();
                    var tuple = Binding.Read(match);

                    if (tuple == null)
                    {
                        return HttpStatusCode.NotFound;
                    }
                    return tuple.Content;
                };
        }

        private static IStorageEngine Binding { get; set; }

        public void Dispose()
        {
            if (this._host != null)
            {
                this._host.Dispose();
            }
        }

        public void Register(IStorageEngine storageEngine, ITupleFactory factory, ILogger logger)
        {
            Logger = logger;
            Binding = storageEngine;
            

            Factory = factory;

            this.StartHost(1337);
        }

        private void StartHost(int port)
        {
            Logger.Info("Starting REST adapter on port {0}", port);

            var config = new HostConfiguration { UrlReservations = { CreateAutomatically = true } };
            this._host = new NancyHost(config, new Uri(string.Format("http://localhost:{0}", port)));
            this._host.Start();
            Logger.Info("REST adapter started on port {0}", port);
        }

        private MatchPattern ParseMatchPattern()
        {
            string[] q = Request.Query.T.Value.Split(',');

            var p = q.Select(
                i =>
                    {
                        if (i == "")
                        {
                            return MatchPattern.MatchAny;
                        }
                        return int.Parse(i);
                    });

            var match = new MatchPattern(p);
            return match;
        }

        private ITuple ParseTuple()
        {
            string[] q = Request.Query.T.Value.Split(',');
            var p = q.Select(int.Parse).Cast<object>();
            return Factory.Create(p.ToList());
        }
    }
}