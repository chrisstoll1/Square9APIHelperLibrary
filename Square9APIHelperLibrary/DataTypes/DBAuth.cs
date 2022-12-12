using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// Database authentication settings
    /// </summary>
    public class DBAuth
    {
        public DBAuth() { }
        public string Type { get; set; } = "windows";
        public string User { get; set; }
        public string Pass { get; set; }
    }
}
