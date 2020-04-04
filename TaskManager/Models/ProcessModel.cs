using CSharpLab4.Tools.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TaskManager.Models
{
    class ProcessModel
    {
        private Process _process;
        private ProcessThreadCollection _threads;

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
            if (!process.HasExited)
            {
                ProcessName = process.ProcessName;
                ProcessId = process.Id;
                Responding = process.Responding;
                _cpuLoadCounter = new PerformanceCounter("Process", "% Processor Time", ProcessName, true);
                _ramLoadCounter = new PerformanceCounter("Process", "Working Set", ProcessName, true);
                ThreadsNumber = process.Threads.Count;
                StartTime = process.StartTime;
                _threads = process.Threads;
            }

        }

        public Process ProcessObj
        {
            get { return _process; }
            set { _process = value; }
        }

        public ProcessThreadCollection ThreadsCollection
        {
            get { return _threads; }
        }

        public string ProcessName {
            get{ return _processName; }
            set { _processName = value; }
        }
        public int ProcessId
        {
            get { return _id; }
            set { _id = value; }
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
            set { _startTime = value; }
        }
    }
}
