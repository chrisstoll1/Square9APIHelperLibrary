using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Security
    {
        public Security() { }
        public int ArchiveId { get; set; }
        public int Permissions { get; set; }
        public int Type { get; set; }
        public string User { get; set; }
    }
}
