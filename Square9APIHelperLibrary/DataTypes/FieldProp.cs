using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class FieldProp
    {
        public FieldProp(int level = 0)
        {
            UpdateBools(level);
            CalculatePermissionLevel();
        }
        public int Level { get; set; }
        public bool Required { get; set; }
        public bool SystemFieldDateEntered { get; set; }
        public bool SystemFieldIndexedBy { get; set; }
        public bool SystemFieldPageCount { get; set; }
        public bool MultiValueField { get; set; }
        public bool DropdownList { get; set; }
        public bool DynamicList { get; set; }
        public bool SystemFieldLastModifedBy { get; set; }
        public bool TableField { get; set; }
        public bool SystemFieldFileType { get; set; }
        public bool SystemFieldReadOnly { get; set; }

        public void CalculatePermissionLevel() //Calculates Level based on Bools
        {
            string permissionLevel = "";
            bool[] permissions = { SystemFieldReadOnly, SystemFieldFileType, TableField, SystemFieldLastModifedBy, DynamicList, DropdownList, MultiValueField, SystemFieldPageCount, SystemFieldIndexedBy, SystemFieldDateEntered, Required, false };
            for (int i = 0; i < permissions.Length; i++)
            {
                permissionLevel = (permissions[i]) ? $"{permissionLevel}1" : $"{permissionLevel}0";
            }
            Level = Convert.ToInt32(permissionLevel, 2);
        }
        private void UpdateBools(int level) //Sets bools bases on passed level
        {
            string permissionLevel = Convert.ToString(level, 2);
            while (permissionLevel.Length < 12)
            {
                permissionLevel = $"0{permissionLevel}";
            }
            SystemFieldReadOnly = (permissionLevel[0] == '1') ? true : false;
            SystemFieldFileType = (permissionLevel[1] == '1') ? true : false;
            TableField = (permissionLevel[2] == '1') ? true : false;
            SystemFieldLastModifedBy = (permissionLevel[3] == '1') ? true : false;
            DynamicList = (permissionLevel[4] == '1') ? true : false;
            DropdownList = (permissionLevel[5] == '1') ? true : false;
            MultiValueField = (permissionLevel[6] == '1') ? true : false;
            SystemFieldPageCount = (permissionLevel[7] == '1') ? true : false;
            SystemFieldIndexedBy = (permissionLevel[8] == '1') ? true : false;
            SystemFieldDateEntered = (permissionLevel[9] == '1') ? true : false;
            Required = (permissionLevel[10] == '1') ? true : false;
        }
        public void SelectAll()
        {
            UpdateBools(Convert.ToInt32("111111111111", 2));
            CalculatePermissionLevel();
        }
        public void ClearAll()
        {
            UpdateBools(0);
            CalculatePermissionLevel();
        }
    }
}
