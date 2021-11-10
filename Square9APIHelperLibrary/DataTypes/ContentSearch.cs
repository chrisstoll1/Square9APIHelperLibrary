using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class ContentSearch
    {
        public ContentSearch() { }
        public bool Enabled { get; set; }
        public bool Phonics { get; set; }
        public bool Stemming { get; set; }
        public bool Fuzziness { get; set; }
        public int FuzzinessValue { get; set; }
    }
}
