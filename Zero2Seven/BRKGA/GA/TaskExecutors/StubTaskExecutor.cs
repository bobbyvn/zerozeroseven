using System;
using System.Collections.Generic;

namespace BRKGA.GA.TaskExecutors
{
    public class StubTaskExecutor : TaskExecutorBase
    {
        public IList<Action> GetTasks()
        {
            return Tasks;
        }

        public bool GetStopRequested()
        {
            return StopRequested;
        }
    }
}
