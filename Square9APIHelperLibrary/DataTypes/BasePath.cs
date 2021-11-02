using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class BasePath
    {
        public BasePath() { }
        public bool Default { get; set; } = true;
        public string Path { get; set; } = "";
        public bool UniqueName { get; set; }
    }
}
