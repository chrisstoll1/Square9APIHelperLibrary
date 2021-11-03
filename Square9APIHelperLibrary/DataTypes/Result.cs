using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Result
    {
        public Result() { }
        public int Count { get; set; }
        public List<Doc> Docs { get; set; }
        public List<Field> Fields { get; set; }
        public string FolderOptions { get; set; }
        public bool PdfConvertStatus { get; set; }
        public bool Terms { get; set; }
        public int WorkflowStatus { get; set; }

    }
}
