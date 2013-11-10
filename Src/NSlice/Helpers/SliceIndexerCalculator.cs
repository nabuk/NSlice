using System;
using NSlice.Indexers;

namespace NSlice.Helpers
{
    static class SliceIndexerCalculator
    {
        internal static StepIndexer Calculate(int? from, int? to, int? step, int count)
        {
            var stepValue = step ?? 1;
            if (stepValue == 0)
                throw new ArgumentException("Step cannot be zero.");

            var result = new StepIndexer();

            if (count == 0)
            {
                result.count = 0;
                return result;
            }

            int fromValue, toValue;
            if (stepValue > 0)
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

                result.count = (toValue - fromValue + stepValue - 1) / stepValue;
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

                var absStep = -stepValue;
                result.count = (fromValue - toValue - 1 + absStep) / absStep;
            }

            result.from = fromValue;
            result.step = stepValue;
            return result;
        }
    }
}
