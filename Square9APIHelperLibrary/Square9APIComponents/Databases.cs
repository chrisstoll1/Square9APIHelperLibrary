using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Square9APIHelperLibrary.DataTypes;

namespace Square9APIHelperLibrary.Square9APIComponents
{
    /// <summary>
    /// Each database in GlobalSearch has an ID and is self-contained. Access to all documents begins with targeting the correct database.
    /// </summary>
    public class Databases
    {
        private RestClient ApiClient;
        private string Default;
        private License License;
        internal Databases(RestClient apiClient, string defaultSql, License license)
        {
            ApiClient = apiClient;
            Default = defaultSql;
            License = license;
        }

        #region Methods
        /// <summary>
        /// Requests a list of databases or a specific database from the server
        /// </summary>
        /// <example>
        /// <code>
        /// DatabaseList databases = Connection.Databases.GetDatabases();
        /// </code>
        /// </example>
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
        /// Requests a list of databases from the server using the admin api endpoint. Returns additional information for each database over the normal <see cref="GetDatabases(int)"/>
        /// </summary>
        /// <example>
        /// <code>
        /// List<AdminDatabase> Databases = Connection.Databases.GetAdminDatabases();
        /// </code>
        /// </example>
        /// <param name="database">Optional: The database you would like to return in the list</param>
        /// <returns>List of <see cref="AdminDatabase"/></returns>
        public List<AdminDatabase> GetAdminDatabases(Database database = null)
        {
            var Request = (database != null) ? new RestRequest($"api/admin/databases/{database.Id}") : new RestRequest("api/admin/databases");
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
        /// <example>
        /// <code>
        /// AdminDatabase NewDatabase = new NewAdminDatabase("NewTestDatabase");
        /// AdminDatabase Database = Connection.Databases.CreateDatabase(NewDatabase);
        /// </code>
        /// </example>
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
        /// Updates the properties of an existing database on the server
        /// </summary>
        /// <example>
        /// <code>
        /// Database.Name = "DatabaseModified";
        /// Database = Connection.Databases.UpdateDatabase(Database);
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// Connection.Databases.DeleteDatabase(Database, true);
        /// </code>
        /// </example>
        /// <param name="database">The ID of the database to be deleted <see cref="Database"/></param>
        /// <param name="drop">Optional: Determines if the database should be dropped from SQL, set to false by default</param>
        public void DeleteDatabase(Database database, bool drop = false)
        {
            var Request = new RestRequest($"api/admin/databases/{database.Id}?Drop={drop}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to delete database: {Response.Content}");
            }
        }
        /// <summary>
        /// Requests a SQL index rebuild on a specific database
        /// </summary>
        /// <example>
        /// <code>
        /// Connection.Databases.RebuildDatabaseIndex(Database.Id);
        /// </code>
        /// </example>
        /// <param name="database">The database to be rebuilt <see cref="Database"/></param>
        public void RebuildDatabaseIndex(Database database)
        {
            var Request = new RestRequest($"api/admin/databases/{database.Id}?rebuild=true");
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to rebuild database index: {Response.Content}");
            }
        }
        #endregion
    }
}
