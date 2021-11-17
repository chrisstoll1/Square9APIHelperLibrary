using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class DocumentIndexFields
    {
        public DocumentIndexFields() { }
        public List<FieldItem> IndexFields { get; set; } = new List<FieldItem>();
    }
}
