using Amerdrix.TGrid.Storage;
using NUnit.Framework;

namespace Amerdrix.TGrid.Tests
{
    [TestFixture]
    public class PutTests
    {
        [Test]
        public void PutOnce()
        {
            var engine = new StorageNode();
            Assert.DoesNotThrow(() => { engine.Put(new BoxTuple(1, 2)); });
        }

        [Test]
        public void PutTwice()
        {
            var engine = new StorageNode();


            Assert.DoesNotThrow(() =>
            {
                engine.Put(new BoxTuple(1, 2));
                engine.Put(new BoxTuple(1, 2));
            });
        }
    }
}