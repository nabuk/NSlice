using NSliceTests.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSliceTests.TestData
{
    public static class DragTestCaseSource
    {
        public static IEnumerable<DragTestCase> GenerateTestCases(bool withNumberOfItemsToDrag)
        {
            var countRange = Enumerable.Range(0, 10).Concat(Enumerable.Range(1014, 20));
            var dragNumberRange = withNumberOfItemsToDrag ? Enumerable.Range(1, 5).Concat(Enumerable.Range(1014, 20)) : Enumerable.Range(2, 1);

            return (from count in countRange
                    from dragNumber in dragNumberRange
                    select new DragTestCase
                    {
                        Count = count,
                        NumberOfItemsToDrag = dragNumber
                    });
        }
    }
}
