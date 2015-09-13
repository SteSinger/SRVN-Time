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
    public partial class MainWindow : Window, IDisposable
    {
        SerialPort port = new SerialPort();
        ObservableCollection<TimeSpan> list = new ObservableCollection<TimeSpan>();
        char timeStopped = '#';
        private string backlog;
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
                        port.Dispose();
                    }));
                }
            };

            list.Add(new TimeSpan(0, 2, 10));
            var raceC = new RaceControl(list);
            raceC.Focus();
            panel.Children.Insert(0, raceC);
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
            string input = backlog + sp.ReadExisting();
            

            string[] times = input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


            for (int i = 0; i < times.Length; i++)
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

                    Trace.WriteLine("Race finished");
                }
                else
                {
                    if (time[1] == ':')
                    {
                        // add leading zero
                        time = "0" + time;
                    }
                    TimeSpan ts;
                    bool valid = TimeSpan.TryParseExact(time, "mm\\:ss\\,ff", null, out ts);

                    if (valid)
                    {
                        list.Add(ts);
                        Trace.WriteLine(ts.ToString("mm\\:ss\\.ff"));
                    }
                    else
                    {
                        Trace.WriteLine("time not valid " + time);
                        backlog = time;
                    }                    
                }
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowSettings(true);
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    watcher.Dispose();
                    port.Close();
                    port.Dispose();
                }

                watcher = null;
                port = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dispose();
        }
    }
}
