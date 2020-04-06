using System;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace TaskManager.Models
{
    class ProcessModel
    {
        private Process _process;
        

        private ObservableCollection<ThreadModel> _threads;  //not ProcessThreadsCollection because want to override properties to handle StartTimeException (Win32AccessDenied)
        private ProcessModuleCollection _modules;

        private string _processName;
        private int _id;
        private bool _responding;

        private readonly PerformanceCounter _cpuLoadCounter;
        private readonly PerformanceCounter _ramLoadCounter;

        private float _cpuLoadPercentage;
        private float _ramLoadPercentage;
        private float _ramLoadVolume;

        private int _threadsNumber;
        private string _userOwnerName;
        private string _sourceFile;
        private string _sourceFileFullPath;
        private DateTime _startTime;

        public ProcessModel(Process process)
        {
            ProcessObj = process;
           
            ProcessName = process.ProcessName;
            ProcessId = process.Id;
            Responding = process.Responding;
            _cpuLoadCounter = new PerformanceCounter("Process", "% Processor Time", ProcessName, true);
            _ramLoadCounter = new PerformanceCounter("Process", "Working Set", ProcessName, true);
            ThreadsNumber = process.Threads.Count;
            StartTime = process.StartTime;

            _threads = new ObservableCollection<ThreadModel>();

            foreach (ProcessThread t in process.Threads)
            {
                _threads.Add(new ThreadModel(t));
            }
        }

        public Process ProcessObj
        {
            get { return _process; }
            set { _process = value; }
        }

        public ObservableCollection<ThreadModel> ThreadsCollection
        {
            get { return _threads; }
        }
        public ProcessModuleCollection ModulesCollection
        {
            get { return _modules; }
            set { _modules = value; }
        }

        public string ProcessName {
            get{ return _processName; }
            private set { _processName = value; }
        }
        public int ProcessId
        {
            get { return _id; }
            private set { _id = value; }
        }
        public bool Responding
        {
            get { return _responding; }
            set { _responding = value; }
        }
        #region ChangingParametrs
        public PerformanceCounter CpuLoadCounter
        {
            get { return _cpuLoadCounter; }
        }
        public PerformanceCounter RamLoadCounter
        {
            get { return _ramLoadCounter; }
        }
        public float CpuLoadPercentage
        {
            get { return _cpuLoadPercentage; }
            set { _cpuLoadPercentage = value; }
        }
        public float RamLoadPercentage
        {
            get { return _ramLoadPercentage; }
            set { _ramLoadPercentage = value; }
        }
        public float RamLoadVolume
        {
            get { return _ramLoadVolume; }
            set { _ramLoadVolume = value; }
        }
        #endregion

        public int ThreadsNumber
        {
            get { return _threadsNumber; }
            set { _threadsNumber = value; }
        }
        public string UserOwnerName
        {
            get { return _userOwnerName; }
            set { _userOwnerName = value; }
        }
        public string SourceFile
        {
            get { return _sourceFile; }
            set { _sourceFile = value; }
        }
        public string SourceFileFullPath
        {
            get { return _sourceFileFullPath; }
            set { _sourceFileFullPath = value; }
        }
        public DateTime StartTime
        {
            get { return _startTime; }
            private set { _startTime = value; }
        }
    }
}
