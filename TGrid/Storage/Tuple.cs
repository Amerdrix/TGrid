using System.Collections.Generic;
using System.Linq;

namespace Amerdrix.TGrid.Storage
{
    public class Tuple
    {
        public Tuple(IEnumerable<object> content)
        {
            Content = content.ToArray();
        }

        public object[] Content { get; private set; }
    }
}