using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Indexers
{
    interface IItemIndexer<T>
    {
        T GetItemAt(int index);
        int Count { get; }
    }
}
