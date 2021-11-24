using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class ArchiveSecurity
    {
        public ArchiveSecurity() { }
        public List<User> Users { get; set; } = new List<User>();
        public List<Target> Targets { get; set; } = new List<Target>();
        public ArchivePermission Permissions { get; set; }
    }
}
