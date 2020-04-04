using CSharpLab4.Tools.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TaskManager.Models;

namespace TaskManager.ViewModel
{
    class TMViewModel : BaseViewModel
    {
        private ProcessesContainer _container;
        private RelayCommand<object> _killCommand;
        private RelayCommand<object> _openFolderCommand;

        public RelayCommand<object> KillCommand
        {
            get
            {
                return _killCommand ?? (_killCommand = new RelayCommand<object>(KillProcess,
                    o => IsSelected()));
            }
        }

        public RelayCommand<object> OpenFolderCommand
        {
            get
            {
                return _openFolderCommand ?? (_openFolderCommand = new RelayCommand<object>(OpenProcessFolder,
                    o => IsSelected()));
            }
        }

        private bool IsSelected()
        {
            return SelectedItem != null;
        }

        public void KillProcess(object obj)
        {
            _container.KillSelectedProcess();
        }

        public void OpenProcessFolder(object obj)
        {
            _container.OpenFolderOfSelectedProcess();
        }

        public TMViewModel()
        {
            _container = new ProcessesContainer();
        }


        public ObservableCollection<ProcessAccess> AllProcesses
        {
            get { return _container.Processes; }
        }

        public ProcessAccess SelectedItem
        {
            get { return _container.SelectedProcess; }
            set 
            { 
                _container.SelectedProcess = value;
                OnPropertyChanged();
            }
        }
    }
}
