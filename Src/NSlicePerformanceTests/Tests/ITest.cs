namespace NSlicePerformanceTests.Tests
{
    interface ITest
    {
        string Name { get; }
        string Code { get; }
        double ExecutionTime { get; }
    }
}
