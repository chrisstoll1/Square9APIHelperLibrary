using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class AdminArchive : Archive
    {
        public AdminArchive() { }
        public BasePath BasePath { get; set; }
        public bool CreateBrowse { get; set; }
        public List<int> Fields { get; set; }
        public Views View { get; set; } = new Views();
        public bool ContentSearch { get; set; }
        public int RevisionControl { get; set; }
        public bool ConvertPdf { get; set; }
        public Notification Notifications { get; set; } = new Notification();
    }
}
