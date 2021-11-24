using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class DatabasePermission
    {
        public DatabasePermission() { }
        public bool Remove { get; set; }
        public int Type { get; set; }
        public string Manager { get; set; }
        public int License { get; set; }
    }
}
