using Amerdrix.TGrid.Storage;
using NUnit.Framework;

namespace Amerdrix.TGrid.Tests
{
    [TestFixture]
    public class TakeTests
    {
        [Test]
        public void PutOnceTakeOnceExact()
        {
            var engine = new StorageNode();

            engine.Put(new BoxTuple(1, 2));

            var result = engine.Take(new MatchPattern(1, 2));

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [Test]
        public void PutOnceTakeOnceNonExact()
        {
            var engine = new StorageNode();

            engine.Put(new BoxTuple(1, 2));

            var result = engine.Take(new MatchPattern(1, MatchPattern.MatchAny));

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [Test]
        public void PutOnceTakeTwiceExact()
        {
            var engine = new StorageNode();

            engine.Put(new BoxTuple(1, 2));
            engine.Take(new MatchPattern(1, 2));

            var result = engine.Take(new MatchPattern(1, 2));

            Assert.IsNull(result);
        }
        
        [Test]
        public void PutOnceTakeTwiceNonExact()
        {
            var engine = new StorageNode();

            engine.Put(new BoxTuple(1, 2));
            engine.Take(new MatchPattern(MatchPattern.MatchAny, 2));

            var result = engine.Take(new MatchPattern(1, MatchPattern.MatchAny));

            Assert.IsNull(result);
        }
    }
}