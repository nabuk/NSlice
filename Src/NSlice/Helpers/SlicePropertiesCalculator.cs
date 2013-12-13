using System;
using NSlice.Indexers;

namespace NSlice.Helpers
{
    static class SlicePropertiesCalculator
    {
        internal static SliceProperties Calculate(int? from, int? to, int step, int count)
        {
            if (step == 0)
                throw new ArgumentException("Step cannot be zero.");

            var result = new SliceProperties();

            if (count == 0)
            {
                result.count = 0;
                return result;
            }

            int fromValue, toValue;
            if (step > 0)
            {
                fromValue = from ?? 0;
                toValue = to ?? count;

                if (fromValue < 0)
                {
                    fromValue += count;
                    if (fromValue < 0)
                        fromValue = 0;
                }
                else if (fromValue >= count)
                    return result;

                if (toValue < 0)
                    toValue += count;
                if (toValue <= fromValue)
                    return result;
                if (toValue > count)
                    toValue = count;

                result.count = (toValue - fromValue + step - 1) / step;
            }
            else
            {
                fromValue = from ?? count - 1;

                if (fromValue < 0)
                {
                    fromValue += count;
                    if (fromValue < 0)
                        return result;
                }
                else if (fromValue >= count)
                    fromValue = count - 1;

                if (to < 0)
                    to += count;

                toValue = to ?? -1;

                if (toValue < -1)
                    toValue = -1;
                else if (toValue >= fromValue)
                    return result;

                var absStep = -step;
                result.count = (fromValue - toValue - 1 + absStep) / absStep;
            }

            result.from = fromValue;
            result.step = step;
            return result;
        }

        internal static SliceProperties Abs(SliceProperties stepIndexer)
        {
            if (stepIndexer.step < 0)
            {
                stepIndexer.step = -stepIndexer.step;
                stepIndexer.from -= stepIndexer.step * stepIndexer.count;
            }

            return stepIndexer;
        }
    }
}
