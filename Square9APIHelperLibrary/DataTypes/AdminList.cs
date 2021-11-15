using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class AdminList
    {
        public AdminList() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssemblyPath { get; set; } = "";
        public string AssemblyParameters { get; set; } = "";
        public List<string> Values { get; set; } = new List<string>();
    }
}
