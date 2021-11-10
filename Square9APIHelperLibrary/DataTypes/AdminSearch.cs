using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class AdminSearch
    {
        public AdminSearch() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Parent { get; set; }
        public string AdvancedGrouping { get; set; } = "";
        public bool GrantAll { get; set; }
        public bool ImageXChange { get; set; }
        public bool MultiValue { get; set; }
        public bool DisplayViewTabs { get; set; }
        public bool UseTime { get; set; }
        public List<int> Archives { get; set; } = new List<int>();
        public ContentSearch ContentSearch { get; set; } = new ContentSearch();
        public List<SearchParameter> Parameters { get; set; } = new List<SearchParameter>();
    }
}
