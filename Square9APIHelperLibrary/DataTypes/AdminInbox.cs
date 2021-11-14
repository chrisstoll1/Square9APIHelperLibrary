using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class AdminInbox
    {
        public AdminInbox() { }
        public BasePath BasePath { get; set; } = new BasePath();
        public int Id { get; set; }
        public string Name { get; set; }
        public bool UniqueName { get; set; } = false;
    }
}
