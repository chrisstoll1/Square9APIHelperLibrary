using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// Live Fields contain custom javascript and are used to perform custom actions from within the document viewer. 
    /// </summary>
    public class LiveField
    {
        public LiveField() { }

        public string Method { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string JsonPath { get; set; } = string.Empty;
        public string Body { get; set; }
        public string Script { get; set; } = string.Empty;
    }
}
