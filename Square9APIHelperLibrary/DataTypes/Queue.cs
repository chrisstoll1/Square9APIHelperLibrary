using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class Queue
    {
        public Queue() { }
        public string Key { get; set; }
        public int ProcessId { get; set; }
        public string Name { get; set; }
        public string UsersAndGroups { get; set; }
        public List<Action> Actions { get; set; }
    }
}
