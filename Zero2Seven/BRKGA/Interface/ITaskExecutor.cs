using System;
using System.Diagnostics.CodeAnalysis;

namespace BRKGA.Interface
{
    public interface ITaskExecutor
    {
        TimeSpan Timeout { get; set; }
        bool IsRunning { get; }
        void Add(Action task);
        void Clear();
        bool Start();
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Justification = "there is no better name")]
        void Stop();
    }
}
