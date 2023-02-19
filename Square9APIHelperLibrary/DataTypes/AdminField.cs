using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// AdminFields are used to define the fields that are available for use in the Square9 Global Search application. 
    /// The Admin Field is used with the Administration API to create, update, and delete fields.
    /// </summary>
    public class AdminField
    {
        public AdminField() { }
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// A string that determines the type of data that will be entered into the field.
        /// Valid values are: "Character", "Date", "Decimal", "Numeric"
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The format that will be used to display the data in the document viewer.
        /// </summary>
        public string Format { get; set; } = "";
        /// <summary>
        /// The regular expression that will be used to validate the data entered into the field.
        /// </summary>
        public string Regex { get; set; } = "";
        public int Length { get; set; }
        public bool Required { get; set; }
        public bool MultiValue { get; set; }
        public string SystemField { get; set; } = "";
        /// <summary>
        /// Custom validation message that will appear when the entered data does not match the regular expression in the document viewer
        /// </summary>
        public string RegexMessage { get; set; } = "";
        /// <summary>
        /// RegSample should only be included on POST or PUT requests. It is not returned on GET requests.
        /// Can contain the name of one of the predefined regular expressions or a custom regular expression
        /// </summary>
        public RegSample RegSample { get; set; }
        public AdminFieldList List { get; set; } = new AdminFieldList();
        /// <summary>
        /// Extended configuration options for the field, includes new <see cref="LiveField"/>
        /// </summary>
        public ExtendedConfig ExtendedConfig { get; set; } = new ExtendedConfig();
    }
}
