using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Archive
    {
        public Archive() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Parent { get; set; }
        public int Permissions { get; set; }
        public int Properties { get; set; }
    }
}
