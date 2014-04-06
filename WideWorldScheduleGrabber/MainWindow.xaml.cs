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
        public MainWindow()
        {
            InitializeComponent();
            var wwr = new WideWorldReader();
            var seasons = wwr.GetSeasons();
            var types2 = wwr.GetScheduleTypes(seasons[1]);
            var divisions = wwr.GetDivisions(seasons[1], types2.Last());
            var teams = wwr.GetTeams(divisions[5]);
            
        }
    }
}
