using System;
using System.Collections.Generic;
using BRKGA.Interface;

namespace BRKGA.GA
{
    public abstract class TaskExecutorBase : ITaskExecutor
    {
        protected TaskExecutorBase()
        {
            Tasks = new List<Action>();
            Timeout = TimeSpan.MaxValue;
        }

        public TimeSpan Timeout { get; set; }
        public bool IsRunning { get; protected set; }
        protected IList<Action> Tasks { get; private set; }
        protected bool StopRequested { get; private set; }
        public void Add(Action task)
        {
            Tasks.Add(task);
        }
        public void Clear()
        {
            Tasks.Clear();
        }
        public virtual bool Start()
        {
            lock (_lock)
            {
                StopRequested = false;
                IsRunning = true;
            }

            return true;
        }

        public virtual void Stop()
        {
            lock (_lock)
            {
                StopRequested = true;
            }
        }

        private object _lock = new object();
    }
}
