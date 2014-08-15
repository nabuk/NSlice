using NSliceTests.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSliceTests.TestData
{
    public static class ChunkTestCaseSource
    {
        public static IEnumerable<ChunkTestCase> GenerateTestCases()
        {
            var countRange = Enumerable.Range(0, 10).Concat(Enumerable.Range(1014, 20));
            var chunkSizeRange = Enumerable.Range(1, 5).Concat(Enumerable.Range(1014, 20));

            return (from count in countRange
                    from chunkSize in chunkSizeRange
                    select new ChunkTestCase
                    {
                        Count = count,
                        ChunkSize = chunkSize
                    });
        }
    }
}
