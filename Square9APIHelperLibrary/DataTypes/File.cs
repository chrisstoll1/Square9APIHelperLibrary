using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class File
    {
        public File() { }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public List<FieldItem> Fields { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public bool NewName { get; set; }
        public int Permissions { get; set; }

    }
}
