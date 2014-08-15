using NSliceTests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSliceTests.Tests.Base
{
    public abstract class BaseDragCaseTests
    {
        protected delegate void DragTestCaseDelegate(int count, int numberOfItemsToDrag);

        protected void RunNoArgumentDragTestCases(DragTestCaseDelegate testBody)
        {
            DragTestCaseSource.GenerateTestCases(false).AsParallel()
                .ForAll(dtc => testBody(dtc.Count, dtc.NumberOfItemsToDrag));
        }

        protected void RunDragTestCasesWithNumberOfItemsToDrag(DragTestCaseDelegate testBody)
        {
            DragTestCaseSource.GenerateTestCases(true).AsParallel()
                .ForAll(dtc => testBody(dtc.Count, dtc.NumberOfItemsToDrag));
        }

        protected bool CheckSut(IEnumerable<IList<int>> sut, int count, int numberOfItemsToDrag)
        {
            var sutArray = sut.ToArray();
            if (sutArray.Length != Math.Max(count - numberOfItemsToDrag + 1, 0))
                return false;

            for (int i = 0; i < sutArray.Length; ++i)
            {
                if (sutArray[i].Count != numberOfItemsToDrag)
                    return false;

                for (int j = 0; j < numberOfItemsToDrag; ++j)
                {
                    if (sutArray[i][j] != i + j)
                        return false;
                }
            }

            return true;
        }
    }
}