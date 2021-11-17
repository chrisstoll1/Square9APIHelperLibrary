using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class DocumentIndexData
    {
        public DocumentIndexData() { }
        public DocumentIndexFields IndexData { get; set; } = new DocumentIndexFields();
    }
}
