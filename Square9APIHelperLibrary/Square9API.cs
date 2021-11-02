using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Square9APIHelperLibrary.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Square9APIHelperLibrary
{
    public class Square9API
    {
        #region Variables
        /// <summary>
        /// Rest API Connection, used to make requests to the server
        /// </summary>
        private static RestClient ApiClient;
        /// <summary>
        /// The Default SQL Instance returned by the server
        /// Loaded everytime a new license is created 
        /// <see cref="GetDefault"/> <seealso cref="CreateLicense"/>
        /// </summary>
        public string Default;
        /// <summary>
        /// Cached license information, contains connection information
        /// <see cref="CreateLicense"/>
        /// </summary>
        public License License;
        #endregion

        #region Setup
        /// <summary>
        /// Creates a new connection to the Square9API
        /// </summary>
        /// <param name="endpoint">Must be the full API endpoint (http://localhost/Square9API/)</param>
        /// <param name="username">Username of the account to authenticate with</param>
        /// <param name="password">Password of the account to authenticate with</param>
        /// <returns>Nothing</returns>
        public Square9API(string endpoint, string username, string password)
        {
            ApiClient = new RestClient(endpoint)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
        }
        /// <summary>
        /// Checks if the current authenticated user is an Administrator
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool IsAdmin()
        {
            var Request = new RestRequest("api/userAdmin/isAdmin");
            var Response = ApiClient.Execute<bool>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Loads default SQL Instance from the server
        /// <see cref="Default"/>
        /// </summary>
        /// <returns><see cref="string"/></returns>
        private string GetDefault()
        {
            var Request = new RestRequest("api/admin/instances/default");
            var Response = ApiClient.Execute<string>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred: {Response.Content}");
            }
            return Response.Data;
        }
        #endregion

        #region Licenses
        /// <summary>
        /// Requests a license from the server
        /// </summary>
        /// <returns><see cref="License"/></returns>
        public License CreateLicense()
        {
            var Request = new RestRequest("api/licenses");
            var Response = ApiClient.Execute<License>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                if (Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Unable to get a License: The passed user is Unauthorized.");
                }
                else if (Response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new Exception($"Unable to get a License: {Response.Content}");
                }
                else if (Response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Unable to get a License: Unable to connect to the license server, server not found.");
                }
                else if (Response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception("Unable to get a License: 403 Forbidden.");
                }
                else if (Response.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    throw new Exception("Unable to get a License: The Request Timed out.");
                }
                else
                {
                    throw new Exception("Unable to get a License: Please check your connection settings.");
                }
            }
            License = Response.Data;
            Default = GetDefault();
            return Response.Data;
        }
        /// <summary>
        /// Requests for a license to be deleted from the server
        /// </summary>
        /// <param name="token">Must be a active token</param>
        public void DeleteLicense()
        {
            var Request = new RestRequest("api/licenses/" + License.Token);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to release license token: {Response.Content}");
            }
        }
        #endregion

        #region Database
        /// <summary>
        /// Requests a list of databases from the server
        /// </summary>
        /// <param name="databaseId">Optional: The ID of the database you would like to return in the list</param>
        /// <returns><see cref="DatabaseList"/></returns>
        public DatabaseList GetDatabases(int databaseId = 0)
        {
            var Request = (databaseId >= 1) ? new RestRequest($"api/dbs/{databaseId}") : new RestRequest("api/dbs");
            var Response = ApiClient.Execute<DatabaseList>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get databases: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests a list of databases from the server using the admin api endpoint. Returns additional information for each database over the normal <see cref="GetDatabaseList(int)"/>
        /// </summary>
        /// <param name="databaseId">Optional: The ID of the database you would like to return in the list</param>
        /// <returns>List of <see cref="AdminDatabase"/></returns>
        public List<AdminDatabase> GetAdminDatabases(int databaseId = 0)
        {
            var Request = (databaseId >= 1) ? new RestRequest($"api/admin/databases/{databaseId}") : new RestRequest("api/admin/databases");
            var Response = ApiClient.Execute<List<AdminDatabase>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get databases: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Creates a new database on the server
        /// </summary>
        /// <param name="database">The new database to be created on the server <see cref="NewAdminDatabase"/></param>
        /// <returns><see cref="AdminDatabase"/></returns>
        public AdminDatabase CreateDatabase(AdminDatabase database)
        {
            var Request = new RestRequest($"api/admin/databases", Method.POST);
            if (database.Server == null) { database.Server = Default; }
            Request.AddJsonBody(database);
            var Response = ApiClient.Execute<AdminDatabase>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create database: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Updates a existing database on the server
        /// </summary>
        /// <param name="database">The existing database to be updates on the server <see cref="AdminDatabase"/></param>
        /// <returns><see cref="AdminDatabase"/></returns>
        public AdminDatabase UpdateDatabase(AdminDatabase database)
        {
            var Request = new RestRequest($"api/admin/databases/{database.Id}", Method.PUT);
            if (database.Server == null) { database.Server = Default; }
            Request.AddJsonBody(database);
            var Response = ApiClient.Execute<AdminDatabase>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update database: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Deletes a existing database on the server
        /// </summary>
        /// <param name="databaseId">The ID of the database to be deleted <see cref="Database.Id"/></param>
        /// <param name="drop">Optional: Determines if the database should be dropped from SQL, set to false by default</param>
        public void DeleteDatabase(int databaseId, bool drop = false)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}?Drop={drop}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to delete database: {Response.Content}");
            }
        }
        /// <summary>
        /// Requests a SQL index rebuild on all tables withing the passed database
        /// </summary>
        /// <param name="databaseId">The ID of the database to be rebuilt <see cref="Database.Id"/></param>
        public void RebuildDatabaseIndex(int databaseId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}?rebuild=true");
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to rebuild database index: {Response.Content}");
            }
        }
        #endregion

        #region Archive
        /// <summary>
        /// Requests a list of root archives in a database or sub archives in a archive from the server
        /// </summary>
        /// <param name="databaseId">The ID of the database you would like to return a list of archives from</param>
        /// <param name="archiveId">Optional: The ID of the archive you would like to return a list of archives from</param>
        /// <returns><see cref="ArchiveList"/></returns>
        public ArchiveList GetArchives(int databaseId, int archiveId = 0)
        {
            var Request = (archiveId >= 1) ? new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}") : new RestRequest($"api/dbs/{databaseId}/archives");
            var Response = ApiClient.Execute<ArchiveList>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get archives: {Response.Content}");
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
                throw new Exception($"Unable to get archives: {Response.Content}");
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
                throw new Exception($"Unable to get archive fields: {Response.Content}");
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
                throw new Exception($"Unable to create archive: {Response.Content} \n {JsonConvert.SerializeObject(archive)}");
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
                throw new Exception($"Unable to delete archive: {Response.Content}");
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
                throw new Exception($"Unable to update archive: {Response.Content} \n {JsonConvert.SerializeObject(archive)}");
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
                throw new Exception($"Unable to get GlobalArchiveOptions: {Response.Content}");
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
                throw new Exception($"Unable to update GlobalArchiveOptions: {Response.Content}");
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
                throw new Exception($"Unable to rebuild archive content index: {Response.Content}");
            }
            return Response.Data;
        }
        #endregion

        #region Search
        /// <summary>
        /// Requests a list of searches from the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="archiveId">Optional: Archive ID</param>
        /// <returns><see cref="Search"/></returns>
        public List<Search> GetSearches(int databaseId, int archiveId = 0)
        {
            var Request = (archiveId >= 1) ? new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/searches") : new RestRequest($"api/dbs/{databaseId}/searches");
            var Response = ApiClient.Execute<List<Search>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get searches: {Response.Content}");
            }
            return Response.Data;
        }
        #endregion

        #region Inbox

        #endregion

        #region Field

        #endregion

        #region Document

        #endregion

        #region Administration

        #endregion

    }
}
