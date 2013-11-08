using System;
using System.Diagnostics;
using System.Threading;

namespace NSlicePerformanceTests.Clock
{
    //Original implementation:
    //http://stackoverflow.com/questions/969290/exact-time-measurement-for-performance-testing#answer-16157458

    class TimeWatch
    {
        readonly Stopwatch _stopwatch = new Stopwatch();

        public TimeSpan Elapsed
        {
            get { return _stopwatch.Elapsed; }
        }

        public bool IsRunning
        {
            get { return _stopwatch.IsRunning; }
        }



        public TimeWatch()
        {
            if (!Stopwatch.IsHighResolution)
                throw new NotSupportedException("Your hardware doesn't support high resolution counter");

            //prevent the JIT Compiler from optimizing Fkt calls away
            long seed = Environment.TickCount;

            //use the second Core/Processor for the test
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);

            //prevent "Normal" Processes from interrupting Threads
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            //prevent "Normal" Threads from interrupting this thread
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }



        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public void Reset()
        {
            _stopwatch.Reset();
        }
    }
}
