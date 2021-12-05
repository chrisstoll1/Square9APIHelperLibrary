using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    internal class Register
    {
        public Register(string uniqueId = "", string serial = "", string registration = "") 
        { 
            UniqueId = uniqueId;
            Serial = serial;
            Registration = registration;
        }
        public string UniqueId { get; set; }
        public string Serial { get; set; }
        public string Registration { get; set; }
    }
}
