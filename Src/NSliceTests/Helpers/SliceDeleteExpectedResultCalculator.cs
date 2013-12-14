using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSliceTests.Helpers
{
    public static class SliceDeleteExpectedResultCalculator
    {
        public static IList<int> Calculate(int? from, int? to, int? step, int length)
        {
            var stepValue = step ?? 1;
            var fromValue = from ?? (stepValue > 0 ? 0 : -1);
            if (fromValue < 0) fromValue = length + fromValue;
            var sanitizedTo = to;
            if (to < 0) sanitizedTo = length + to.Value;
            var toValue = sanitizedTo ?? (stepValue > 0 ? length : -1);
            if (stepValue > 0)
            {
                if (fromValue < 0)
                    fromValue = 0;
                if (toValue > length)
                    toValue = length;
            }
            else
            {
                if (fromValue >= length)
                    fromValue = length - 1;
                if (toValue < -1)
                    toValue = -1;
            }


            var deleted = new HashSet<int>();
            for (var i = fromValue; stepValue > 0 ? i < toValue : i > toValue; i += stepValue)
                if (i >= 0 && i < length)
                    deleted.Add(i);

            return Enumerable.Range(0, length)
                .Where(x => !deleted.Contains(x))
                .ToArray();
        }
    }
}
