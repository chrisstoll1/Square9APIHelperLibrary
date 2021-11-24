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
            string permissionLevel = Convert.ToString(level, 2);
            while (permissionLevel.Length < 20)
            {
                permissionLevel = $"0{permissionLevel}";
            }
            FullAPIAccess = (permissionLevel[0] == '1') ? true: false;
            DeleteBatches = (permissionLevel[1] == '1') ? true : false;
            MoveDoc = (permissionLevel[2] == '1') ? true: false;
            ModifyPages = (permissionLevel[3] == '1') ? true: false;
            PublishRevisions = (permissionLevel[4] == '1') ? true: false;
            ViewDocRevision = (permissionLevel[5] == '1') ? true: false;
            LaunchDocCopy = (permissionLevel[6] == '1') ? true: false;
            LaunchDoc = (permissionLevel[7] == '1') ? true: false;
            ModifyAnnotations = (permissionLevel[8] == '1') ? true: false;
            ModifyData = (permissionLevel[9] == '1') ? true: false;
            ViewDocHistory = (permissionLevel[10] == '1') ? true: false;
            ViewInAcrobat = (permissionLevel[11] == '1') ? true: false;
            ExportDocs = (permissionLevel[12] == '1') ? true: false;
            ExportData = (permissionLevel[13] == '1') ? true: false;
            Email = (permissionLevel[14] == '1') ? true: false;
            Print = (permissionLevel[15] == '1') ? true: false;
            Delete = (permissionLevel[16] == '1') ? true: false;
            ModifyDocument = (permissionLevel[17] == '1') ? true: false;
            Add = (permissionLevel[18] == '1') ? true: false;
            View = (permissionLevel[19] == '1') ? true: false;
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

        public void CalculatePermissionLevel()
        {
            string permissionLevel = "";
            bool[] permissions = { FullAPIAccess, DeleteBatches, MoveDoc, ModifyPages, PublishRevisions, ViewDocRevision, LaunchDocCopy, LaunchDoc, ModifyAnnotations, ModifyData, ViewDocHistory, ViewInAcrobat, ExportDocs, ExportData, Email, Print, Delete, ModifyDocument, Add, View};
            for (int i = 0; i < permissions.Length; i++)
            {
                permissionLevel = (permissions[i]) ? $"{permissionLevel}1" : $"{permissionLevel}0";
            }
            Level = Convert.ToInt32(permissionLevel, 2);
        }
        public void SelectAll()
        {
            FullAPIAccess = true;
            DeleteBatches = true;
            MoveDoc = true;
            ModifyPages = true;
            PublishRevisions = true;
            ViewDocRevision = true;
            LaunchDocCopy = true;
            LaunchDoc = true;
            ModifyAnnotations = true;
            ModifyData = true;
            ViewDocHistory = true;
            ViewInAcrobat = true;
            ExportDocs = true;
            ExportData = true;
            Email = true;
            Print = true;
            Delete = true;
            ModifyDocument = true;
            Add = true;
            View = true;
            CalculatePermissionLevel();
        }
        public void ClearAll()
        {
            FullAPIAccess = false;
            DeleteBatches = false;
            MoveDoc = false;
            ModifyPages = false;
            PublishRevisions = false;
            ViewDocRevision = false;
            LaunchDocCopy = false;
            LaunchDoc = false;
            ModifyAnnotations = false;
            ModifyData = false;
            ViewDocHistory = false;
            ViewInAcrobat = false;
            ExportDocs = false;
            ExportData = false;
            Email = false;
            Print = false;
            Delete = false;
            ModifyDocument = false;
            Add = false;
            View = false;
            CalculatePermissionLevel();
        }
    }
}
