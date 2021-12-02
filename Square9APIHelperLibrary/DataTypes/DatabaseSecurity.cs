using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class DatabaseSecurity
    {
        public DatabaseSecurity() { }
        public List<User> Users { get; set; } = new List<User>();
        public List<Target> Targets { get; set; } = new List<Target>();
        public DatabasePermission Permissions { get; set; } = new DatabasePermission();
    }
}
