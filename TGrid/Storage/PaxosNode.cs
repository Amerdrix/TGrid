using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace Amerdrix.TGrid.Storage
{
    internal class PaxosNode : IStorageEngine
    {
        // Must be pow 2;
        private const long AssumedMaxWritesPerTick = 0xFF;
        private readonly UdpClient _acceptClient;
        private readonly UdpClient _acceptedClient;
        private readonly long _nodeCount;
        private readonly long _nodeId;
        private readonly Dictionary<long, TupleContainer<ITuple>> _pendingProposals = new Dictionary<long, TupleContainer<ITuple>>();
        private readonly UdpClient _prepareClient;
        private readonly UdpClient _promiseClient;
        private long _versionIncrementer;

        public PaxosNode(PaxosConfig paxosConfig, IEnumerable<PaxosConfig> otherNodeConfigs)
        {
            Engine = new StorageEngine();

            var otherNodes = otherNodeConfigs.Where(x => paxosConfig.PreparePort != x.PreparePort).ToArray();

            _nodeCount = otherNodes.Length + 1;
            _nodeId = paxosConfig.ClientNumber;

            _prepareClient = new UdpClient(paxosConfig.PreparePort);
            _prepareClient.BeginReceive(PrepareMessageReceived, null);

            _promiseClient = new UdpClient(paxosConfig.PromisePort);
            _promiseClient.BeginReceive(PromiseMessageReceived, null);

            _acceptClient = new UdpClient(paxosConfig.AcceptPort);
            _acceptClient.BeginReceive(AcceptMessageReceived, null);

            _acceptedClient = new UdpClient(paxosConfig.AcceptedPort);
            _acceptedClient.BeginReceive(AcceptedMessageReceived, null);
        }

        public StorageEngine Engine { get; private set; }

        public void Put(ITuple tuple)
        {
            var container = Engine.GetOrCreateContainer(tuple);
            var updatedContainer = container.IncrementWriteCount();

            SubmitProposal(updatedContainer);


            //Engine.Put(tuple);
        }

        public ITuple Read(MatchPattern match)
        {
            return Engine.Read(match);
        }

        public ITuple Take(MatchPattern match)
        {
            var container = Engine.GetContainer(match);
            var updatedContainer = container.IncrementRemoveCount();

            SubmitProposal(updatedContainer);
            return null;
        }

        private long GetNextVersionNumber()
        {
            return (Stopwatch.GetTimestamp()*_nodeCount + _nodeId)*AssumedMaxWritesPerTick + GetIncrementerValue();
        }

        private long GetIncrementerValue()
        {
            return Interlocked.Increment(ref _versionIncrementer) & AssumedMaxWritesPerTick;
        }

        private void SubmitProposal(TupleContainer<ITuple> container)
        {
            var version = GetNextVersionNumber();

            _pendingProposals[version] = container;
        }

        private void AcceptedMessageReceived(IAsyncResult ar)
        {
            _acceptedClient.BeginReceive(AcceptedMessageReceived, null);
        }

        private void AcceptMessageReceived(IAsyncResult ar)
        {
            _acceptClient.BeginReceive(AcceptMessageReceived, null);
        }

        private void PromiseMessageReceived(IAsyncResult ar)
        {
            _promiseClient.BeginReceive(PromiseMessageReceived, null);
        }

        private void PrepareMessageReceived(IAsyncResult ar)
        {
            _prepareClient.BeginReceive(PrepareMessageReceived, null);
        }

        public class PaxosConfig
        {
            public int PreparePort { get; set; }
            public int PromisePort { get; set; }
            public int AcceptPort { get; set; }
            public int AcceptedPort { get; set; }
            public int ClientNumber { get; set; }
        }
    }
}