using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class TableField
    {
        public List<List<string>> Data { get; set; } = new List<List<string>>();
        public List<int> Fields { get; set; }
        public int Id { get; set; }
        public AdminTableField Properties { get; set; }

        public TableField(AdminTableField field)
        {
            Fields = field.Fields;
            Id = field.Id;
            Properties = field;
        }
    }
}
