using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Notification
    {
        public Notification() { }
        public string To { get; set; } = "";
        public string From { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
