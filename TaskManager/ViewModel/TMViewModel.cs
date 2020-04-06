using CSharpLab4.Tools.MVVM;
using System.Collections.ObjectModel;

namespace TaskManager.ViewModel
{
    class TMViewModel : BaseViewModel
    {
        private ProcessesContainer _container;
        private RelayCommand<object> _killCommand;
        private RelayCommand<object> _openFolderCommand;



        public TMViewModel()
        {
            _container = new ProcessesContainer();
        }

        #region ViewPropertiesAndCommands
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
        #endregion

        private bool IsSelected()
        {
            return SelectedItem != null;
        }

        private void KillProcess(object obj)
        {
            _container.KillSelectedProcess();
        }

        private void OpenProcessFolder(object obj)
        {
            _container.OpenFolderOfSelectedProcess();
        }
    }
}
