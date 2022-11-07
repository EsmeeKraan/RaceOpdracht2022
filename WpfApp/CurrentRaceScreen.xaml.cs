using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
