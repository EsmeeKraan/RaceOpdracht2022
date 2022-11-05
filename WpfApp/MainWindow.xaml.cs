using Controller;
using Model;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CurrentCompetitionScreen _currentCompetitionScreen = new CurrentCompetitionScreen();
        private CurrentRaceScreen _currentRaceScreen = new CurrentRaceScreen();
        public MainWindow()
        {
            MainWindowInitialize();
        }

        public void MainWindowInitialize()
        {
            Data.Initialize();
            Data.NextRaceEvent += OnNextRaceEvent; // <--- geen delegate maar een event
            Data.CurrentRace.DriversChanged += OnDriversChanged;

            InitializeComponent();
        }

        public void OnNextRaceEvent(object? sender, NextRaceEventArgs e)
        {
            //Geen Intialize die wel in VisualisatieStatic
            //Hoe implementeer ik mijn events
            //RaceChanged een delegated van maken???
            createImage.ClearImageDictionary();
            Data.CurrentRace.DriversChanged += OnDriversChanged;
        }

        public void OnDriversChanged(object? sender, DriversChangedEventArgs e)
        {
            TrackImage.Dispatcher.BeginInvoke(
            DispatcherPriority.Render,
            new Action(() =>
            {
                TrackImage.Source = null;
                TrackImage.Source = WPFVisualisatie.DrawTrack(e.Track);
            }));
        }

        private void MenuItem_Open_CurrentRaceScreen(object sender, RoutedEventArgs e)
        {
            _currentRaceScreen.Show();
        }

        private void MenuItem_Open_CurrentCompetitionScreen(object sender, RoutedEventArgs e)
        {
            _currentCompetitionScreen.Show();
        }
    }
}
