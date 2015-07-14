using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SRVN_time
{
    public class Race
    {
        string _name;

        public Race(string name)
        {
            _name = name;
            if (!IsValid())
            {
                BuildCorrectString();
            }
        }

        private void BuildCorrectString()
        {
            if(_name.IndexOf("R") == -1)
            {

            }
        }

        public bool IsValid()
        {
            Regex rx = new Regex("R[0-9]{3}[FV]-[0-9]");
            return rx.IsMatch(_name);
        }
    }
}
