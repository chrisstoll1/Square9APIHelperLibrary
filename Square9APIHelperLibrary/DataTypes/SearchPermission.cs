using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class SearchPermission
    {
        public SearchPermission() { }
        public bool View { get; set; }
        public int Type { get; set; }
        public bool Remove { get; set; }
    }
}
