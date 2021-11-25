using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    public class ArchivePermission
    {
        public ArchivePermission(int level = 0) 
        {
            UpdateBools(level);
            CalculatePermissionLevel();
        }
        public int Level { get; set; }
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool ModifyDocument { get; set; }
        public bool Delete { get; set; }
        public bool Print { get; set; }
        public bool Email { get; set; }
        public bool ExportData { get; set; }
        public bool ExportDocs { get; set; }
        public bool ViewInAcrobat { get; set; }
        public bool ViewDocHistory { get; set; }
        public bool ModifyData { get; set; }
        public bool ModifyAnnotations { get; set; }
        public bool LaunchDoc { get; set; }
        public bool LaunchDocCopy { get; set; }
        public bool ViewDocRevision { get; set; }
        public bool PublishRevisions { get; set; }
        public bool ModifyPages { get; set; }
        public bool MoveDoc { get; set; }
        public bool DeleteBatches { get; set; }
        public bool FullAPIAccess { get; set; }

        public void CalculatePermissionLevel() //Calculates Level based on Bools
        {
            string permissionLevel = "";
            bool[] permissions = { FullAPIAccess, DeleteBatches, MoveDoc, ModifyPages, PublishRevisions, ViewDocRevision, LaunchDocCopy, LaunchDoc, ModifyAnnotations, ModifyData, ViewDocHistory, ViewInAcrobat, ExportDocs, ExportData, Email, Print, Delete, ModifyDocument, Add, View};
            for (int i = 0; i < permissions.Length; i++)
            {
                permissionLevel = (permissions[i]) ? $"{permissionLevel}1" : $"{permissionLevel}0";
            }
            Level = Convert.ToInt32(permissionLevel, 2);
        }
        private void UpdateBools(int level) //Sets bools bases on passed level
        {
            string permissionLevel = Convert.ToString(level, 2);
            while (permissionLevel.Length < 20)
            {
                permissionLevel = $"0{permissionLevel}";
            }
            FullAPIAccess = (permissionLevel[0] == '1') ? true : false;
            DeleteBatches = (permissionLevel[1] == '1') ? true : false;
            MoveDoc = (permissionLevel[2] == '1') ? true : false;
            ModifyPages = (permissionLevel[3] == '1') ? true : false;
            PublishRevisions = (permissionLevel[4] == '1') ? true : false;
            ViewDocRevision = (permissionLevel[5] == '1') ? true : false;
            LaunchDocCopy = (permissionLevel[6] == '1') ? true : false;
            LaunchDoc = (permissionLevel[7] == '1') ? true : false;
            ModifyAnnotations = (permissionLevel[8] == '1') ? true : false;
            ModifyData = (permissionLevel[9] == '1') ? true : false;
            ViewDocHistory = (permissionLevel[10] == '1') ? true : false;
            ViewInAcrobat = (permissionLevel[11] == '1') ? true : false;
            ExportDocs = (permissionLevel[12] == '1') ? true : false;
            ExportData = (permissionLevel[13] == '1') ? true : false;
            Email = (permissionLevel[14] == '1') ? true : false;
            Print = (permissionLevel[15] == '1') ? true : false;
            Delete = (permissionLevel[16] == '1') ? true : false;
            ModifyDocument = (permissionLevel[17] == '1') ? true : false;
            Add = (permissionLevel[18] == '1') ? true : false;
            View = (permissionLevel[19] == '1') ? true : false;
        }
        public void SelectAll()
        {
            UpdateBools(Convert.ToInt32("11111111111111111111", 2));
            CalculatePermissionLevel();
        }
        public void ClearAll()
        {
            UpdateBools(0);
            CalculatePermissionLevel();
        }
    }
}
