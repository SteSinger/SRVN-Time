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
using Microsoft.WindowsAPICodePack.Dialogs;
using SRVN_time.Properties;

namespace SRVN_time
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private List<USBInfo> usbStrings = new List<USBInfo>();
        string currentDirectory = "";

        

        public SettingsWindow()
        {
            InitializeComponent();
            usbDevices.ItemsSource = UsbStrings;
            usbDevices.DisplayMemberPath = "Port";
            usbDevices.SelectedValuePath = "Port";
            
            FillUsbList();
            usbDevices.SelectedIndex = 0;
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

        public string CurrentDirectory
        {
            get
            {
                return txtFolderPath.Text;
            }

            set
            {
                txtFolderPath.Text = value;

            }
        }

        private void FillUsbList()
        {

            foreach (var s in SerialPort.GetPortNames())
            {
                usbStrings.Add(new USBInfo(s));
                Trace.WriteLine(s);
            }
        }

        private void usbDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void folderSelection_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Choose file save location";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = CurrentDirectory;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = CurrentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;
            

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                CurrentDirectory = dlg.FileName;
                
            }
            this.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
