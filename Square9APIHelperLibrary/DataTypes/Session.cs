using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    internal class Session
    {
        public Session() { }
        public int Archive { get; set; }
        public int Database { get; set; }
        public List<SessionDoc> Documents { get; set; } = new List<SessionDoc>();
        public int InboxId { get; set; }
        public string SearchCriteria { get; set; }
        public int SearchId { get; set; }
        public string _Id { get; set; }
        public DateTime Created { get; set; }
        public string Creator { get; set; }
        public bool Uploaded { get; set; }
    }
}
