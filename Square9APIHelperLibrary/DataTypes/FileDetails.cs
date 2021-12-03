using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class FileDetails
    {
        public FileDetails() { }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public bool IsEmail { get; set; }
        public string OEmailData { get; set; }
    }
}
