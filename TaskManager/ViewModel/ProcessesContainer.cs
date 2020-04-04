
using CSharpLab4.Tools.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.Models;

namespace TaskManager.ViewModel
{
    class ProcessesContainer : BaseViewModel
    {
        private ObservableCollection<ProcessAccess> _processes;
        private ProcessAccess _selectedProcess;
        private readonly object _processesLock = new object();

        #region PublicMembers
        public ProcessesContainer()
        {
            InitializeAll();
        }

        public ProcessAccess SelectedProcess
        {
            get { return _selectedProcess; }
            set { _selectedProcess = value; }
        }

        public ObservableCollection<ProcessAccess> Processes
        {
            get {
                return _processes; 
            }
        }

        public void KillSelectedProcess()
        {
            
            SelectedProcess.ProcessObj.Kill();
            _processes.Remove(SelectedProcess);
        }

        public void OpenFolderOfSelectedProcess()
        {
            Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + SelectedProcess.SourceFileFullPath));
        }
        #endregion

        #region PrivateMembers
        private void InitializeAll()
        {
            _processes = new ObservableCollection<ProcessAccess>();
            ProcessAccess.TotalPhysicalRam = GetTotalPhysicalRam();
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName != "Idle"&& process.ProcessName != "System")
                {
                    foreach (ManagementObject obj in GetProcessesManagmentCollection((UInt32)process.Id)) {
                        _processes.Add(new ProcessAccess(process, GetProcessAdditionalInfo(obj)));
                    }
                } 
            }
            StartTracking();
        }

        private UInt64 GetTotalPhysicalRam()
        {
            ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);
            UInt64 ram = 0;
            foreach (ManagementObject item in searcher.Get())
            {
                ram = (UInt64)item["TotalPhysicalMemory"];
            }
            return ram;
        }


        private async void StartTracking()
        {
            ScanNewOrDeletedProcesses(); // ManagementEventWatcher works already in different thread(no need to run different process)
            await Task.Run(() => ProcessesRefreshAsync());  // async, to start refresh timer in different thread
        }

        private void ScanNewOrDeletedProcesses()  
        {
            //   https://docs.microsoft.com/ru-ru/dotnet/api/system.management.wqleventquery.withininterval?view=netframework-4.8

            WqlEventQuery instanceCreationMonitorQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 5), "TargetInstance isa \"Win32_Process\"");
            ManagementEventWatcher instanceCreationWatcher = new ManagementEventWatcher();
            instanceCreationWatcher.Query = instanceCreationMonitorQuery;
            instanceCreationWatcher.EventArrived += new EventArrivedEventHandler(ProcessStartEvent);
            instanceCreationWatcher.Start();


            WqlEventQuery instanceDeletionQuery = new WqlEventQuery("__InstanceDeletionEvent", new TimeSpan(0, 0, 5), "TargetInstance isa \"Win32_Process\"");
            ManagementEventWatcher instanceDeletionWatcher = new ManagementEventWatcher();
            instanceDeletionWatcher.Query = instanceDeletionQuery;
            instanceDeletionWatcher.EventArrived += new EventArrivedEventHandler(ProcessStopEvent);
            instanceDeletionWatcher.Start();
        }

        private void ProcessStartEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject newEvent = e.NewEvent;
            ManagementBaseObject targetInstanceBase = (ManagementBaseObject)newEvent["TargetInstance"];
            UInt32 processId = (UInt32)targetInstanceBase.Properties["ProcessId"].Value;

            ManagementObjectCollection foundProcessesList = GetProcessesManagmentCollection(processId);
            
            foreach (ManagementObject obj in foundProcessesList)
            {
                
                lock (_processesLock)
                {
                    try
                    {
                        Process catchedProcess = Process.GetProcessById(Convert.ToInt32(processId));
                        string[] additionalInfo = GetProcessAdditionalInfo(obj);
                        ProcessAccess newProcess = new ProcessAccess(catchedProcess, additionalInfo);
                        if (!newProcess.ProcessObj.HasExited)
                        {
                            Application.Current.Dispatcher.BeginInvoke(new Action(() => _processes.Add(newProcess)));
                        }
                    }
                    catch(System.ArgumentException) { }
                }
            }
        }

        private ManagementObjectCollection GetProcessesManagmentCollection(UInt32 processId)
        {
            ObjectQuery processQueryObject = new ObjectQuery();  // create query to get process with needed id
            processQueryObject.QueryString = "Select * From Win32_Process WHERE ProcessId = " + processId;

            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher();  //creating object searcher with new query
            processSearcher.Query = processQueryObject;

            ManagementObjectCollection foundProcessesList = processSearcher.Get();
            return foundProcessesList; // get processes with id
        }

        private void ProcessStopEvent(object sender, EventArrivedEventArgs e)
        {
            lock (_processesLock)
            {
                ManagementBaseObject newEvent = e.NewEvent;
                ManagementBaseObject targetInstance = (ManagementBaseObject)newEvent["TargetInstance"];
                UInt32 processId = (UInt32)targetInstance.Properties["ProcessId"].Value;
                foreach (ProcessAccess stoppedProcess in _processes.ToList())
                {
                    if((UInt32)stoppedProcess.ProcessId == processId)
                    {
                        Application.Current.Dispatcher.BeginInvoke
                        (new Action(() => _processes.Remove(stoppedProcess)));
                    }
                }
                
            }
            
        }

        private string[] GetProcessAdditionalInfo(ManagementObject obj)
        {
           string[] result = new string[2];
            #region OwnerName
            string[] argList = new string[] { string.Empty, string.Empty };
            int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
            if (returnVal == 0){ result[0] = argList[0]; }
            else { result[0] = ""; }
            #endregion

            #region SourceFile
             result[1] = (string)obj["ExecutablePath"];
            #endregion

            return result;
        }


        private void ProcessesRefreshAsync()
        {
            System.Timers.Timer updateProcessParamsTimer = new System.Timers.Timer(); //Таймер обновления
            updateProcessParamsTimer.Elapsed += (sender, eventArgs) => RefreshProcessesData(sender, eventArgs);
            updateProcessParamsTimer.Interval = 2000;
            updateProcessParamsTimer.Enabled = true;
            updateProcessParamsTimer.Start();
        }

        private void RefreshProcessesData(object sender, EventArgs timerArguments)
        {
            lock (_processesLock)
            {
                var models = _processes.ToList();
                foreach (ProcessAccess processModel in models)
                {
                    if (!processModel.ProcessObj.HasExited)
                    {
                        processModel.CpuLoadPercentage = processModel.CpuLoadCounter.NextValue();
                        processModel.RamLoadVolume = processModel.RamLoadCounter.NextValue();
                        processModel.ThreadsNumber = processModel.ProcessObj.Threads.Count;
                        processModel.Responding = processModel.ProcessObj.Responding;
                    }
                }
            }
        }
        #endregion

    }
}
