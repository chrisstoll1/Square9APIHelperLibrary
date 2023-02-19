using RestSharp;
using Square9APIHelperLibrary.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Square9APIHelperLibrary
{
    public partial class Square9API
    {
        #region Licenses
        /// <summary>
        /// Requests a license from the server
        /// </summary>
        /// <returns><see cref="License"/></returns>
        public async Task<License> CreateLicenseAsync(CancellationToken cancellationToken)
        {
            var Request = new RestRequest("api/licenses");
            var Response = await ApiClient.ExecuteAsync<License>(Request, cancellationToken);
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
        public async Task DeleteLicenseAsync(License license = null, CancellationToken cancellationToken = default)
        {
            if (license != null || License != null)
            {
                string token = (license != null) ? license.Token : License.Token;
                var Request = new RestRequest($"api/licenses/{token}");
                var Response = await ApiClient.ExecuteAsync(Request, cancellationToken);
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
        public async Task<List<License>> GetLicensesAsync(CancellationToken cancellationToken)
        {
            var Request = new RestRequest($"api/LicenseManager");
            var Response = await ApiClient.ExecuteAsync<List<License>>(Request, cancellationToken);
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
        public async Task ReleaseLicenseAsync(License license, bool forceLogout = false, CancellationToken cancellationToken = default)
        {
            var Request = new RestRequest($"api/LicenseManager?userToken={license.Token}&forceLogout={forceLogout}", Method.Delete);
            var Response = await ApiClient.ExecuteAsync(Request, cancellationToken);
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
        public async Task ReleaseAllLicensesAsync(bool forceLogout = false, CancellationToken cancellationToken = default)
        {
            var Request = new RestRequest($"api/LicenseManager?All=true&forceLogout={forceLogout}", Method.Delete);
            var Response = await ApiClient.ExecuteAsync(Request, cancellationToken);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to release licenses: {Response.Content}");
            }
        }
        #endregion
    }
}
