using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class SearchDetail
    {
        public SearchDetail() { }
        public int Id { get; set; }
        public int Fid { get; set; }
        public int ListId { get; set; }
        public int Parent { get; set; }
        public int Operator { get; set; }
        public string Prompt { get; set; }
        public string Val { get; set; }
        public int Prop { get; set; }
    }
}
