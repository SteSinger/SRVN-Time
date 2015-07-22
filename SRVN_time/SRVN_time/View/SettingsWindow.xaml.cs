using System.Windows;
using System.IO.Ports;
using Microsoft.WindowsAPICodePack.Dialogs;
using SRVN_time.Properties;
using System.Collections.ObjectModel;
using System;

namespace SRVN_time
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private ObservableCollection<USBInfo> usbStrings = new ObservableCollection<USBInfo>();

        private USBWatcher watcher;

        public SettingsWindow(string portName, bool force, USBWatcher watcher)
        {
            InitializeComponent();

            this.watcher = watcher;
            watcher.usbPlugged += FillUsbList;
            watcher.usbUnplugged += FillUsbList;

            FillUsbList();
            usbDevices.ItemsSource = UsbStrings;
            usbDevices.DisplayMemberPath = "Port";
            usbDevices.SelectedValuePath = "Port";

            for (int i = 0; i < UsbStrings.Count; i++)
            {
                if (UsbStrings[i].Port.Equals(portName))
                {
                    usbDevices.SelectedIndex = i;
                }
            }

            if (force)
            {
                btnCancel.IsEnabled = false;
                WindowStyle = WindowStyle.None;
            }

            

        }

        public ObservableCollection<USBInfo> UsbStrings
        {
            get
            {
                return usbStrings;
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
                Settings.Default.savePath = value;
            }
        }

        private void FillUsbList(string _)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                FillUsbList();
            }));
            
        }

        private void FillUsbList()
        {
            usbStrings.Clear();
            foreach (var s in SerialPort.GetPortNames())
            {
                usbStrings.Add(new USBInfo(s));
            }
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


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Reload();
            DialogResult = false;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            watcher.usbPlugged -= FillUsbList;
            watcher.usbUnplugged -= FillUsbList;
        }
    }
}
