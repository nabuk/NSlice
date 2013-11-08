using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NSliceTests.TestData
{
    public class SliceTestCaseSource : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var from = new object[]
            {
                1, 2, 3, 10, 11, 12, -1, -2, -3, -10, -11, -12, null
            };
            var to = new object[]
            {
                1, 2, 3, 10, 11, 12, -1, -2, -3, -10, -11, -12, null
            };
            var step = new object[]
            {
                1, 2, 3, -1, -2, -3
            };
            var length = new object[]
            {
                0, 1, 2, 3, 9, 10
            };

            return
                (from f in @from
                 from t in to
                 from s in step
                 from l in length
                 select new [] {f, t, s, l})
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}