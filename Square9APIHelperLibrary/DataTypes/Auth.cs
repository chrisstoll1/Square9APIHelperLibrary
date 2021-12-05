using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Auth
    {
        public Auth(string username = "", string password = "") { Password = password; User = username; }
        public string Password { get; set; }
        public string User { get; set; }
    }
}
