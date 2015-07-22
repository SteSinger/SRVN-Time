using SRVN_time.View;
using System;
using System.Collections.ObjectModel;
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
        ObservableCollection<TimeSpan> times;

        public RaceControl(ObservableCollection<TimeSpan> times)
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
            if (!string.IsNullOrWhiteSpace(txtRace.Text))
            {
                if (race.IsValid())
                {
                    txtRace.Text = race.Name;
                    string path = Properties.Settings.Default.savePath;
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        try
                        {
                            using (var file = new StreamWriter(path + "\\" + race.Name, false, Encoding.ASCII))
                            {
                                foreach (TimeSpan ts in times)
                                {
                                    file.WriteLine(ts.ToString(@"mm\:ss\.ff"));
                                }
                                success = true;
                            }
                        }
                        catch (UnauthorizedAccessException e)
                        {

                        }
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
            if (e.Key == Key.Enter)
            {
                saveTimes();
            }
        }

        private void raceTimes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                ListBox lb = sender as ListBox;
                int index = lb.SelectedIndex;

                if (index >= 0)
                {
                    times.RemoveAt(index);
                }

                if (times.Count > 0)
                {
                    if (index > 0)
                    {
                        lb.SelectedIndex = index - 1;
                    }
                    else if (index < times.Count)
                    {
                        lb.SelectedIndex = index;
                    }

                    ListBoxItem item = lb.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
                    item.Focus();
                }

                
            }
        }

        private void ListBoxItem_MouseDoubleClick(object sender, EventArgs e)
        {
            ListBoxItem li = sender as ListBoxItem;
            var ts = (TimeSpan)li.Content;
            DisplayOffset(ts);
        }

        private void ListBoxItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                ListBoxItem li = sender as ListBoxItem;
                var ts = (TimeSpan)li.Content;
                DisplayOffset(ts);
            }
        }

        private void DisplayOffset(TimeSpan ts)
        {
            if (times.Contains(ts))
            {
                int index = -1;
                for (int i = 0; i < times.Count; i++)
                {
                    if (ts.Equals(times[i]))
                    {
                        index = i;
                    }
                }
                Point p = this.PointToScreen(new Point(0, 0));

                TimeOffset offset = new TimeOffset() { Left = p.X, Top = p.Y };

                if (offset.ShowDialog() == true)
                {
                    if (offset.Add)
                    {
                        ts = ts.Add(offset.Time);
                    }
                    else
                    {
                        ts = ts.Subtract(offset.Time);
                    }
                    times[index] = ts;
                    txtRace.Background = null;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtRace.Focus();
        }

        private void btnOffset_Click(object sender, RoutedEventArgs e)
        {
            Point p = this.PointToScreen(new Point(0, 0));

            TimeOffset offset = new TimeOffset() { Left = p.X, Top = p.Y };

            if (offset.ShowDialog() == true)
            {
                for (int i = 0; i < times.Count; i++)
                {
                    TimeSpan ts = times[i];
                    if (offset.Add)
                    {
                        ts = ts.Add(offset.Time);
                    }
                    else
                    {
                        ts = ts.Subtract(offset.Time);
                    }
                    times[i] = ts;
                }

                txtRace.Background = null;
            }
        }
    }
}
