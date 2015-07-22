using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace SRVN_time
{
    public sealed class USBWatcher : IDisposable
    {
        private ManagementEventWatcher plugInWatcher;
        private ManagementEventWatcher unPlugWatcher;
        public delegate void USBUnplugged(string name);
        public delegate void USBPlugged(string name);

        public USBPlugged usbPlugged;
        public USBUnplugged usbUnplugged;


        public void Dispose()
        {
            if (plugInWatcher != null)
                try
                {
                    plugInWatcher.Dispose();
                    plugInWatcher = null;
                }
                catch (Exception) { }

            if (unPlugWatcher == null) return;
            try
            {
                unPlugWatcher.Dispose();
                unPlugWatcher = null;
            }
            catch (Exception) { }
        }

        public void Start()
        {
            const string plugInSql = "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PnPEntity'";
            const string unpluggedSql = "SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PnPEntity'";

            var scope = new ManagementScope("root\\CIMV2") { Options = { EnablePrivileges = false } };

            var pluggedInQuery = new WqlEventQuery(plugInSql);
            plugInWatcher = new ManagementEventWatcher(scope, pluggedInQuery);
            plugInWatcher.EventArrived += HandlePluggedInEvent;
            plugInWatcher.Start();

            var unPluggedQuery = new WqlEventQuery(unpluggedSql);
            unPlugWatcher = new ManagementEventWatcher(scope, unPluggedQuery);
            unPlugWatcher.EventArrived += HandleUnPluggedEvent;
            unPlugWatcher.Start();
        }

        private void HandleUnPluggedEvent(object sender, EventArrivedEventArgs e)
        {
            var description = GetDeviceName(e.NewEvent);
            if (description.ToLower().Contains("com"))
            {
                if (usbUnplugged != null)
                {
                    usbUnplugged(description);
                }
            }
        }

        private void HandlePluggedInEvent(object sender, EventArrivedEventArgs e)
        {
            var description = GetDeviceName(e.NewEvent);
            if (description.ToLower().Contains("com"))
            {
                if (usbPlugged != null)
                {
                    usbPlugged(description);
                }
            }

        }

        private static string GetDeviceName(ManagementBaseObject newEvent)
        {
            var targetInstanceData = newEvent.Properties["TargetInstance"];
            var targetInstanceObject = (ManagementBaseObject)targetInstanceData.Value;
            if (targetInstanceObject == null) return "";

            var description = targetInstanceObject.Properties["Name"].Value.ToString();
            return description;
        }

    }


}
