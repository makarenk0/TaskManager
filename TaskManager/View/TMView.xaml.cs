using System.Windows.Controls;
using TaskManager.ViewModel;

namespace TaskManager.View
{
    /// <summary>
    /// Логика взаимодействия для TMView.xaml
    /// </summary>
    public partial class TMView : UserControl
    {
        public TMView()
        {
            DataContext = new TMViewModel();
            InitializeComponent();
        }
    }
}
