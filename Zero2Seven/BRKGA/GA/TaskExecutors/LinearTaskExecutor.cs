using System;

namespace BRKGA.GA.TaskExecutors
{
    public class LinearTaskExecutor : TaskExecutorBase
    {
        public override bool Start()
        {
            var startTime = DateTime.Now;
            base.Start();

            foreach (var t in Tasks)
            {
                if (StopRequested)
                {
                    return true;
                }

                t();

                if ((DateTime.Now - startTime) > Timeout)
                {
                    return false;
                }
            }

            IsRunning = false;
            return true;
        }
    }
}
