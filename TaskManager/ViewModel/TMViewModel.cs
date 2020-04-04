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
