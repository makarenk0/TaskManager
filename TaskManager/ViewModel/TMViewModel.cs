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


        public ProcessesContainer AllProcesses
        {
            get { return _container; }
        }
    }
}
