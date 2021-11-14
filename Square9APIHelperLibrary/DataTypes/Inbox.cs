using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Inbox
    {
        public Inbox() { }
        public int Count { get; set; }
        public List<File> Files { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Permissions { get; set; }
    }
}
