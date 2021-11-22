using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Revision
    {
        public Revision() { }
        public int DocId { get; set; }
        public string SecureId { get; set; }
        public int Version { get; set; }

        public Doc ReturnDoc()
        {
            Doc doc = new Doc();
            doc.Id = DocId;
            doc.Hash = SecureId;
            doc.Version = Version;
            return doc;
        }
    }
}
