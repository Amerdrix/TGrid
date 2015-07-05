namespace Amerdrix.TGrid.Storage
{
    using System;
    using System.Collections.Generic;

    public interface ITupleFactory
    {
        ITuple Create(IList<object> items);
    }

    internal class TupleFactory : ITupleFactory
    {
        public ITuple Create(IList<Object> items)
        {
            var lookup = Lookup.Root;
            for (int i = 0; i < items.Count; i++)
            {
                lookup = lookup.Deeper(items[i].GetType());
            }

            return (ITuple)lookup.FactoryMethod.DynamicInvoke(items);
        }

        private class Lookup
        {
            public static readonly Lookup Root = new Lookup(null, null);

            private readonly Dictionary<Type, Lookup> nested = new Dictionary<Type, Lookup>();

            private Delegate factoryMethod;

            private Lookup(Type type, Lookup parent)
            {
            }

            public Delegate FactoryMethod
            {
                get
                {
                    return this.factoryMethod ?? (this.factoryMethod = this.CreateFactoryMethod());
                }
            }

            public Lookup Deeper(Type nextType)
            {
                Lookup next;
                if (this.nested.TryGetValue(nextType, out next))
                {
                    return next;
                }

                next = new Lookup(nextType, this);
                this.nested[nextType] = next;
                return next;
            }

            private Delegate CreateFactoryMethod()
            {
                return (Func<IList<object>, object>)(items => new BoxTuple(items));
            }
        }
    }
}