using System;
using Amib.Threading;

namespace BRKGA.GA.TaskExecutors
{
    public sealed class SmartThreadPoolTaskExecutor : TaskExecutorBase, IDisposable
    {
        public SmartThreadPoolTaskExecutor()
        {
            MinThreads = 2;
            MaxThreads = 2;
        }

        public int MinThreads { get; set; }

        public int MaxThreads { get; set; }

        public override bool Start()
        {
            base.Start();
            _threadPool = new SmartThreadPool();

            try
            {
                _threadPool.MinThreads = MinThreads;
                _threadPool.MaxThreads = MaxThreads;
                var workItemResults = new IWorkItemResult[Tasks.Count];

                for (int i = 0; i < Tasks.Count; i++)
                {
                    var t = Tasks[i];
                    workItemResults[i] = _threadPool.QueueWorkItem(new WorkItemCallback(Run), t);
                }

                _threadPool.Start();

                // Timeout was reach?
                if (!_threadPool.WaitForIdle(Timeout.TotalMilliseconds > int.MaxValue ? int.MaxValue : Convert.ToInt32(Timeout.TotalMilliseconds)))
                {
                    if (_threadPool.IsShuttingdown)
                    {
                        return true;
                    }
                    else
                    {
                        _threadPool.Cancel(true);
                        return false;
                    }
                }

                foreach (var wi in workItemResults)
                {
                    System.Exception ex;
                    wi.GetResult(out ex);

                    if (ex != null)
                    {
                        throw ex;
                    }
                }

                return true;
            }
            finally
            {
                _threadPool.Shutdown(true, 1000);
                _threadPool.Dispose();
                IsRunning = false;
            }
        }

        public override void Stop()
        {
            base.Stop();

            if (_threadPool != null && !_threadPool.IsShuttingdown)
            {
                _threadPool.Shutdown(true, Timeout);
            }

            IsRunning = false;
        }

        public void Dispose()
        {
            if (_threadPool != null)
            {
                _threadPool.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        private object Run(object state)
        {
            ((System.Action)state)();

            return true;
        }

        private SmartThreadPool _threadPool;
    }
}
