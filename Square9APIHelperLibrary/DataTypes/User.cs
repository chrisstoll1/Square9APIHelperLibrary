using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class User
    {
        public User() { }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool Secured { get; set; }
        public string Password { get; set; }

        public void ConvertSecuredGroup(SecuredGroup group)
        {
            Name = group.Name;
            Type = group.Type;
            Secured = true;
        }
        public void ConvertUnsecuredGroup(UnsecuredGroup group)
        {
            Name = group.Name;
            Type = group.Type;
            Secured = false;
        }
    }
}
