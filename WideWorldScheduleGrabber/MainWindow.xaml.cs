using ExternalInteractions;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WideWorldScheduleGrabber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IWideWorldReader wwr;
        private ComboBox SessionBox;
        private ComboBox SessionTypeBox;
        private ComboBox DivisionBox;
        private List<Link> DivisionList;
        private ComboBox TeamBox;
        private List<Link> TeamList;
        private List<SoccerGame> Schedule;
        private Button SaveButton;


        public MainWindow()
        {
            InitializeComponent();
            wwr = new WideWorldReader();
        }

        private void SessionBox_Loaded(object sender, RoutedEventArgs e)
        {
            SessionBox = sender as ComboBox;

            SessionBox.ItemsSource = wwr.GetSeasons();
        }

        private void SessionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string selectedSession = comboBox.SelectedItem as string;
            SessionTypeBox.ItemsSource = wwr.GetScheduleTypes(selectedSession);
            SessionTypeBox.IsEnabled = true;
            SessionTypeBox.SelectedIndex = -1;
            DivisionBox.IsEnabled = false;
            DivisionBox.SelectedIndex = -1;
            TeamBox.IsEnabled = false;
            TeamBox.SelectedIndex = -1;
        }

        private void SessionTypeBox_Loaded(object sender, RoutedEventArgs e)
        {
            SessionTypeBox = sender as ComboBox;
        }

        private void SessionTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string selectedSessionType = comboBox.SelectedItem as string;
            if (comboBox.SelectedIndex >= 0)
            {
                DivisionList = wwr.GetDivisions(SessionBox.SelectedItem as string, selectedSessionType);
                DivisionBox.ItemsSource = DivisionList.Select(d => d.Name);
                DivisionBox.IsEnabled = true;
                DivisionBox.SelectedIndex = -1;
            }
            TeamBox.IsEnabled = false;
            TeamBox.SelectedIndex = -1;
        }

        private void DivisionBox_Loaded(object sender, RoutedEventArgs e)
        {
            DivisionBox = sender as ComboBox;
        }

        private void DivisionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string selectedDivision = comboBox.SelectedItem as string;
            if (comboBox.SelectedIndex >= 0)
            {
                TeamList = wwr.GetTeams(DivisionList.Single(d => d.Name == selectedDivision));
                TeamBox.ItemsSource = TeamList.Select(d => d.Name);
                TeamBox.IsEnabled = true;
                TeamBox.SelectedIndex = -1;
            }
        }

        private void TeamBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            SaveButton.IsEnabled = comboBox.SelectedIndex >= 0;
        }

        private void TeamBox_Loaded(object sender, RoutedEventArgs e)
        {
            TeamBox = sender as ComboBox;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            var schedule = wwr.GetSchedule(TeamList.Single(t => t.Name == TeamBox.SelectedItem as string));
            SaveButton.IsEnabled = true;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            SaveButton = sender as Button;
        }
    }
}
