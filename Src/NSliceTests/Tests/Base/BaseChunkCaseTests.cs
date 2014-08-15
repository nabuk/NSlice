using NSliceTests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSliceTests.Tests.Base
{
    public abstract class BaseChunkCaseTests
    {
        protected const string chunkResultErrorFormat = "For Range(0, {0}).Chunk({1}) got unexpected result.";

        protected delegate void ChunkTestCaseDelegate(int count, int chunkSize);

        protected void RunChunkTestCases(ChunkTestCaseDelegate testBody)
        {
            ChunkTestCaseSource.GenerateTestCases().AsParallel()
                .ForAll(dtc => testBody(dtc.Count, dtc.ChunkSize));
        }

        protected bool CheckSut(IEnumerable<IList<int>> sut, int count, int chunkSize)
        {
            var sutArray = sut.ToArray();
            if (sutArray.Length != Math.Max(count / chunkSize + (count % chunkSize == 0 ? 0 : 1), 0))
                return false;

            for (int i = 0; i < sutArray.Length; ++i)
            {
                if (i == count / chunkSize)
                {
                    var expectedCount = count % chunkSize;
                    if (expectedCount == 0 || sutArray[i].Count != expectedCount)
                        return false;
                }
                else
                    if (sutArray[i].Count != chunkSize)
                        return false;

                for (int j = 0; j < sutArray[i].Count; ++j)
                {
                    if (sutArray[i][j] != i * chunkSize + j)
                        return false;
                }
            }

            return true;
        }
    }
}
