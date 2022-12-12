using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Square9APIHelperLibrary.DataTypes;

namespace Square9APIHelperLibrary.Square9APIComponents
{
    /// <summary>
    /// The Administration API provides access for appropriately authenticated administrator users to add, create, update, and delete SmartSearch resources.
    /// </summary>
    public class Administration
    {
        private RestClient ApiClient;
        private string Default;
        private License License;
        internal Administration(RestClient apiClient, string defaultSql, License license)
        {
            ApiClient = apiClient;
            Default = defaultSql;
            License = license;
        }

        #region Methods
        /// <summary>
        /// Returns a list of all users and groups that are secured to any database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<SecuredGroup> GetSecuredUsersAndGroups()
        {
            var Request = new RestRequest($"api/userAdmin/secured");
            var Response = ApiClient.Execute<List<SecuredGroup>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get secured users and groups: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Returns a list of all users and groups that are not secured to any database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<UnsecuredGroup> GetUnsecuredUsersAndGroups()
        {
            var Request = new RestRequest($"api/userAdmin/unsecured");
            var Response = ApiClient.Execute<List<UnsecuredGroup>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get unsecured users and groups: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Returns an object containing databases, their associated archives, and searches
        /// </summary>
        /// <returns><see cref="SecurityNode"/></returns>
        /// <exception cref="Exception"></exception>
        public List<SecurityNode> GetTreeStructure()
        {
            var Request = new RestRequest($"api/userAdmin/tree");
            var Response = ApiClient.Execute<List<SecurityNode>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get server tree view: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Returns specified user's archive permissions for a given archive
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="archiveId"><see cref="Archive.Id"/></param>
        /// <param name="username">Username to return permissions on</param>
        /// <returns><see cref="ArchivePermission"/></returns>
        /// <exception cref="Exception"></exception>
        public ArchivePermission GetUserArchivePermissions(int databaseId, int archiveId, string username)
        {
            var Request = new RestRequest($"api/userAdmin/archives?db={databaseId}&archive={archiveId}&username={username}");
            var Response = ApiClient.Execute<int>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get archive permissions: {Response.Content}");
            }
            return new ArchivePermission(Response.Data);
        }
        /// <summary>
        /// Returns specified user's inbox permissions for a given archive
        /// </summary>
        /// <param name="inbox"><see cref="Inbox.Id"/></param>
        /// <param name="username">Username to return permissions on</param>
        /// <returns><see cref="InboxPermission"/></returns>
        /// <exception cref="Exception"></exception>
        public InboxPermission GetUserInboxPermissions(int inbox, string username)
        {
            var Request = new RestRequest($"api/userAdmin/inboxes?inbox={inbox}&username={username}");
            var Response = ApiClient.Execute<int>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get archive permissions: {Response.Content}");
            }
            return new InboxPermission(Response.Data);
        }
        /// <summary>
        /// Returns specified user's search properties for a given database
        /// </summary>
        /// <param name="databaseId"><see cref="Database.Id"/></param>
        /// <param name="username">Username to return search properties of</param>
        /// <returns><see cref="SearchProperties"/></returns>
        /// <exception cref="Exception"></exception>
        public List<SearchProperties> GetUserSearchProperties(int databaseId, string username)
        {
            var Request = new RestRequest($"api/userAdmin/searches?db={databaseId}&username={username}");
            var Response = ApiClient.Execute<List<SearchProperties>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Cannot get user search properties: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Sets the security for a given user/group on a given archive
        /// </summary>
        /// <param name="archiveSecurity"><see cref="ArchiveSecurity"/></param>
        /// <exception cref="Exception"></exception>
        public void SetArchiveSecurity(ArchiveSecurity archiveSecurity)
        {
            var Request = new RestRequest($"api/userAdmin/archives", Method.POST);
            Request.AddJsonBody(archiveSecurity);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK || !Response.Content.Contains("\"Saved\":1"))
            {
                throw new Exception($"Unable to save archive security: {Response.Content}");
            }
        }
        /// <summary>
        /// Sets the security for a given user/group on a given inbox
        /// </summary>
        /// <param name="inboxSecurity"><see cref="InboxSecurity"/></param>
        /// <exception cref="Exception"></exception>
        public void SetInboxSecurity(InboxSecurity inboxSecurity)
        {
            var Request = new RestRequest($"api/userAdmin/inboxes", Method.POST);
            Request.AddJsonBody(inboxSecurity);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to save inbox security: {Response.Content}");
            }
        }
        /// <summary>
        /// Sets the security for a given user/group on a given database
        /// </summary>
        /// <param name="databaseSecurity"><see cref="DatabaseSecurity"/></param>
        /// <exception cref="Exception"></exception>
        public void SetDatabaseSecurity(DatabaseSecurity databaseSecurity)
        {
            var Request = new RestRequest($"api/userAdmin/databases", Method.POST);
            Request.AddJsonBody(databaseSecurity);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK || !Response.Content.Contains("\"Success\""))
            {
                throw new Exception($"Unable to save database security: {Response.Content}");
            }
        }
        /// <summary>
        /// Sets the security for a given user/group on a given search
        /// </summary>
        /// <param name="searchSecurity"><see cref="SearchSecurity"/></param>
        /// <exception cref="Exception"></exception>
        public void SetSearchSecurity(SearchSecurity searchSecurity)
        {
            var Request = new RestRequest($"api/userAdmin/searches", Method.POST);
            Request.AddJsonBody(searchSecurity);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK || !Response.Content.Contains("\"Success\""))
            {
                throw new Exception($"Unable to save search security: {Response.Content}");
            }
        }
        /// <summary>
        /// Sets the search properties for a given user/group on a given search
        /// </summary>
        /// <param name="searchSecurity"><see cref="SearchSecurity"/></param>
        /// <exception cref="Exception"></exception>
        public void SetSearchProperties(SearchSecurity searchSecurity)
        {
            var Request = new RestRequest($"api/userAdmin/searchType", Method.POST);
            Request.AddJsonBody(searchSecurity);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to save search properties: {Response.Content}");
            }
        }
        /// <summary>
        /// Creates a new S9 user on the server
        /// </summary>
        /// <param name="newUser"><see cref="User"/></param>
        /// <exception cref="Exception"></exception>
        public void CreateUser(User newUser)
        {
            var Request = new RestRequest($"api/userAdmin/user?create=", Method.POST);
            Request.AddJsonBody(newUser);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create new user: {Response.Content}");
            }
        }
        /// <summary>
        /// Deletes a S9 user from the server
        /// </summary>
        /// <param name="user"><see cref="User"/></param>
        /// <exception cref="Exception"></exception>
        public void DeleteUser(User user)
        {
            var Request = new RestRequest($"api/userAdmin/user?name={user.Name}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to delete user: {Response.Content}");
            }
        }
        /// <summary>
        /// Updates a S9 user on the server
        /// </summary>
        /// <param name="user"><see cref="User"/></param>
        /// <exception cref="Exception"></exception>
        public void UpdateUser(User user)
        {
            var Request = new RestRequest($"api/userAdmin/user?name={user.Name}", Method.PUT);
            Request.AddJsonBody(user);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update user: {Response.Content}");
            }
        }
        /// <summary>
        /// Creates a new S9 group on the server
        /// </summary>
        /// <param name="newGroup"><see cref="Group"/></param>
        /// <exception cref="Exception"></exception>
        public void CreateGroup(Group newGroup)
        {
            var Request = new RestRequest($"api/userAdmin/group?createGroup=", Method.POST);
            Request.AddJsonBody(newGroup);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create new group: {Response.Content}");
            }
        }
        /// <summary>
        /// Deletes a S9 group from the server
        /// </summary>
        /// <param name="group"><see cref="Group"/></param>
        /// <exception cref="Exception"></exception>
        public void DeleteGroup(Group group)
        {
            var Request = new RestRequest($"api/userAdmin/group?name={group.Name}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to delete group: {Response.Content}");
            }
        }
        /// <summary>
        /// Updates a S9 group on the server
        /// </summary>
        /// <param name="group"><see cref="Group"/></param>
        /// <exception cref="Exception"></exception>
        public void UpdateGroup(Group group)
        {
            var Request = new RestRequest($"api/userAdmin/group?groupName={group.Name}", Method.PUT);
            Request.AddJsonBody(group);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update group: {Response.Content}");
            }
        }
        /// <summary>
        /// Requests a list of S9 groups and members on the server
        /// </summary>
        /// <returns><see cref="Group"/></returns>
        /// <exception cref="Exception"></exception>
        public List<Group> GetGroups()
        {
            var Request = new RestRequest($"api/userAdmin/s9groups");
            var Response = ApiClient.Execute<List<Group>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get groups: {Response.Content}");
            }
            return Response.Data;
        }
        #endregion

    }
}
