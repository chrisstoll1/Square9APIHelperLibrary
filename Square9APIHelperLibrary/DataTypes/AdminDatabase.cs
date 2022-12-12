using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// Wrapper class that contains additional database properties, specific to the admin api
    /// </summary>
    public class AdminDatabase : Database
    {
        public AdminDatabase() { }
        public DBAuth Auth { get; set; } = new DBAuth();
        /// <summary>
        /// Local SQL Server instance that the database belongs to
        /// </summary>
        public string Server { get; set; } = null;
    }
}
