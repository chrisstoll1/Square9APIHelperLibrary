using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Square9APIHelperLibrary.DataTypes;

namespace Square9APIHelperLibrary.Square9APIComponents
{
    /// <summary>
    /// Inboxes act as a holding folder for <see cref="Documents"/> that have not yet been entered into an <see cref="Archive"/>. Inboxes are not database dependent, but they also contain no index information.
    /// </summary>
    public class Inboxes
    {
        private RestClient ApiClient;
        private string Default;
        private License License;
        internal Inboxes(RestClient apiClient, string defaultSql, License license)
        {
            ApiClient = apiClient;
            Default = defaultSql;
            License = license;
        }

        #region Methods
        /// <summary>
        /// Requests a list of inboxes from the server
        /// </summary>
        /// <returns><see cref="Inbox"/></returns>
        /// <exception cref="Exception"></exception>
        public InboxList GetInboxes()
        {
            var Request = new RestRequest($"api/inboxes");
            var Response = ApiClient.Execute<InboxList>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get inboxes: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests a single specified inbox from the server
        /// </summary>
        /// <param name="inboxId">The ID of the desired inbox</param>
        /// <returns><see cref="Inbox"/></returns>
        /// <exception cref="Exception"></exception>
        public Inbox GetInbox(int inboxId)
        {
            var Request = new RestRequest($"api/inboxes/{inboxId}");
            var Response = ApiClient.Execute<Inbox>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get inbox: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests a list of admin inboxes from the server
        /// </summary>
        /// <returns><see cref="AdminInbox"/></returns>
        /// <exception cref="Exception"></exception>
        public List<AdminInbox> GetAdminInboxes()
        {
            var Request = new RestRequest($"api/admin/inboxes");
            var Response = ApiClient.Execute<List<AdminInbox>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get admin inbox: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Returns a specified inboxes security objects
        /// </summary>
        /// <param name="inboxId">The ID of the desired Inbox</param>
        /// <returns><see cref="Security"/></returns>
        /// <exception cref="Exception"></exception>
        public List<Security> GetAdminInboxSecurity(int inboxId)
        {
            var Request = new RestRequest($"api/admin/inboxes/{inboxId}");
            var Response = ApiClient.Execute<List<Security>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get admin inbox security: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests the GlobalInbox options for the server
        /// </summary>
        /// <returns><see cref="GlobalInboxOptions"/></returns>
        /// <exception cref="Exception"></exception>
        public GlobalInboxOptions GetGlobalInboxOptions()
        {
            var Request = new RestRequest($"api/admin/options/inboxes");
            var Response = ApiClient.Execute<GlobalInboxOptions>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get global inbox options: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Updates the global inbox options on the server
        /// </summary>
        /// <param name="option">The modified <see cref="GlobalInboxOptions"/></param>
        /// <returns><see cref="GlobalInboxOptions"/></returns>
        /// <exception cref="Exception"></exception>
        public GlobalInboxOptions UpdateGlobalInboxOptions(GlobalInboxOptions option)
        {
            var Request = new RestRequest($"api/admin/options/inboxes", Method.Put);
            Request.AddJsonBody(option);
            var Response = ApiClient.Execute<GlobalInboxOptions>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update Global Inbox Options: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Creates a new inbox on the server
        /// </summary>
        /// <param name="inbox">The new <see cref="NewAdminInbox"/></param>
        /// <returns><see cref="AdminInbox"/></returns>
        /// <exception cref="Exception"></exception>
        public AdminInbox CreateInbox(NewAdminInbox inbox)
        {
            var Request = new RestRequest($"api/admin/inboxes", Method.Post);
            Request.AddJsonBody(inbox);
            var Response = ApiClient.Execute<AdminInbox>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create inbox: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Deletes a specified inbox from the server
        /// </summary>
        /// <param name="inboxId">The Id of the desired Inbox</param>
        /// <exception cref="Exception"></exception>
        public void DeleteInbox(int inboxId)
        {
            var Request = new RestRequest($"api/admin/inboxes/{inboxId}", Method.Delete);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to delete inbox: {Response.Content}");
            }
        }
        #endregion
    }
}
