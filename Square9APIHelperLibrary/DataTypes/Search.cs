using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Search
    {
        public Search() { }
        public int Id { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public int Props { get; set; }
        public int Fuzzy { get; set; }
        public int Settings { get; set; }
        public string Grouping { get; set; }
        public List<Archive> Archives { get; set; }
        public List<SearchDetail> Detail { get; set; }
    }
}
