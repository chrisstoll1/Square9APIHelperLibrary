using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Doc
    {
        public Doc() { }
        public List<FieldItem> Fields { get; set; }
        public string FileType { get; set; }
        public string Hash { get; set; }
        public int Hits { get; set; }
        public int Id { get; set; }
        public int Permissions { get; set; }
        public int RevisionOptions { get; set; }
        public bool Revisions { get; set; }
        public int RootVersionId { get; set; }
        public int Tid { get; set; }
        public bool User_Name { get; set; }
        public int Version { get; set; }
        public int VersionArchive { get; set; }

    }
}
