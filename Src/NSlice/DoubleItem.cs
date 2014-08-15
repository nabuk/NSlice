using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice
{
    public class DoubleItem<T>
    {
        public DoubleItem(T first, T second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; private set; }

        public T Second { get; private set; }
    }
}
