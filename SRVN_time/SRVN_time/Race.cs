using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SRVN_time
{
    public class Race
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public Race(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Tries to create a correct ecup string
        /// Format: RxxxF-x
        /// or      RxxxV-x
        /// </summary>
        public void BuildCorrectString()
        {
            if(_name.Length < 3 || IsValid())
            {
                // need at least 3 chars to get a valid race
                return;
            }
            _name = _name.ToUpperInvariant();
            char firstChar = _name[0];
            if(firstChar != 'R')
            {
                _name = 'R' + _name;
            }

            int length = 0;
            for (int i = 1; i <= 3; i++)
            {
                int result = 0;
                var subStr = _name.Substring(i, 1).ToUpper();
                bool valid = int.TryParse(subStr, out result);

                
                if((subStr == "F" || subStr == "V") && i+1 < _name.Length && (_name.Substring(i+1, 1).ToUpper() != "F" && _name.Substring(i+1, 1).ToUpper() != "V"))
                {
                    break;
                }
                else if (Regex.IsMatch(subStr, "[0-9a-zA-Z]"))
                {
                    length++;
                }

            }

            // insert leading zeroes
            for(int i = length; i < 3; i++)
            { 
                _name = _name.Insert(1, "0");
            }

            if(_name.Length <= 5)
            {
                // 
                return;
            }

            // capitalize f or v
            char raceType = _name[4];
            if(raceType != 'F' && raceType != 'V')
            {
                 //can't determine race type impossible to create correct string
                return;
            }

            char seperator = _name[5];
            if (seperator != '-')
            {
                _name = _name.Insert(5, "-");
            }

        }

        public bool IsValid()
        {
            return IsValid(_name);
        }

        public bool IsValid(string raceName)
        {
            return raceName.Length == 7 || Regex.IsMatch(raceName, "R[0-9a-zA-Z]{3}[FV]-[0-9]");
        }
    }
}
