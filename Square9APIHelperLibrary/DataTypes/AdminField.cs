using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class AdminField
    {
        public AdminField() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Format { get; set; } = "";
        public string Regex { get; set; } = "";
        public int Length { get; set; }
        public bool Required { get; set; }
        public bool MultiValue { get; set; }
        public string SystemField { get; set; } = "";
        public AdminFieldList List { get; set; } = new AdminFieldList();
    }
}
