using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class SecurityNode
    {
        public SecurityNode() { }
        public int Id { get; set; }
        public int Dbid { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public bool DefaultSearch { get; set; }
        public bool QueueSearch { get; set; }
        public bool DirectSearch { get; set; }
        public List<SecurityNode> Children { get; set; } = new List<SecurityNode>();
    }
}
