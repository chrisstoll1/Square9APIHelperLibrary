using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Square9APIHelperLibrary.DataTypes;
using Square9APIHelperLibrary.Square9APIComponents;
using System;
using System.Collections.Generic;
using System.Net;

namespace Square9APIHelperLibrary
{
    /// <summary>
    /// Used as the main entry point for the Square9API
    /// </summary>
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
        public Databases Databases;
        public Archives Archives;
        public Searches Searches;
        public Inboxes Inboxes;
        public Fields Fields;
        public Documents Documents;
        public Administration Administration;
        #endregion

        #region System
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
            RebuildComponents();
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
        /// <summary>
        /// Returns the user currently logged in
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetUsername()
        {
            var Request = new RestRequest("api/admin");
            var Response = ApiClient.Execute<string>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Configures the outgoing mail server settings
        /// </summary>
        /// <param name="databaseId">The database you would like to update these settings for</param>
        /// <param name="emailServer"><see cref="EmailServer"/></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public EmailServer UpdateEmailOptions(int databaseId, EmailServer emailServer)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/notifications", Method.PUT);
            Request.AddJsonBody(emailServer);
            var Response = ApiClient.Execute<EmailServer>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred while updating email options: {Response.Content}");
            }
            return JsonConvert.DeserializeObject<EmailServer>(Response.Content);
        }
        /// <summary>
        /// Gets the current outgoing mail server settings
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <returns><see cref="EmailServer"/></returns>
        /// <exception cref="Exception"></exception>
        public EmailServer GetEmailOptions(int databaseId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/notifications");
            var Response = ApiClient.Execute<EmailServer>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred while retrieving email options: {Response.Content}");
            }
            return JsonConvert.DeserializeObject<EmailServer>(Response.Content);
        }
        /// <summary>
        /// Creates a new stamp on the server
        /// </summary>
        /// <param name="stamp"><see cref="Stamp"/></param>
        /// <returns><see cref="Stamp"/></returns>
        /// <exception cref="Exception"></exception>
        public Stamp CreateStamp(Stamp stamp)
        {
            var Request = new RestRequest($"api/admin/stamps", Method.POST);
            Request.AddJsonBody(stamp);
            var Response = ApiClient.Execute<Stamp>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred creating stamp: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Returns a list of stamps on the server
        /// </summary>
        /// <returns><see cref="Stamp"/></returns>
        /// <exception cref="Exception"></exception>
        public List<Stamp> GetStamps()
        {
            var Request = new RestRequest($"api/admin/stamps");
            var Response = ApiClient.Execute<List<Stamp>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred while retrieving stamps: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Deletes a stamp from the server
        /// </summary>
        /// <param name="stamp"><see cref="Stamp"/></param>
        /// <exception cref="Exception"></exception>
        public void DeleteStamp(Stamp stamp)
        {
            var Request = new RestRequest($"api/admin/stamps/{stamp.Id}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred deleting stamp: {Response.Content}");
            }
        }
        /// <summary>
        /// Get the server's current registration status
        /// </summary>
        /// <returns><see cref="Registration"/></returns>
        /// <exception cref="Exception"></exception>
        public Registration GetRegistration()
        {
            var Request = new RestRequest($"api/admin/registration");
            var Response = ApiClient.Execute<Registration>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred while retrieving registration: {Response.Content}");
            }
            Registration registration = Response.Data;
            var FeatureRequest = new RestRequest($"api/admin/registration?features=true", Method.GET);
            var FeatureResponse = ApiClient.Execute(FeatureRequest);
            if (FeatureResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred while retrieving registration: {FeatureResponse.Content}");
            }
            registration.Features = JsonConvert.DeserializeObject<IDictionary<string, string>>(FeatureResponse.Content);
            return registration;
        }
        /// <summary>
        /// Submits a request to Square 9 for registration data
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="serial"></param>
        /// <exception cref="Exception"></exception>
        public void RequestWebRegistration(string uniqueId, string serial)
        {
            Register register = new Register(uniqueId, serial);
            var Request = new RestRequest($"api/admin/registration", Method.POST);
            Request.AddJsonBody(register);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred while requesting web registration: {Response.Content}");
            }
        }
        /// <summary>
        /// Manually registers the server
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="serial"></param>
        /// <param name="registration"></param>
        /// <exception cref="Exception"></exception>
        public void ManualRegistration(string uniqueId, string serial, string registration)
        {
            Register register = new Register(uniqueId, serial, registration);
            var Request = new RestRequest($"api/admin/registration", Method.PUT);
            Request.AddJsonBody(register);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"An error occurred while performing manual registration: {Response.Content}");
            }
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
            RebuildComponents();
            return Response.Data;
        }
        /// <summary>
        /// Requests for a license to be deleted from the server
        /// </summary>
        /// <param name="license">Must be a active license</param>
        public void DeleteLicense(License license = null)
        {
            if (license != null || License != null)
            {
                string token = (license != null) ? license.Token : License.Token;
                var Request = new RestRequest($"api/licenses/{token}");
                var Response = ApiClient.Execute(Request);
                if (Response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Unable to release license token: {Response.Content}");
                }
                License = null; //Delete cached license
            }
        }
        /// <summary>
        /// Requests a list of all licenses from the server
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<License> GetLicenses()
        {
            var Request = new RestRequest($"api/LicenseManager");
            var Response = ApiClient.Execute<List<License>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get licenses: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Releases a specific license from the server, can enforce logout with optional parameter
        /// </summary>
        /// <param name="license"><see cref="License"/></param>
        /// <param name="forceLogout">When true will delete license token from the server, forcing user to log back in</param>
        /// <exception cref="Exception"></exception>
        public void ReleaseLicense(License license, bool forceLogout = false)
        {
            var Request = new RestRequest($"api/LicenseManager?userToken={license.Token}&forceLogout={forceLogout}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to release license: {Response.Content}");
            }
        }
        /// <summary>
        /// Releases all licenses from the server, can enforce logout with optional parameter
        /// </summary>
        /// <param name="forceLogout">When true will delete license token from the server, forcing user to log back in</param>
        /// <exception cref="Exception"></exception>
        public void ReleaseAllLicenses(bool forceLogout = false)
        {
            var Request = new RestRequest($"api/LicenseManager?All=true&forceLogout={forceLogout}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to release licenses: {Response.Content}");
            }
        }
        #endregion

        private void RebuildComponents()
        {
            Databases = new Databases(ApiClient, Default, License);
            Archives = new Archives(ApiClient, Default, License);
            Searches = new Searches(ApiClient, Default, License);
            Inboxes = new Inboxes(ApiClient, Default, License);
            Fields = new Fields(ApiClient, Default, License);
            Documents = new Documents(ApiClient, Default, License);
            Administration = new Administration(ApiClient, Default, License);
        }

    }
}
