using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class SearchParameter
    {
        public SearchParameter() { }
        public int Id { get; set; }
        public int Field { get; set; }
        public string Condition { get; set; }
        public string Prompt { get; set; }
    }
}
