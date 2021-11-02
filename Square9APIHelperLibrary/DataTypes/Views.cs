using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Views
    {
        public Views() { }
        public bool Enabled { get; set; }
        public int StatusField { get; set; }
        public List<ViewValue> Values { get; set; }
    }
}
