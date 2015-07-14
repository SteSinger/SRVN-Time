using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SRVN_time
{
    /// <summary>
    /// Interaction logic for RaceControl.xaml
    /// </summary>
    public partial class RaceControl : UserControl
    {
        public RaceControl(List<TimeSpan> times)
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var race = new Race(txtRace.Text);
            
        }

    }
}
