using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NSlicePerformanceTests.Tests;

namespace NSlicePerformanceTests.Markdown
{
    static class TestRenderers
    {
        public static IMarkdown ToMarkdown(this ITest test)
        {
            return Markdown.Create(() =>
                test.Name.ToMarkdown()
                .Add(test.Name != string.Empty ? ": " : string.Empty)
                .Add(test.Code.ToMarkdown().InlineCode())
                .NewLine()
                .Add(string.Format("{0:0} ms", test.ExecutionTime).ToMarkdown().Bold()));
        }

        public static IMarkdown ToMarkdown(this ITestCase testCase, CultureInfo culture)
        {
            Func<IMarkdown> renderGain = () =>
            {
                var testExecutionTime = testCase.Test.ExecutionTime;
                var benchmarkExecutionTime = testCase.Benchmark.ExecutionTime;
                if (testExecutionTime < 1.0 || benchmarkExecutionTime < 1.0)
                    return Markdown.Empty;
                var gain = (testExecutionTime/benchmarkExecutionTime) - 1.0;
                gain *= 100.0;
                if (Math.Abs(gain) < 5.0)
                    return Markdown.Empty;
                return
                    Markdown.Empty
                    .Add("  ")
                    .Add(
                        Markdown.Create(() => string.Format(culture, "{0:+0.0;-0.0;0.0}%", gain))
                        .InlineCode());
            };
            return Markdown.Create(() =>
            {
                var result = testCase.Name.ToMarkdown();
                if (testCase.Benchmark != null)
                    result = result.Add(renderGain());
                result = result.H3().NewLine().NewLine();
                IEnumerable<ITest> tests = new[] { testCase.Test };
                if (testCase.Benchmark != null)
                    tests = tests.Concat(new[] {testCase.Benchmark});
                return result.Add(tests.Select(t => t.ToMarkdown().NewLine()).AsLines());
            });
        }
    }
}
