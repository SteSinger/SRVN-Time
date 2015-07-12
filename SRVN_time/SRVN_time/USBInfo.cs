using System;
using System.Collections.Generic;
using System.Text;

namespace SRVN_time
{
    public class USBInfo
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
    }
}
