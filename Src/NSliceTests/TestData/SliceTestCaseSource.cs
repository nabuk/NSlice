using NSliceTests.Dto;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NSliceTests.TestData
{
    public static class SliceTestCaseSource
    {
        public static IEnumerable<SliceTestCase> GenerateTestCases()
        {
            var iteratorRange = Enumerable.Range(-12, 25).Select(x => (int?)x).Concat(new int?[] { null });
            var stepRange = Enumerable.Range(1, 5).Concat(Enumerable.Range(-5, 5));
            var lengthRange = Enumerable.Range(0, 13);

            return (from @from in iteratorRange
                         from to in iteratorRange
                         from step in stepRange
                         from length in lengthRange
                         select new SliceTestCase
                         {
                             From = @from,
                             To = to,
                             Step = step,
                             Length = length
                         });
        }
    }
}