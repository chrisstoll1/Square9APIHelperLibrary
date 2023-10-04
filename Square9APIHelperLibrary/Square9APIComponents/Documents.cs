using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Square9APIHelperLibrary.DataTypes;

namespace Square9APIHelperLibrary.Square9APIComponents
{
    /// <summary>
    /// Documents are image files stored within either archives or inboxes. They contain index information that can be used to retrieve the document via <see cref="Searches"/>
    /// </summary>
    public class Documents
    {
        private RestClient ApiClient;
        private string Default;
        private Fields Fields;
        private License License;
        internal Documents(RestClient apiClient, string defaultSql, License license, Fields fields)
        {
            ApiClient = apiClient;
            Default = defaultSql;
            License = license;
            Fields = fields;
        }

        #region Methods
        /// <summary>
        /// Requests an individual document from the server based on ID
        /// </summary>
        /// <param name="databaseId">DatabaseID</param>
        /// <param name="archiveId">ArchiveID</param>
        /// <param name="documentId">Document ID</param>
        /// <exception cref="Exception"></exception>
        public Result GetArchiveDocument(int databaseId, int archiveId, int documentId)
        {
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}?DocumentID={documentId}&Token={License.Token}");
            var SecureIDResponse = ApiClient.Execute(Request);
            if (SecureIDResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get document Secure ID: {SecureIDResponse.Content}");
            }
            string SecureID = SecureIDResponse.Content;
            var DocRequest = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{documentId}?SecureId={SecureID}");
            var Response = ApiClient.Execute<Result>(DocRequest);
            if (SecureIDResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get document: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests all meta data for a given document
        /// </summary>
        /// <param name="databaseId">DatabaseID</param>
        /// <param name="archiveId">ArchiveID</param>
        /// <param name="document"><see cref="Doc"/></param>
        /// <returns>Returns the same <see cref="Result"/> that <see cref="GetSearchResults(int, Search, int, int, int, int, int)"/> would return</returns>
        /// <exception cref="Exception"></exception>
        public Result GetArchiveDocumentMetaData(int databaseId, int archiveId, Doc document)
        {
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}?SecureId={document.Hash}");
            var Response = ApiClient.Execute<Result>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get document Meta Data: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests a files details from the server for a specified inbox/file
        /// </summary>
        /// <param name="inboxId">The ID of the inbox the file resides in</param>
        /// <param name="fileName">The original filename of the file to return details on</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DocDetails GetInboxDocumentDetails(int inboxId, File file)
        {
            var Request = new RestRequest($"api/inboxes?FilePath={file.FileName}{file.FileType}&Id={inboxId}");
            var Response = ApiClient.Execute<DocDetails>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get document Details: {Response.StatusDescription}");
            }
            return JsonConvert.DeserializeObject<DocDetails>(Response.Content);
        }
        /// <summary>
        /// Downloads a given documents file to either a temp path or specific local path
        /// </summary>
        /// <param name="databaseId">DatabaseID</param>
        /// <param name="archiveId">ArchiveID</param>
        /// <param name="document"><see cref="Doc"/> to be downloaded from the server</param>
        /// <param name="savePath">Optional: Local path to save the documents file to</param>
        /// <returns>A <see cref="string"/> of the downloaded files filepath</returns>
        /// <exception cref="Exception"></exception>
        public string GetArchiveDocumentFile(int databaseId, int archiveId, Doc document, string savePath = "")
        {
            var fileName = (savePath == "") ? $"{System.IO.Path.GetTempPath()}{Guid.NewGuid().ToString()}{document.FileType}" : $"{savePath}{document.Id}{document.FileType}";
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/File?SecureId={document.Hash}&token={License.Token}");

            using (var writer = System.IO.File.OpenWrite(fileName))
            {
                Request.ResponseWriter = responseStream =>
                {
                    using (responseStream)
                    {
                        responseStream.CopyTo(writer);
                        writer.Flush();
                    }
                };

                var Response = ApiClient.Execute(Request);

                if (Response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Unable to download file. HTTP Status: {Response.StatusCode}. Content: {Response.StatusDescription}");
                }
            }

            return fileName;
        }
        /// <summary>
        /// Requests a documents file from a inbox on the server
        /// </summary>
        /// <param name="inboxId">The target inboxes ID</param>
        /// <param name="file"><see cref="File"/></param>
        /// <param name="savePath">Optional: Local path to save file</param>
        /// <returns>A <see cref="string"/> containing the local files path</returns>
        /// <exception cref="Exception"></exception>
        public string GetInboxDocumentFile(int inboxId, File file, string savePath = "")
        {
            var fileName = (savePath == "") ? $"{System.IO.Path.GetTempPath()}{Guid.NewGuid().ToString()}{file.FileType}" : $"{savePath}{file.FileName}{file.FileType}";
            var writer = System.IO.File.OpenWrite(fileName);
            var Request = new RestRequest($"api/inboxes/{inboxId}?FileName={file.FileName}{file.FileType}");
            Request.ResponseWriter = responseStream =>
            {
                using (responseStream)
                {
                    responseStream.CopyTo(writer);
                }
            };
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to download file: {Response.StatusDescription}");
            }
            return fileName;
        }
        /// <summary>
        /// Downloads a thumbnail of the document in JPG format
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/></param>
        /// <param name="document"><see cref="Doc"/></param>
        /// <param name="savePath">The local filepath where you would like to save the thumbnail</param>
        /// <param name="height"><see cref="int"/> height in pixels the thumbnail should be downloaded in</param>
        /// <param name="width"><see cref="int"/> width in pixels the thumbnail should be downloaded in</param>
        /// <returns>a <see cref="string"/> containing the local file path of the downloaded thumbnail</returns>
        /// <exception cref="Exception"></exception>
        public string GetArchiveDocumentThumbnail(int databaseId, int archiveId, Doc document, string savePath = "", int height = 0, int width = 0)
        {
            var fileName = (savePath == "") ? $"{System.IO.Path.GetTempPath()}{Guid.NewGuid().ToString()}.jpg" : $"{savePath}{document.Id}.jpg";
            var writer = System.IO.File.OpenWrite(fileName);
            var Request = (height == 0 || width == 0) ? new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/thumb?SecureId={document.Hash}") : new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/thumb?SecureId={document.Hash}&height={height}&width={width}");
            Request.ResponseWriter = responseStream =>
            {
                using (responseStream)
                {
                    responseStream.CopyTo(writer);
                }
            };
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to download thumbnail: {Response.StatusDescription}");
            }
            return fileName;
        }
        /// <summary>
        /// Updates a documents index data on the server
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/></param>
        /// <param name="document">Document to update on the server</param>
        /// <returns><see cref="Doc"/> *Document object returned is the same one passed to the method, the server returns nothing*</returns>
        /// <exception cref="Exception"></exception>
        public Doc UpdateDocumentIndexData(int databaseId, int archiveId, Doc document)
        {
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/save?SecureId={document.Hash}", Method.POST);
            DocumentIndexData IndexData = new DocumentIndexData();
            IndexData.IndexData.IndexFields = document.Fields;
            Request.AddJsonBody(IndexData);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update document index data: {Response.StatusDescription}");
            }
            return document;
        }
        /// <summary>
        /// Uploads a file to the server
        /// </summary>
        /// <param name="fileName">The full local path of the file to be uploaded</param>
        /// <returns><see cref="UploadedFiles"/></returns>
        /// <exception cref="Exception"></exception>
        public UploadedFiles UploadDocument(string fileName)
        {
            var Request = new RestRequest($"api/files", Method.POST);
            Request.AddFile("File", fileName);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to upload file: {Response.StatusDescription}");
            }
            return JsonConvert.DeserializeObject<UploadedFiles>(Response.Content);
        }
        /// <summary>
        /// Imports a uploaded document into a archive
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/></param>
        /// <param name="newFile"><see cref="NewFile"/></param>
        /// <returns>The document Id of the indexed document</returns>
        public List<int> ImportArchiveDocument(int databaseId, int archiveId, NewFile newFile, bool useViewerCache = false)
        {
            var Request = new RestRequest((!useViewerCache) ? $"api/dbs/{databaseId}/archives/{archiveId}" : $"api/dbs/{databaseId}/archives/{archiveId}?useViewerCache=true", Method.POST);
            Request.AddJsonBody(newFile);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to index document: {Response.StatusDescription}");
            }
            return JsonConvert.DeserializeObject<List<int>>(Response.Content);
        }
        /// <summary>
        /// Imports a new document into a inbox
        /// </summary>
        /// <param name="inboxId">ID of the inbox to import the file to</param>
        /// <param name="file"><see cref="FileDetails"/> Returned by <see cref="UploadDocument(string)"/></param>
        /// <exception cref="Exception"></exception>
        public void ImportInboxDocument(int inboxId, FileDetails file)
        {
            var Request = new RestRequest($"api/inboxes/{inboxId}?FilePath={file.Name}&newFileName={file.OriginalName}", Method.POST);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to import document: {Response.StatusDescription}");
            }
        }
        /// <summary>
        /// Indexes a document from a inbox to a archive
        /// </summary>
        /// <param name="databaseId">Database ID of the destination database</param>
        /// <param name="archiveId">Archive ID of the desitination archive</param>
        /// <param name="inboxId">Inbox ID of the source inbox</param>
        /// <param name="file"><see cref="File"/></param>
        /// <param name="fields">Optional: <see cref="FileField"/></param>
        public void IndexInboxDocument(int databaseId, int archiveId, int inboxId, File file, List<FileField> fields = null)
        {
            DocDetails details = GetInboxDocumentDetails(inboxId, file);

            NewFile newFile = new NewFile();
            if (fields != null) { newFile.Fields = fields; }
            FileDetails newFileDetails = new FileDetails();
            newFileDetails.Name = details.FileName;
            newFile.Files.Add(newFileDetails);

            ImportArchiveDocument(databaseId, archiveId, newFile, true);
        }
        /// <summary>
        /// Downloads a zip file containing the specified documents
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="fieldId"><see cref="Doc.Fields"/></param>
        /// <param name="files"><see cref="FileExport"/></param>
        /// <param name="savePath">Optional: Path to save Zip file to</param>
        /// <returns><see cref="string"/> containing the local path of the downloaded zip file</returns>
        /// <exception cref="Exception"></exception>
        public string ExportDocument(int databaseId, int fieldId, List<FileExport> files, string savePath = "")
        {
            var CreateExportJobRequest = new RestRequest($"api/dbs/{databaseId}/export?alwaysExportZip=true&auditEntry=Document+Exported&field={fieldId}", Method.POST);
            CreateExportJobRequest.AddJsonBody(files);
            var CreateExportJobResponse = ApiClient.Execute(CreateExportJobRequest);
            if (CreateExportJobResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create export job: {CreateExportJobResponse.Content}");
            }
            var fileName = (savePath == "") ? $"{System.IO.Path.GetTempPath()}{Guid.NewGuid().ToString()}.zip" : $"{savePath}{CreateExportJobResponse.Content.Replace("\"", "")}.zip";
            var writer = System.IO.File.OpenWrite(fileName);
            Console.WriteLine($"api/dbs/{databaseId}/export?jobid={CreateExportJobResponse.Content.Replace("\"", "")}");
            var GetJobZipRequest = new RestRequest($"api/dbs/{databaseId}/export?jobid={CreateExportJobResponse.Content.Replace("\"", "")}");
            GetJobZipRequest.ResponseWriter = responseStream =>
            {
                using (responseStream)
                {
                    responseStream.CopyTo(writer);
                }
            };
            var GetJobZipResponse = ApiClient.Execute(GetJobZipRequest);
            if (GetJobZipResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to download export zip: {GetJobZipResponse.Content}");
            }
            return fileName;
        }
        /// <summary>
        /// Deletes a document from the server
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/></param>
        /// <param name="document"><see cref="Doc"/></param>
        /// <exception cref="Exception"></exception>
        public void DeleteArchiveDocument(int databaseId, int archiveId, Doc document)
        {
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/delete?SecureId={document.Hash}");
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable delete document: {Response.StatusDescription}");
            }
        }
        /// <summary>
        /// Deletes the specified document from a inbox
        /// </summary>
        /// <param name="inboxId">The ID of the inbox you would like to delete from</param>
        /// <param name="file"><see cref="File"/> to be deleted</param>
        /// <exception cref="Exception"></exception>
        public void DeleteInboxDocument(int inboxId, File file)
        {
            var Request = new RestRequest($"api/inboxes/{inboxId}?FilePath={file.FileName}{file.FileType}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable delete document: {Response.StatusDescription}");
            }
        }
        /// <summary>
        /// Performs either a move or copy operation to a given document on the server
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/> The source archive ID</param>
        /// <param name="destArchiveId"><see cref="Archive.Id"/> The destination archive ID</param>
        /// <param name="document"><see cref="Doc"/></param>
        /// <param name="move"><see cref="bool"/> Flag to perform move operation (Cut/Paste)</param>
        /// <returns><see cref="int"/> Doc ID of the moved/copied document</returns>
        /// <exception cref="Exception"></exception>
        public int TransferArchiveDocument(int databaseId, int archiveId, int destArchiveId, Doc document, bool move = false)
        {
            string type = (move) ? "move" : "copy";
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/{type}?DestinationArchive={destArchiveId}&SecureID={document.Hash}");
            var Response = ApiClient.Execute<int>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to transfer document: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Performs either a move or copy operation to a given document on the server
        /// </summary>
        /// <param name="inboxId">Inbox ID</param>
        /// <param name="destInboxId">Destination inbox ID</param>
        /// <param name="file"><see cref="File"/> to by transfered</param>
        /// <param name="move">Optional: When set to true, operation will delete the source file in the source inbox after copy</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string TransferInboxDocument(int inboxId, int destInboxId, File file, bool move = false)
        {
            var Request = new RestRequest($"api/inboxes/{inboxId}?FilePath={file.FileName}{file.FileType}&deleteOriginal={move}&targetInboxId={destInboxId}");
            var Response = ApiClient.Execute<string>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to transfer document: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Returns a list of document revisions from the server
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/></param>
        /// <param name="documentId"><see cref="Doc.Id"/></param>
        /// <returns><see cref="Revision"/></returns>
        /// <exception cref="Exception"></exception>
        public List<Revision> GetDocumentRevisions(int databaseId, int archiveId, Doc document)
        {
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/rev?SecureId={document.Hash}");
            var Response = ApiClient.Execute<List<Revision>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get document revisions: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Returns a queue object that the document is in 
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/></param>
        /// <param name="document"><see cref="Doc"/></param>
        /// <returns><see cref="Queue"/></returns>
        /// <exception cref="Exception"></exception>
        public Queue GetDocumentQueue(int databaseId, int archiveId, Doc document)
        {
            var Request = new RestRequest($"api/useraction?Database={databaseId}&Archive={archiveId}&Document={document.Id}&SecureId={document.Hash}");
            var Response = ApiClient.Execute<Queue>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get document queue: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Triggers a GlobalAction action on a given document
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/></param>
        /// <param name="document"><see cref="Doc"/></param>
        /// <param name="action"><see cref="DataTypes.Action"/></param>
        /// <exception cref="Exception"></exception>
        public void FireDocumentQueueAction(int databaseId, int archiveId, Doc document, DataTypes.Action action)
        {
            var Request = new RestRequest($"api/useraction?Database={databaseId}&Archive={archiveId}&Document={document.Id}&ActionId={action.Key}&SecureId={document.Hash}", Method.POST);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to trigger document action: {Response.StatusDescription}");
            }
        }
        /// <summary>
        /// This is a helper function that will map out table field data returned by the <see cref="GetArchiveDocument(int, int, int)"/> method into the
        /// <see cref="TableField"/> class. The <see cref="TableField"/> class can be passed directly into <see cref="UpdateTableFieldData(int, int, int, TableField)"/>
        /// </summary>
        /// <param name="databaseId"></param>
        /// <param name="archiveId"></param>
        /// <param name="documentId"></param>
        /// <param name="tableFieldId"></param>
        /// <returns></returns>
        public TableField GetTableFieldData(int databaseId, int archiveId, int documentId, int tableFieldId)
        {
            Result document = GetArchiveDocument(databaseId, archiveId, documentId);
            TableField tfield = new TableField(Fields.GetTableField(databaseId, tableFieldId));

            // map out table field data
            foreach (Doc row in document.Docs)
            {
                List<string> dataRow = new List<string>();
                foreach (int tfColumnFieldId in tfield.Fields)
                {
                    foreach (FieldItem fieldItem in row.Fields)
                    {
                        if (fieldItem.Id == tfColumnFieldId)
                        {
                            dataRow.Add(fieldItem.Val);
                            break;
                        }
                    }
                }
                tfield.Data.Add(dataRow);
            }

            return tfield;
        }
        /// <summary>
        /// Updates a given table field
        /// </summary>
        /// <param name="databaseId"></param>
        /// <param name="archiveId"></param>
        /// <param name="documentId"></param>
        /// <param name="tableField"></param>
        public void UpdateTableFieldData(int databaseId, int archiveId, int documentId, TableField tableField)
        {
            Doc document = GetArchiveDocument(databaseId, archiveId, documentId).Docs[0];
            var Request = new RestRequest($"api/UpdateDocument/databases/{databaseId}/archive/{archiveId}/document/{documentId}/TableData", Method.PUT);
            Request.AddHeader("SecureId", document.Hash);
            Request.AddHeader("Token", License.Token);
            List<TableField> tableFieldItems = new List<TableField>();
            tableFieldItems.Add(tableField);
            Request.AddJsonBody(tableFieldItems);

            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update table field index data: {Response.StatusDescription}");
            }
        }
        /// <summary>
        /// Gets a list of audit log entrys on a given document 
        /// </summary>
        /// <param name="databaseId"></param>
        /// <param name="archiveId"></param>
        /// <param name="document"></param>
        /// <returns>List of <see cref="LogEntry"/></returns>
        public List<LogEntry> GetArchiveDocumentHistory(int databaseId, int archiveId, Doc document)
        {
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/audit", Method.GET);
            Request.AddParameter("secureId", document.Hash);
            Request.AddParameter("token", License.Token);

            var Response = ApiClient.Execute<List<LogEntry>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to retrieve document history: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Adds a new audit log entry to a given document
        /// </summary>
        /// <param name="databaseId"></param>
        /// <param name="archiveId"></param>
        /// <param name="document"></param>
        /// <param name="logEntry"></param>
        /// <exception cref="Exception"></exception>
        public void CreateArchiveDocumentHistoryEntry(int databaseId, int archiveId, Doc document, LogEntry logEntry)
        {
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}", Method.POST);
            Request.AddParameter("documentID", document.Id, ParameterType.QueryString);
            Request.AddParameter("secureid", document.Hash, ParameterType.QueryString);
            Request.AddParameter("token", License.Token, ParameterType.QueryString);
            Request.AddJsonBody(logEntry);

            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to add document history entry: {Response.StatusDescription}");
            }
        }
        #endregion
    }
}
