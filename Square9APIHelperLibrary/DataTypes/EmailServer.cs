using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class EmailServer
    {
        public EmailServer() { }
        public Auth Auth { get; set; }
        public int Port { get; set; }
        public string Server { get; set; }
    }
}
