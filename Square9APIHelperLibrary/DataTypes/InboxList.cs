using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class InboxList
    {
        public InboxList() { }
        public int InboxOptions { get; set; }
        public List<Inbox> Inboxes { get; set; }
    }
}
