using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// Extended Config is a class that contains the LiveField and UseRegExValidation properties.  This class is used to extend the functionality of the Field class.
    /// Added in version 6.3 of GlobalSearch
    /// </summary>
    public class ExtendedConfig
    {
        public ExtendedConfig() { }

        public LiveField LiveField { get; set; } = new LiveField();
        public bool UseRegExValidation { get; set; } = false;
    }
}
