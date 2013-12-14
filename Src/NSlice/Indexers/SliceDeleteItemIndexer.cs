using NSlice.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Indexers
{
    class SliceDeleteItemIndexer<T> : IItemIndexer<T>
    {
        private readonly IList<T> _source;
        private readonly int _from;
        private readonly int _step;
        private readonly int _skipCount;
        private readonly int _takeCount;
        private readonly int _boundary;

        public SliceDeleteItemIndexer(IList<T> source, int? from, int? to, int step)
        {
            this._source = source;

            var slice = SlicePropertiesCalculator.Calculate(from, to, step, source.Count);
            if (slice.count == 0 || slice.from >= source.Count)
            {
                this._from = source.Count;
                this._step = 1;
                this._skipCount = 0;
                this._takeCount = source.Count;
            }
            else
            {
                slice = SlicePropertiesCalculator.Abs(slice);

                this._from = slice.from;
                this._step = slice.step;
                this._skipCount = slice.count;
                this._boundary = slice.from + (slice.count - 1) * (slice.step - 1);
                this._takeCount = source.Count - Math.Min(slice.count, (source.Count - slice.from + slice.step - 1) / slice.step);
            }
        }

        public T GetItemAt(int index)
        {
            if (index >= this._from)
            {
                if (index >= this._boundary || this._step == 1)
                    index += this._skipCount;
                else
                {
                    int rem;
                    var div = Math.DivRem(index - this._from, this._step - 1, out rem);
                    index = div * this._step + rem + this._from + 1;
                }
            }

            return _source[index];
        }

        public int Count
        {
            get { return this._takeCount; }
        }
    }
}
