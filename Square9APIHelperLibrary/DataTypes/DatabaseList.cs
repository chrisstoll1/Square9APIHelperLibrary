using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// Wrapper Class that contains a list of <see cref="Database"/>
    /// </summary>
    public class DatabaseList
    {
        public DatabaseList() { }
        public List<Database> Databases { get; set; }
    }
}
