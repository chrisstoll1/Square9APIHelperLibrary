using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Square9APIHelperLibrary.DataTypes;

namespace Square9APIHelperLibrary.Square9APIComponents
{
    /// <summary>
    /// Archives are contained within databases. Each archive has its own index fields and searches. All documents in GlobalSearch with the exception of those contained in <see cref="Inboxes"/>, are contained within archives.
    /// </summary>
    public class Archives
    {
        private RestClient ApiClient;
        private string Default;
        private License License;
        internal Archives(RestClient apiClient, string defaultSql, License license)
        {
            ApiClient = apiClient;
            Default = defaultSql;
            License = license;
        }

        #region Methods
        /// <summary>
        /// Requests a list of root archives in a database or sub-archives in a parent archive
        /// </summary>
        /// <example>
        /// <code>
        /// ArchiveList archives = Connection.Archives.GetArchives(database.Id);
        /// ArchiveList subArchives = Connection.Archives.GetArchives(database.Id, archives.Archives[0].Id);
        /// </code>
        /// </example>
        /// <param name="databaseId">The ID of the database you would like to return a list of archives from</param>
        /// <param name="archiveId">Optional: The ID of the archive you would like to return a list of archives from</param>
        /// <returns><see cref="ArchiveList"/></returns>
        public ArchiveList GetArchives(int databaseId, int archiveId = 0)
        {
            var Request = (archiveId >= 1) ? new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}") : new RestRequest($"api/dbs/{databaseId}/archives");
            var Response = ApiClient.Execute<ArchiveList>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get archives: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests a list of archives in a database or sub archives in a archive from the server, includes additional admin only details
        /// </summary>
        /// <param name="databaseId">The ID of the database you would like to return a list of archives from</param>
        /// <param name="archiveId">Optional: The ID of the archive you would like to return a list of archvies from</param>
        /// <returns>List of <see cref="AdminArchive"/></returns>
        public List<AdminArchive> GetAdminArchives(int databaseId, int archiveId = 0)
        {
            var Request = (archiveId >= 1) ? new RestRequest($"api/admin/databases/{databaseId}/archives/{archiveId}") : new RestRequest($"api/admin/databases/{databaseId}/archives");
            var Response = ApiClient.Execute<List<AdminArchive>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get archives: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Request a list of fields in a archive or sub archive from the server
        /// </summary>
        /// <param name="databaseId">The ID of the database the archive is in</param>
        /// <param name="archiveId">The ID of the archive to return fields from</param>
        /// <returns>List of <see cref="Field"/></returns>
        public List<Field> GetArchiveFields(int databaseId, int archiveId)
        {
            var Request = new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}?type=fields");
            var Response = ApiClient.Execute<List<Field>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get archive fields: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Creates a new archive on the server
        /// </summary>
        /// <param name="databaseId">The database id where the archive should be created</param>
        /// <param name="archive">The archive to be created</param>
        /// <returns><see cref="AdminArchive"/></returns>
        public AdminArchive CreateArchive(int databaseId, NewAdminArchive archive)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/archives", Method.POST);
            Request.AddJsonBody(archive);
            var Response = ApiClient.Execute<AdminArchive>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create archive: {Response.StatusDescription} \n {JsonConvert.SerializeObject(archive)}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Deletes a archive on the server
        /// </summary>
        /// <param name="databaseId">Database ID that relates to the archive</param>
        /// <param name="archiveId">Archive ID of the archive to be deleted</param>
        public void DeleteArchive(int databaseId, int archiveId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/archives/{archiveId}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to delete archive: {Response.StatusDescription}");
            }
        }
        /// <summary>
        /// Edit the properties of a archive
        /// </summary>
        /// <param name="databaseId">Database ID that relates to the archive</param>
        /// <param name="archive">Archive to be updated</param>
        /// <returns></returns>
        public AdminArchive UpdateArchive(int databaseId, AdminArchive archive)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/archives/{archive.Id}", Method.PUT);
            Request.AddJsonBody(archive);
            var Response = ApiClient.Execute<AdminArchive>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update archive: {Response.StatusDescription} \n {JsonConvert.SerializeObject(archive)}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests Global Archive Options from the specified database on the server
        /// </summary>
        /// <param name="databaseId">ID of the database to request options from</param>
        /// <returns><see cref="GlobalArchiveOptions"/></returns>
        public GlobalArchiveOptions GetGlobalArchiveOptions(int databaseId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/options/archives");
            var Response = ApiClient.Execute<GlobalArchiveOptions>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get GlobalArchiveOptions: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Updates the Global Archive Options for the specified database on the server
        /// </summary>
        /// <param name="databaseId">ID of the database to update options on</param>
        /// <param name="globalArchiveOptions">Options object to update</param>
        /// <returns><see cref="GlobalArchiveOptions"/></returns>
        public GlobalArchiveOptions UpdateGlobalArchiveOptions(int databaseId, GlobalArchiveOptions globalArchiveOptions)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/options/archives", Method.PUT);
            Request.AddJsonBody(globalArchiveOptions);
            var Response = ApiClient.Execute<GlobalArchiveOptions>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update GlobalArchiveOptions: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Send a number of documents to the queue to be run through content indexing
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="archiveId">Archive ID</param>
        /// <param name="count">Number of most recent documents to index</param>
        /// <returns><see cref="bool"/></returns>
        public bool RebuildArchiveContentIndex(int databaseId, int archiveId, int count = 1000)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/archives/{archiveId}/indexrebuild?docCount={count}");
            var Response = ApiClient.Execute<bool>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to rebuild archive content index: {Response.StatusDescription}");
            }
            return Response.Data;
        }
        #endregion
    }
}
