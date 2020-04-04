using CSharpLab4.Tools.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TaskManager.Models;

namespace TaskManager.ViewModel
{
    class ProcessAccess : BaseViewModel
    {
        private ProcessModel _processModel;
        private static UInt64 _totalPhysicalRam;

        public ProcessAccess(Process newProcess, string[] additionalInfo)
        {
            _processModel = new ProcessModel(newProcess);
            UserOwnerName = additionalInfo[0];
            SourceFileFullPath = additionalInfo[1];
        }

        public static UInt64 TotalPhysicalRam
        {
            set { _totalPhysicalRam = value; }
        }

        public Process ProcessObj
        {
            get { return _processModel.ProcessObj; }
            set { _processModel.ProcessObj = value; }
        }

        public ProcessThreadCollection ThreadsCollection
        {
            get { return _processModel.ThreadsCollection; }
        }
        public ProcessModuleCollection ModulesCollection
        {
            get { return _processModel.ModulesCollection; }
        }

        public string ProcessName
        {
            get { return _processModel.ProcessName; }
            set { _processModel.ProcessName = value; }
        }
        public int ProcessId
        {
            get { return _processModel.ProcessId; }
            set { _processModel.ProcessId = value; }
        }
        public bool Responding
        {
            get { return _processModel.Responding; }
            set { 
                _processModel.Responding = value;
                OnPropertyChanged();
            }
        }
        #region RAM_and_CPU
        public PerformanceCounter CpuLoadCounter
        {
            get { return _processModel.CpuLoadCounter; }
        }
        public PerformanceCounter RamLoadCounter
        {
            get { return _processModel.RamLoadCounter; }
        }
        public float CpuLoadPercentage
        {
            get { return _processModel.CpuLoadPercentage; }
            set {
                _processModel.CpuLoadPercentage = (float)Math.Round(value, 2);
                OnPropertyChanged();
            }
        }
        public float RamLoadPercentage
        {
            get { return _processModel.RamLoadPercentage; }
            set { 
                _processModel.RamLoadPercentage = (float)Math.Round((value * 100) / _totalPhysicalRam, 1);
                OnPropertyChanged();
            }
        }
        public float RamLoadVolume
        {
            get { return _processModel.RamLoadVolume; }
            set {
                RamLoadPercentage = value;
                _processModel.RamLoadVolume = (float)Math.Round(value / 1024 / 1024, 2);
                OnPropertyChanged();
            }
        }
        #endregion

        public int ThreadsNumber
        {
            get { return _processModel.ThreadsNumber; }
            set { 
                _processModel.ThreadsNumber = value;
                OnPropertyChanged();
            }
        }
        public string UserOwnerName
        {
            get { return _processModel.UserOwnerName; }
            set { _processModel.UserOwnerName = value; }
        }
        public string SourceFile
        {
            get { return _processModel.SourceFile; }
            set { _processModel.SourceFile = value; }
        }
        public string SourceFileFullPath
        {
            get { return _processModel.SourceFileFullPath; }
            set {
                _processModel.SourceFileFullPath = value;
                if (!String.IsNullOrEmpty(value))
                {
                    int position = value.LastIndexOf('\\');
                    SourceFile = value.Substring(position + 1);
                }
            }
        }
        public DateTime StartTime
        {
            get { return _processModel.StartTime; }
            set { _processModel.StartTime = value; }
        }
    }
}
