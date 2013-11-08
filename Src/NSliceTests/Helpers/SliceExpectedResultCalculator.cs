using System.Collections.Generic;

namespace NSliceTests.Helpers
{
    public static class SliceExpectedResultCalculator
    {
        public static IList<int> Calculate(int? from, int? to, int? step, int length)
        {
            var stepValue = step ?? 1;
            var fromValue = from ?? (stepValue > 0 ? 0 : -1);
            if (fromValue < 0) fromValue = length + fromValue;
            var sanitizedTo = to;
            if (to < 0) sanitizedTo = length + to.Value;
            var toValue = sanitizedTo ?? (stepValue > 0 ? length : -1);
            var expected = new List<int>();
            for (var i = fromValue; stepValue > 0 ? i < toValue : i > toValue; i += stepValue)
                if (i >= 0 && i < length)
                    expected.Add(i);
            return expected;
        }
    }
}
