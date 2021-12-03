using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class DocDetails
    {
        DocDetails() { }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string Id { get; set; }
        public bool IsNative { get; set; }
    }
}
