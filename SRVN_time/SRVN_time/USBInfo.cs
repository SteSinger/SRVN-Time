using System;
using System.Collections.Generic;
using System.Text;

namespace SRVN_time
{
    public class USBInfo : IComparable
    {
        string _port;

        public USBInfo(string port)
        {
            _port = port;
        }

        public string Port
        {
            get
            {
                return _port;
            }

            private set
            {
                _port = value;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is USBInfo)
            {
                var info = obj as USBInfo;
                return _port.CompareTo(info.Port);
            }

            throw new ArgumentException();
        }
    }
}
