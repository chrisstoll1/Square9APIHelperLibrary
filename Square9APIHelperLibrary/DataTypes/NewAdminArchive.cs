using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class NewAdminArchive : AdminArchive
    {
        public NewAdminArchive(string name)
        {
            Name = name;
            BasePath = new BasePath();
            CreateBrowse = true;
        }
    }
}
