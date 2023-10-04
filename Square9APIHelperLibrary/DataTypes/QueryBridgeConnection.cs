using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class QueryBridgeConnection
    {
        public QueryBridgeConnection(string sqlUsername, string sqlPassword, string sqlServer, string sqlDatabase)
        {
            Username = sqlUsername;
            Password = sqlPassword;
            Server = sqlServer;
            Database = sqlDatabase;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
    }
}
