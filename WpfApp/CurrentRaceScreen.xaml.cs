using System.Windows;
using System.Windows.Threading;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for CurrentRaceScreen.xaml
    /// </summary>
    public partial class CurrentRaceScreen : Window
    {
        public CurrentRaceScreen()
        {
            InitializeComponent();
            ((MainWindowDataContext)DataContext).Dispatcher = (action =>
            {
                Dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
            });
        }
    }
}
