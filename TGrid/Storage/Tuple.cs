using System.Collections.Generic;
using System.Linq;

namespace Amerdrix.TGrid.Storage
{
    public class Tuple
    {
        public Tuple(params object[] content)
        {
            Content = content;
        }

        public Tuple(IEnumerable<object> content)
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