using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class AdminFieldList
    {
        public AdminFieldList() { }
        public int ListId { get; set; }
        public List<AdminMapping> Mapping { get; set; } = new List<AdminMapping>();
        public int Primary { get; set; }
        public int Secondary { get; set; }
        public string Type { get; set; }
    }
}
