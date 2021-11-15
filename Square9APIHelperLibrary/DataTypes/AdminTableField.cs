using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class AdminTableField
    {
        public AdminTableField() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Fields { get; set; } = new List<int>();
    }
}
