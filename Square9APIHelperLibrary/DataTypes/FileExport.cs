using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class FileExport
    {
        public FileExport(int archiveId, Doc document) 
        {
            Archive = archiveId;
            Id = document.Id;
            Hash = document.Hash;
        }
        public int Archive { get; set; }
        public int Id { get; set; }
        public string Hash { get; set; }
    }
}
