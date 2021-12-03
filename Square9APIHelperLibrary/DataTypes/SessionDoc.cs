using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    internal class SessionDoc
    {
        public SessionDoc() { }
        public int Archive { get; set; }
        public string InboxFileName { get; set; }
        public int InboxId { get; set; }
        public string ArchiveName { get; set; }
        public string ContentSearch { get; set; }
        public string FieldData { get; set; }
        public string Fields { get; set; }
        public string Hash { get; set; }
        public int Id { get; set; }
        public string OriginalFileName { get; set; }
    }
}
