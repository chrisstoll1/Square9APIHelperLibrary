using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class UnsecuredGroup
    {
        public UnsecuredGroup() { }
        public string Name { get; set; }
        public string Email { get; set; }
        public int License { get; set; }
        public int Type { get; set; }
        public List<SecuredDatabase> SecuredDBs { get; set; } = new List<SecuredDatabase>();
    }
}
