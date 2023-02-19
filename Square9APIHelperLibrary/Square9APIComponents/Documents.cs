using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Square9APIHelperLibrary.DataTypes;
using File = Square9APIHelperLibrary.DataTypes.File;

namespace Square9APIHelperLibrary.Square9APIComponents
{
    /// <summary>
    /// Documents are image files stored within either archives or inboxes. They contain index information that can be used to retrieve the document via <see cref="Searches"/>
    /// </summary>
    public class Documents
    {
        private RestClient ApiClient;
        private string Default;
        private License License;
        internal Documents(RestClient apiClient, string defaultSql, License license)
        {
            ApiClient = apiClient;
            Default = defaultSql;
            License = license;
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
            Console.WriteLine(SecureID);
            var DocRequest = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{documentId}?SecureId={SecureID}");
            var Response = ApiClient.Execute<Result>(DocRequest);
            if (SecureIDResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get document: {Response.Content}");
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
                throw new Exception($"Unable to get document Meta Data: {Response.Content}");
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
                throw new Exception($"Unable to get document Details: {Response.Content}");
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
            var writer = System.IO.File.OpenWrite(fileName);
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/file?SecureId={document.Hash}")
            {
                ResponseWriter = (responseStream) =>
                {
                    using (responseStream)
                    {
                        responseStream.CopyTo(writer);
                    }
                    return writer;
                }
            };
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to download file: {Response.Content}");
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
            var Request = new RestRequest($"api/inboxes/{inboxId}?FileName={file.FileName}{file.FileType}")
            {
                ResponseWriter = (responseStream) =>
                {
                    using (responseStream)
                    {
                        responseStream.CopyTo(writer);
                    }
                    return writer;
                }
            };
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to download file: {Response.Content}");
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
            var Request = (height == 0 || width == 0) ? new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/thumb?SecureId={document.Hash}") : new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/thumb?SecureId={document.Hash}&height={height}&width={width}")
            {
                ResponseWriter = (responseStream) =>
                {
                    using (responseStream)
                    {
                        responseStream.CopyTo(writer);
                    }
                    return writer;
                }
            };
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to download thumbnail: {Response.Content}");
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
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/documents/{document.Id}/save?SecureId={document.Hash}", Method.Post);
            DocumentIndexData IndexData = new DocumentIndexData();
            IndexData.IndexData.IndexFields = document.Fields;
            Request.AddJsonBody(IndexData);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update document index data: {Response.Content}");
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
            var Request = new RestRequest($"api/files", Method.Post);
            Request.AddFile("File", fileName);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to upload file: {Response.Content}");
            }
            return JsonConvert.DeserializeObject<UploadedFiles>(Response.Content);
        }
        /// <summary>
        /// Imports a uploaded document into a archive
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/></param>
        /// <param name="newFile"><see cref="NewFile"/></param>
        /// <exception cref="Exception"></exception>
        public void ImportArchiveDocument(int databaseId, int archiveId, NewFile newFile, bool useViewerCache = false)
        {
            var Request = new RestRequest((!useViewerCache) ? $"api/dbs/{databaseId}/archives/{archiveId}" : $"api/dbs/{databaseId}/archives/{archiveId}?useViewerCache=true", Method.Post);
            Request.AddJsonBody(newFile);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to index document: {Response.Content}");
            }
        }
        /// <summary>
        /// Imports a new document into a inbox
        /// </summary>
        /// <param name="inboxId">ID of the inbox to import the file to</param>
        /// <param name="file"><see cref="FileDetails"/> Returned by <see cref="UploadDocument(string)"/></param>
        /// <exception cref="Exception"></exception>
        public void ImportInboxDocument(int inboxId, FileDetails file)
        {
            var Request = new RestRequest($"api/inboxes/{inboxId}?FilePath={file.Name}&newFileName={file.OriginalName}", Method.Post);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to import document: {Response.Content}");
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
            var CreateExportJobRequest = new RestRequest($"api/dbs/{databaseId}/export?alwaysExportZip=true&auditEntry=Document+Exported&field={fieldId}", Method.Post);
            CreateExportJobRequest.AddJsonBody(files);
            var CreateExportJobResponse = ApiClient.Execute(CreateExportJobRequest);
            if (CreateExportJobResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create export job: {CreateExportJobResponse.Content}");
            }
            var fileName = (savePath == "") ? $"{System.IO.Path.GetTempPath()}{Guid.NewGuid().ToString()}.zip" : $"{savePath}{CreateExportJobResponse.Content.Replace("\"", "")}.zip";
            var writer = System.IO.File.OpenWrite(fileName);
            Console.WriteLine($"api/dbs/{databaseId}/export?jobid={CreateExportJobResponse.Content.Replace("\"", "")}");
            var GetJobZipRequest = new RestRequest($"api/dbs/{databaseId}/export?jobid={CreateExportJobResponse.Content.Replace("\"", "")}")
            {
                ResponseWriter = (responseStream) =>
                {
                    using (responseStream)
                    {
                        responseStream.CopyTo(writer);
                    }
                    return writer;
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
                throw new Exception($"Unable delete document: {Response.Content}");
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
            var Request = new RestRequest($"api/inboxes/{inboxId}?FilePath={file.FileName}{file.FileType}", Method.Delete);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable delete document: {Response.Content}");
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
                throw new Exception($"Unable to transfer document: {Response.Content}");
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
                throw new Exception($"Unable to transfer document: {Response.Content}");
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
                throw new Exception($"Unable to get document revisions: {Response.Content}");
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
                throw new Exception($"Unable to get document queue: {Response.Content}");
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
            var Request = new RestRequest($"api/useraction?Database={databaseId}&Archive={archiveId}&Document={document.Id}&ActionId={action.Key}&SecureId={document.Hash}", Method.Post);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to trigger document action: {Response.Content}");
            }
        }
        #endregion
    }
}
