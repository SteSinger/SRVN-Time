using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SRVN_time
{
    /// <summary>
    /// Interaction logic for RaceControl.xaml
    /// </summary>
    public partial class RaceControl : UserControl
    {

        Race race = new Race("");
        List<TimeSpan> times;

        public RaceControl(List<TimeSpan> times)
        {
            InitializeComponent();

            raceTimes.ItemsSource = times;
            raceTimes.ItemStringFormat = "mm\\:ss\\.ff";
            this.times = times;

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

                using (var file = new StreamWriter(race.Name, false, Encoding.ASCII))
                    {
                        foreach (TimeSpan ts in times)
                        {
                            file.WriteLine(ts.ToString(@"mm\:ss\.ff"));
                        }
                        success = true;
                    }
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
