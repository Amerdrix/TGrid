using System.Collections.Generic;

namespace Amerdrix.TGrid
{
    public class MatchPattern
    {
        public static readonly object MatchAny = new Any();

        public MatchPattern(params object[] pattern)
        {
            Pattern = pattern;
        }

        public MatchPattern(IEnumerable<object> pattern)
        {
            Pattern = pattern;
        }

        internal IEnumerable<object> Pattern { get; private set; }

        private class Any
        {
        }
    }
}