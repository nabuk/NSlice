using NSliceTests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSliceTests.Tests.Base
{
    public abstract class BaseSliceCaseTests
    {
        protected delegate void SliceTestCaseDelegate(int? from, int? to, int step, int length);

        protected void RunSliceTestCases(SliceTestCaseDelegate testBody)
        {
            SliceTestCaseSource.GenerateTestCases().AsParallel()
                .ForAll(stc => testBody(stc.From, stc.To, stc.Step, stc.Length));
        }
    }
}
