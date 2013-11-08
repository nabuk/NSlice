using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NSliceTests.TestData
{
    public class AtOrDefaultTestCaseSource : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var index = new object[]
            {
                0, 1, 2, 3, 4, 5, -1, -2, -3, -4, -5, -6
            };
            var length = new object[]
            {
                6
            };

            return
                (from i in index
                 from l in length
                 select new[] { i, l })
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
