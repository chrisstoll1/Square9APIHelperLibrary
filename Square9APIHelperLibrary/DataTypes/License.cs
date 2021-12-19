using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class License
    {
        public License() { }

        public string AuthServer { get; set; }
        public bool Active { get; set; }
        public DateTime DateAccessed { get; set; }
        public DateTime DateCreated { get; set; }
        public string Domain { get; set; }
        public string IPAddress { get; set; }
        public int Reg { get; set; }
        public string Token { get; set; }
        public int Type { get; set; }
        public string Username { get; set; }
    }
}
