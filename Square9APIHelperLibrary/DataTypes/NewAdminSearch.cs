using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class NewAdminSearch : AdminSearch
    {
        public NewAdminSearch(string name) 
        {
            Name = name;        
        }

        public void AddParameter(int fieldId, string condition, string prompt)
        {
            SearchParameter newParameter = new SearchParameter();
            newParameter.Field = fieldId;
            newParameter.Condition = condition;
            newParameter.Prompt = prompt;
            Parameters.Add(newParameter);
        }
    }
}
