using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary.DataTypes
{
    /// <summary>
    /// Can be used to specify user permissions for a given archive. 
    /// This class translates the permission <see cref="Level"/> that is stored in the database into a 
    /// series of boolean values. 
    /// </summary>
    public class ArchivePermission
    {
        /// <summary>
        /// If this is the only permission granted, the user or group may view but not change data and documents. Users still need permissions to at least one Search to see documents. Since a user or group must be able to see documents to perform any other functions, View will be enabled when any other Archives permissions are selected.
        /// </summary>
        public bool View { get; set; }
        /// <summary>
        /// The user or group may put documents into the Archive, but, if this is the only choice selected, once added no changes may be made. Adding includes not just by capturing (like scan, import, import data and document, or drag-and-drop) but also with Burst, Merge, and Move.
        /// </summary>
        public bool Add { get; set; }
        /// <summary>
        /// The user or group may rotate, copy/paste, insert, append, and reorder pages of documents.
        /// </summary>
        public bool ModifyDocument { get; set; }
        /// <summary>
        /// The user or group may remove documents from the Archive.
        /// </summary>
        public bool Delete { get; set; }
        /// <summary>
        /// The user or group may use the Print functions from the Document Viewer or Search Results Document List in the Grid View.
        /// </summary>
        public bool Print { get; set; }
        /// <summary>
        /// The user or group may email documents from the Document Viewer or Search Results in the Grid View. The user must have a local email client in order to take advantage of this permission.
        /// </summary>
        public bool Email { get; set; }
        /// <summary>
        ///  Allows the user or group to export the data to a CSV file (using the Export to Excel function) from the Search Results in the Grid View.
        /// </summary>
        public bool ExportData { get; set; }
        /// <summary>
        /// The user or group may export documents in their original format from the Document Viewer or from the Search Results in the Grid View.
        /// </summary>
        public bool ExportDocs { get; set; }
        /// <summary>
        /// Allows the user or group to view documents within the native PDF file viewer. (Note that if the PDF viewer has editing capability, the user can make changes, but those changes do not carry into GlobalSearch; this situation would be similar to using Launch Copy on a PDF document.)
        /// </summary>
        public bool ViewInAcrobat { get; set; }
        /// <summary>
        /// The user or group may view the Audit Log of documents.
        /// </summary>
        public bool ViewDocHistory { get; set; }
        /// <summary>
        /// The user or group may modify indexing data for a document in any Field, so long as it it not a System Field or Read Only Field.
        /// </summary>
        public bool ModifyData { get; set; }
        /// <summary>
        /// The user or group may create, modify, and delete annotations on documents, even without other document edit permissions.
        /// </summary>
        public bool ModifyAnnotations { get; set; }
        /// <summary>
        /// Allows the user or group to use the Launch Document feature in the Document Viewer.
        /// </summary>
        public bool LaunchDoc { get; set; }
        /// <summary>
        /// Allows the user or group to use the Launch Copy feature in the Document Viewer.
        /// </summary>
        public bool LaunchDocCopy { get; set; }
        /// <summary>
        /// The user or group may view all the versions of a revision-controlled document. Users without this option will only be able to see the current version of the document. On a Versions Archive, the user or group may see previous versions of the document returned by the Search.
        /// </summary>
        public bool ViewDocRevision { get; set; }
        /// <summary>
        /// For use with a Check In/Check Out Archive in the desktop client, the user or group may set the Publish and Unpublish status for documents.
        /// </summary>
        public bool PublishRevisions { get; set; }
        /// <summary>
        /// The user or group may enhance, cut (but not paste), replace, and delete pages of documents.
        /// </summary>
        public bool ModifyPages { get; set; }
        /// <summary>
        /// The user or group may move documents from its current Archive to another.
        /// </summary>
        public bool MoveDoc { get; set; }
        /// <summary>
        /// A legacy setting which allows the user or group to remove any Batches that have errored from the Batch Manager Batch List
        /// </summary>
        public bool DeleteBatches { get; set; }
        /// <summary>
        /// This API-only security setting allows the user or group to bypass Search security, via the API, on the current Archive. It is intended for use with GlobalAction and custom integrations from 3rd party developers.  Full API Access enables the use of a specific API call that eliminates the need to run a search for a document when you already know it's Database, Document ID, and Archive.  Full API Access should be enabled for the Service Account running GlobalAction, and should be disabled in all other cases without a specific use case.
        /// </summary>
        public bool FullAPIAccess { get; set; }
        /// <summary>
        /// The Archive permission level stored in the database
        /// </summary>
        public int Level
        {
            get
            {
                string permissionLevel = "";
                bool[] permissions = { FullAPIAccess, DeleteBatches, MoveDoc, ModifyPages, PublishRevisions, ViewDocRevision, LaunchDocCopy, LaunchDoc, ModifyAnnotations, ModifyData, ViewDocHistory, ViewInAcrobat, ExportDocs, ExportData, Email, Print, Delete, ModifyDocument, Add, View };
                for (int i = 0; i < permissions.Length; i++)
                {
                    permissionLevel = (permissions[i]) ? $"{permissionLevel}1" : $"{permissionLevel}0";
                }
                return Convert.ToInt32(permissionLevel, 2);
            }
            set
            {
                string permissionLevel = Convert.ToString(value, 2);
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
        }
        public ArchivePermission(int level = 0)
        {
            Level = level;
        }
        /// <summary>
        /// Sets all permissions to True
        /// </summary>
        public void SelectAll()
        {
            Level = Convert.ToInt32("11111111111111111111", 2);
        }
        /// <summary>
        /// Sets all permissions to False
        /// </summary>
        public void ClearAll()
        {
            Level = 0;
        }
    }
}
