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

            var fromValue = from ?? (stepValue > 0 ? 0 : count - 1);

            if (stepValue > 0)
            {
                if (fromValue < 0)
                {
                    fromValue += count;
                    if (fromValue < 0)
                    {
                        if (stepValue == 1)
                            fromValue = 0;
                        else
                        {
                            fromValue %= stepValue;
                            if (fromValue < 0)
                                fromValue += stepValue;
                        }
                    }
                }

                if (fromValue >= count)
                {
                    result.count = 0;
                    return result;
                }

                if (to.HasValue)
                {
                    var toValue = to.Value;
                    if (toValue < 0)
                    {
                        toValue += count;
                    }

                    if (toValue <= fromValue)
                    {
                        result.count = 0;
                        return result;
                    }
                    if (toValue >= count)
                    {
                        result.count = (count - fromValue + stepValue - 1) / stepValue;
                    }
                    else
                    {
                        result.count = (toValue - fromValue + stepValue - 1) / stepValue;
                    }
                }
                else
                {
                    result.count = (count - fromValue + stepValue - 1) / stepValue;
                }
            }
            else
            {
                var absStep = -stepValue;
                if (fromValue < 0)
                {
                    fromValue += count;
                    if (fromValue < 0)
                    {
                        result.count = 0;
                        return result;
                    }
                }
                else if (fromValue >= count)
                {
                    var offset = fromValue % absStep;
                    if (offset >= count)
                    {
                        result.count = 0;
                        return result;
                    }

                    fromValue = ((count - offset - 1) / absStep) * absStep + offset;
                }

                if (to.HasValue)
                {
                    var toValue = to.Value;
                    if (toValue < 0)
                        toValue += count;

                    if (toValue >= fromValue)
                    {
                        result.count = 0;
                        return result;
                    }

                    if (toValue < 0)
                    {
                        result.count = (fromValue + absStep) / absStep;
                    }
                    else
                    {
                        result.count = (fromValue - toValue - 1 + absStep) / absStep;
                    }
                }
                else
                {
                    result.count = (fromValue + absStep) / absStep;
                }
            }

            result.from = fromValue;
            result.step = stepValue;
            return result;
        }
    }
}
