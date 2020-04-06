using System;
using System.Diagnostics;

namespace TaskManager.Models
{
    class ThreadModel
    {
        private ProcessThread _thread;
        private String _startTime;

        public ThreadModel(ProcessThread thread)
        {
            _thread = thread;

            try
            {
                _startTime = _thread.StartTime.ToString();  //not every thread could be accessed even in administrator mode
            }
            catch(Exception s)
            {
                _startTime = String.Empty;
            }
        }

        public int Id
        {
            get { return _thread.Id; }
        }

        public System.Diagnostics.ThreadState ThreadState
        {
            get { return _thread.ThreadState; }
        }

        public String StartTime
        {
            get { return _startTime; }
        }
    }
}
