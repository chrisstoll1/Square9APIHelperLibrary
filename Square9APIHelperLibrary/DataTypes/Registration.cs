using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Registration
    {
        public Registration() { }
        public IDictionary<string, string> Features { get; set; }
        public string Product { get; set; }
        public string RegNumber { get; set; }
        public int RegType { get; set; }
        public string Serial { get; set; }
        public string UID { get; set; }
    }
}
