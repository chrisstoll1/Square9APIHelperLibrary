using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class LogEntry
    {
        public int DatabaseID { get; set; }
        public int DocumentID { get; set; }
        public int ArchiveID { get; set; }
        public int OldArchiveID {get; set; }
        public int OldDocID { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string ConnectionsString { get; set; }
        public DateTime Time { get; set; }
    }
}
