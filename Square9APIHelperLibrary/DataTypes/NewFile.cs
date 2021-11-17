using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class NewFile
    {
        public NewFile() { }
        public List<FileField> Fields { get; set; } = new List<FileField>();
        public List<FileDetails> Files { get; set; } = new List<FileDetails>();
    }
}
