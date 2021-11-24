using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class SearchSecurity
    {
        public SearchSecurity() { }
        public int Id { get; set; }
        public int Properties { get; set; }
        public bool IsDefault { get; set; }
        public bool IsQueue { get; set; }
        public bool IsDirect { get; set; }
    }
}
