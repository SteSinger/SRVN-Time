using System;
using System.Windows;
using System.IO.Ports;
using System.Diagnostics;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace SRVN_time
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort port = new SerialPort();
        ObservableCollection<TimeSpan> list = new ObservableCollection<TimeSpan>();
        char timeStopped = '#';

        USBWatcher watcher = new USBWatcher();

        public MainWindow()
        {
            InitializeComponent();

            port.DataReceived += new SerialDataReceivedEventHandler(DataHandler);
            port.BaudRate = 9600;

            lblConnection.Content = "Not connected!";
            lblConnection.Foreground = Brushes.IndianRed;

            watcher.Start();
            watcher.usbUnplugged += name =>
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(port.PortName) && name.ToUpper().Contains(port.PortName))
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        lblConnection.Content = "Not connected!";
                        lblConnection.Foreground = Brushes.IndianRed;
                        port.Close();
                    }));
                }
            };
        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowSettings();
        }

        private void ShowSettings(bool force = false)
        {
            SettingsWindow settings = new SettingsWindow(port.PortName, force, watcher);
            if (settings.ShowDialog() == true)
            {

                var info = settings.usbDevices.SelectedItem as USBInfo;
                if (info != null && (force || !info.Port.Equals(port.PortName)))
                {
                    port.Close();
                    port.PortName = info.Port;
                    try
                    {
                        port.Open();
                        lblConnection.Content = "Connected on " + info.Port;
                        lblConnection.Foreground = Brushes.Green;
                    }
                    catch (System.IO.IOException e)
                    {
                        lblConnection.Content = "Not connected!";
                        lblConnection.Foreground = Brushes.IndianRed;
                    }

                }
            }
        }

        private void DataHandler(object sender, SerialDataReceivedEventArgs args)
        {
            SerialPort sp = (SerialPort)sender;
            string input = sp.ReadExisting();
            string[] times = input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


            for(int i =0; i< times.Length; i++) 
            {
                var time = times[i];
                if (timeStopped == time[0])
                {
                    Dispatcher disp = panel.Dispatcher;

                    disp.Invoke(new Action(() =>
                    {
                        var raceC = new RaceControl(list);
                        raceC.Focus();
                        panel.Children.Insert(0, raceC);

                    }));


                    list = new ObservableCollection<TimeSpan>();
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

    
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowSettings(true);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            port.Close();
            port.Dispose();
        }
    }
}
