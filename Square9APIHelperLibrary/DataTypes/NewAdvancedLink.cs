using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// Advanced Links can be attatched to a field and are used to create a link to external resources.
    /// The New Advanced Link class is used to create a new Advanced Link.
    /// It takes the following parameters: Name, Field, URL, and Archive.
    /// Archive is optional and defaults to 0. If its value is 0, the link will apply under all archives.
    /// </summary>
    public class NewAdvancedLink : AdvancedLink
    {
        public NewAdvancedLink(string name, int field, string url, int archive = 0) 
        { 
            this.Name = name;
            this.FieldId = field;
            this.UrlPattern = url;
            this.ArchiveId = archive;
        }
    }
}
