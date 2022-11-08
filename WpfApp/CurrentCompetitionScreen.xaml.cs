using System.Windows;
using System.Windows.Threading;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for CurrentCompetitionScreen.xaml
    /// </summary>
    public partial class CurrentCompetitionScreen : Window
    {
        public CurrentCompetitionScreen()
        {
            InitializeComponent();
            ((MainWindowDataContext)DataContext).Dispatcher = (action =>
            {
                Dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
            });
        }
    }
}
