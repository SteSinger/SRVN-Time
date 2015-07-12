using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Management;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO.Ports;

namespace SRVN_time
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private List<USBInfo> usbStrings = new List<USBInfo>();

        public SettingsWindow()
        {
            InitializeComponent();

            FillUsbList();
        }

        public List<USBInfo> UsbStrings
        {
            get
            {
                return usbStrings;
            }

            set
            {
                usbStrings = value;
            }
        }

        private void FillUsbList()
        {


            /*            ManagementObjectCollection collection;
                        using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                        {
                            collection = searcher.Get();
                        }

                        foreach (var device in collection)
                        {
                            usbStrings.Add( String.Format("{0} {1} {2}", (string)device.GetPropertyValue("DeviceID"), (string)device.GetPropertyValue("PNPDeviceID"), (string)device.GetPropertyValue("Description")));

                        }
                        */

            foreach (var s in SerialPort.GetPortNames())
            {
                usbStrings.Add(new USBInfo(s));
                Trace.WriteLine(s);
            }
            

            /*foreach (var s in usbStrings)
                Trace.WriteLine(s);
                */
        }

        private void usbDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
