using System;
using System.Collections.Generic;
using System.Linq;

namespace NSlicePerformanceTests.Clock
{
    //Original implementation:
    //http://stackoverflow.com/questions/969290/exact-time-measurement-for-performance-testing#answer-16157458

    static class Performance
    {
        public static double BenchmarkTime(Action action, int iterationsPerSample = 1, int sampleCount = 5)
        {
            //clean Garbage
            GC.Collect();

            //wait for the finalizer queue to empty
            GC.WaitForPendingFinalizers();

            //clean Garbage
            GC.Collect();

            //warm up
            action();

            var watch = new TimeWatch();
            var timings = new double[sampleCount];
            for (var i = 0; i < timings.Length; i++)
            {
                watch.Reset();
                watch.Start();
                for (var j = 0; j < iterationsPerSample; j++)
                    action();
                watch.Stop();
                timings[i] = watch.Elapsed.TotalMilliseconds;
            }
            return timings.NormalizedMean();
        }

        private static double NormalizedMean(this ICollection<double> values)
        {
            if (values.Count == 0)
                return double.NaN;

            var deviations = values.Deviations().ToArray();
            var meanDeviation = deviations.Sum(t => Math.Abs(t.Item2)) / values.Count;
            return deviations.Where(t => t.Item2 > 0 || Math.Abs(t.Item2) <= meanDeviation).Average(t => t.Item1);
        }

        private static IEnumerable<Tuple<double, double>> Deviations(this ICollection<double> values)
        {
            if (values.Count == 0)
                yield break;

            var avg = values.Average();
            foreach (var d in values)
                yield return Tuple.Create(d, avg - d);
        }
    }
}
