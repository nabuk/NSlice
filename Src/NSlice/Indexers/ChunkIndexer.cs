using NSlice.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Indexers
{
    class ChunkIndexer<T> : IItemIndexer<IList<T>>
    {
        private readonly IList<T> _source;
        private readonly int _count;
        private readonly int _chunkSize;
        private readonly int _baseChunkCount;
        private readonly int _lastChunkSize;

        public ChunkIndexer(IList<T> source, int chunkSize)
        {
            this._source = source;
            this._chunkSize = chunkSize;
            this._baseChunkCount = source.Count / chunkSize;
            this._lastChunkSize = source.Count % chunkSize;
            this._count = _baseChunkCount +  (_lastChunkSize == 0 ? 0 : 1);
        }

        public IList<T> GetItemAt(int index)
        {
            return new ProxiedReadOnlyList<T>(new SegmentIndexer<T>(_source, index * _chunkSize, index != _baseChunkCount ? _chunkSize : _lastChunkSize));
        }

        public int Count
        {
            get { return this._count; }
        }
    }
}
