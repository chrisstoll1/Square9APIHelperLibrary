using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Database
    {
        public Database() { }

        /// <summary>
        /// The ID of a given Database
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The String name of a given Database
        /// </summary>
        public string Name { get; set; }
    }
}
