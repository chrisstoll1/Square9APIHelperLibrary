using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// QueryBridge allows for SQL queries to be evaluated in the database environment through the Square9API. 
    /// This is an advanced feature and requires proper SQL authentication to be configured on the server before hand
    /// </summary>
    public class QueryBridgeRequest
    {
        /// <summary>
        /// This constructor will build the QueryBridgeRequest Object
        /// </summary>
        /// <param name="server">Full SQL Server Name</param>
        /// <param name="sqluser">SQL User Account to process the request</param>
        /// <param name="sqlpassword">Password for SQL user account</param>
        /// <param name="sqldatabase">Database Name to process request on</param>
        /// <param name="query">SQL Query to be evaluated</param>
        public QueryBridgeRequest(string server, string sqluser, string sqlpassword, string sqldatabase, string query = "")
        {
            Connection = new QueryBridgeConnection(sqluser, sqlpassword, server, sqldatabase);
            Query = query;
        }
        public string Query { get; set; }
        public QueryBridgeConnection Connection { get; set; }
    }
}
