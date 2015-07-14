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

        Race race = new Race("");

        public RaceControl(List<TimeSpan> times)
        {
            InitializeComponent();

            raceTimes.ItemsSource = times;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            saveTimes();
        }


        private void saveTimes()
        {

            race.Name = txtRace.Text;
            race.BuildCorrectString();


            bool success = false;
            if (!String.IsNullOrEmpty(txtRace.Text))
            {
                


                if (race.IsValid())
                {
                    txtRace.Text = race.Name;
                    success = true;
                }
            }

            if (success)
            {
                txtRace.Background = Brushes.LightGreen;
            }
            else
            {
                txtRace.Background = Brushes.IndianRed;
            }
        }

        private void txtRace_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter || e.Key == Key.Return)
            {
                saveTimes();
            }
        }
    }
}
