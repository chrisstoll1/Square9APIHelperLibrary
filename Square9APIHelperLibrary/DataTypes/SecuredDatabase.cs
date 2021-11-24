using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class SecuredDatabase : Database
    {
        public SecuredDatabase() { }
        public int SecurityLevel { get; set; }
        public string Manager { get; set; }
    }
}
