using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            DialogResult = TimeSpan.TryParseExact(txtTime.Text, @"mm\:ss\,ff", null, out ts);
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
