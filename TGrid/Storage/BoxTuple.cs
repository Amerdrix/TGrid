using System.Collections.Generic;
using System.Linq;

namespace Amerdrix.TGrid.Storage
{
    public interface ITuple
    {
        object[] Content { get; }
        uint Length { get; }
        object this[int index] { get; }
    }

    internal class BoxTuple : ITuple
    {
        public BoxTuple(params object[] content)
        {
            Content = content;
        }

        public BoxTuple(IEnumerable<object> content)
        {
            Content = content.ToArray();
        }

        public object[] Content { get; private set; }

        public object this[int index]
        {
            get { return Content[index]; }
        }

        public uint Length { get { return (uint) Content.Length; } }
    }
}