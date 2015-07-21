using System;
using System.Windows;
using System.IO.Ports;
using System.Diagnostics;
using System.Windows.Threading;
using System.Collections.ObjectModel;

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
        public MainWindow()
        {
            InitializeComponent();

            port.DataReceived += new SerialDataReceivedEventHandler(DataHandler);
            port.BaudRate = 9600;
        }

        

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowSettings();
        }

        private void ShowSettings(bool force = false)
        {
            SettingsWindow settings = new SettingsWindow(port.PortName, force);
            if (settings.ShowDialog() == true)
            {
                var info = settings.usbDevices.SelectedItem as USBInfo;
                if (info != null && !info.Port.Equals(port.PortName))
                {
                    port.Close();
                    port.PortName = info.Port;
                    port.Open();
                }
            }
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

        private void Window_Closed(object sender, EventArgs e)
        {
            port.Close();
            port.Dispose();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            ShowSettings(true);
        }
    }
}
