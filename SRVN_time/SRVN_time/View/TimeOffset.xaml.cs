using System;
using System.Windows;
using System.Windows.Input;

namespace SRVN_time.View
{
    /// <summary>
    /// Interaction logic for TimeOffset.xaml
    /// </summary>
    public partial class TimeOffset : Window
    {
        private bool add = true;
        private TimeSpan ts;

        public bool Add
        {
            get
            {
                return add;
            }

            set
            {
                add = value;
            }
        }

        public TimeSpan Time
        {
            get
            {
                return ts;
            }

            set
            {
                ts = value;
            }
        }

        public TimeOffset()
        {
            InitializeComponent();

            txtTime.Focus();
        }

        public TimeOffset(TimeSpan ts)
        {
            InitializeComponent();

            txtTime.Focus();
            txtTime.Text = ts.ToString(@"mm\:ss\.ff");
            btnAdd.Visibility = Visibility.Hidden;
            btnSubt.Visibility = Visibility.Hidden;
        }

        private void btnSubt_Click(object sender, RoutedEventArgs e)
        {
            SubtractTime();
        }

        private void SubtractTime()
        {
            add = false;
            ParseTime();
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTime();
        }

        private void AddTime()
        {
            add = true;
            ParseTime();
            Close();
        }

        private void ParseTime()
        {
            DialogResult = TimeSpan.TryParseExact(txtTime.Text, @"mm\:ss\.ff", null, out ts);
        }

        private void txtTime_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    DialogResult = false;
                    Close();
                    break;
                case Key.Add:
                case Key.OemPlus:
                case Key.Return:
                    AddTime();
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    SubtractTime();
                    break;
                default:
                    break;
            }
        }
    }
}
