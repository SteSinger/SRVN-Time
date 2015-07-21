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
using System.IO.Ports;
using System.Diagnostics;
using System.Windows.Threading;

namespace SRVN_time
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort port = new SerialPort("COM4", 9600);
        List<TimeSpan> list = new List<TimeSpan>();
        char timeStopped = '#';
        public MainWindow()
        {
            InitializeComponent();

            port.DataReceived += new SerialDataReceivedEventHandler(DataHandler);
            port.Open();
        }

        

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.ShowDialog();
        }

        private void DataHandler(object sender, SerialDataReceivedEventArgs args)
        {
            SerialPort sp = (SerialPort)sender;
            string time = sp.ReadExisting();
            time = time.TrimEnd(new char[]{'\r', '\n' });


            if(timeStopped == time[0])
            {
                Dispatcher disp = panel.Dispatcher;

                disp.Invoke(new Action(() =>
                {
                    panel.Children.Insert(0, new RaceControl(list));
                }));

                
                list = new List<TimeSpan>();
            }
            else
            {
                if (time[1] == ':')
                {
                    // add leading zero
                    time = "0" + time;
                }

                TimeSpan ts = TimeSpan.ParseExact(time, "mm\\:ss\\,ff", null);
                list.Add(ts);
                
                Trace.WriteLine(ts.ToString("mm\\:ss\\.ff"));
            }
            
            

        }
    }
}
