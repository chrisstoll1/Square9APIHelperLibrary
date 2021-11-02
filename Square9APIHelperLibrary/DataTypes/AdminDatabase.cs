using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class AdminDatabase : Database
    {
        public AdminDatabase() { }
        public DBAuth Auth { get; set; } = new DBAuth();
        public string Server { get; set; } = null;
    }
}
