using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// Advanced Links can be attatched to a field and are used to create a link to external resources.
    /// </summary>
    public class AdvancedLink
    {
        public AdvancedLink() { }
        public int ArchiveId { get; set; }
        public int FieldId { get; set; }
        public Nullable<int> Id { get; set; }
        public string Name { get; set; }
        public string UrlPattern { get; set; }
    }
}
