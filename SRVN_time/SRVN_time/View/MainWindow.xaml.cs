using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<TimeSpan> list = new List<TimeSpan>();
            list.Add(TimeSpan.Parse("00:01:12"));

            panel.Children.Add(new RaceControl(list));
            panel.Children.Add(new RaceControl(list));
            panel.Children.Add(new RaceControl(list));
            //AddChild(new RaceControl(null));

            list.Add(TimeSpan.Parse("00:01:13"));
            list.Add(TimeSpan.Parse("00:02:12"));
            list.Add(TimeSpan.Parse("00:03:12"));
            list.Add(TimeSpan.Parse("00:04:12"));
            list.Add(TimeSpan.Parse("00:05:12"));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();
        }
    }
}
