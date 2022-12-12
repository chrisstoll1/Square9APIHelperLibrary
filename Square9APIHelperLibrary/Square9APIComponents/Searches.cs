using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Square9APIHelperLibrary.DataTypes;

namespace Square9APIHelperLibrary.Square9APIComponents
{
    /// <summary>
    /// Searches are querys used to retrieve <see cref="Documents"/> from <see cref="Archives"/> by building conditional statements off of Index <see cref="Fields"/>
    /// </summary>
    public class Searches
    {
        private RestClient ApiClient;
        private string Default;
        private License License;
        internal Searches(RestClient apiClient, string defaultSql, License license)
        {
            ApiClient = apiClient;
            Default = defaultSql;
            License = license;
        }

        #region Methods
        /// <summary>
        /// Requests a list of searches from the server
        /// Can be used to request a list of all searches from a database, archive or an individual search
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="archiveId">Optional: Archive ID</param>
        /// <param name="searchId">Optional: Search ID</param>
        /// <returns><see cref="Search"/></returns>
        public List<Search> GetSearches(int databaseId, int archiveId = 0, int searchId = 0)
        {
            var Request = (searchId >= 1) ? new RestRequest($"api/dbs/{databaseId}/searches/{searchId}") : (archiveId >= 1) ? new RestRequest($"api/dbs/{databaseId}/archives/{archiveId}/searches") : new RestRequest($"api/dbs/{databaseId}/searches");
            var Response = ApiClient.Execute<List<Search>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get searches: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Returns results from a single search
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="search">The Search you'd like to return results for <see cref="Search"/></param>
        /// <param name="page">Optional: Denotes the specific page or results returned (Default is 1)</param>
        /// <param name="recordsPerPage">Optional: Denotes how many records should be returned per page (Default is 50)</param>
        /// <param name="tabId">Optional: Returns results from a specific view tab</param>
        /// <param name="sort">Optional: Sorts results based on desired column</param>
        /// <param name="time">Optional: Epoch time stamp</param>
        /// <returns><see cref="Result"/></returns>
        public Result GetSearchResults(int databaseId, Search search, int page = 0, int recordsPerPage = 0, int tabId = 0, int sort = 0, int time = 0)
        {
            //Format Search Criteria
            List<string> searchCriteria = new List<string>();
            foreach (SearchDetail criteria in search.Detail)
            {
                if (criteria.Val != "")
                {
                    searchCriteria.Add($"{criteria.Id}:\"{criteria.Val}\"");
                }
            }
            string pageParam = (page >= 1) ? $"&Page={page}" : "";
            string recordsPerPageParam = (recordsPerPage >= 1) ? $"&RecordsPerPage={recordsPerPage}" : "";
            string tabIdParam = (tabId >= 1) ? $"&tabId={tabId}" : "";
            string sortParam = (sort >= 1) ? $"&Sort={sort}" : "";
            string timeParam = (time >= 1) ? $"&time={time}" : "";
            var Request = new RestRequest($"api/dbs/{databaseId}/searches/{search.Id}/archive/{search.Parent}/documents?SecureId={search.Hash}&SearchCriteria={{{string.Join(",", searchCriteria)}}}{pageParam}{recordsPerPageParam}{tabIdParam}{sortParam}{timeParam}&Count=false");
            var Response = ApiClient.Execute<Result>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get search results: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Returns count from a single search
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="search">The Search you'd like to return results for <see cref="Search"/></param>
        /// <param name="page">Optional: Denotes the specific page or results returned (Default is 1)</param>
        /// <param name="recordsPerPage">Optional: Denotes how many records should be returned per page (Default is 50)</param>
        /// <param name="tabId">Optional: Returns results from a specific view tab</param>
        /// <param name="sort">Optional: Sorts results based on desired column</param>
        /// <param name="time">Optional: Epoch time stamp</param>
        /// <returns><see cref="ArchiveCount"/></returns>
        public ArchiveCount GetSearchCount(int databaseId, Search search, int page = 0, int recordsPerPage = 0, int tabId = 0, int sort = 0, int time = 0)
        {
            //Format Search Criteria
            List<string> searchCriteria = new List<string>();
            foreach (SearchDetail criteria in search.Detail)
            {
                if (criteria.Val != "")
                {
                    searchCriteria.Add($"{criteria.Id}:\"{criteria.Val}\"");
                }
            }
            string pageParam = (page >= 1) ? $"&Page={page}" : "";
            string recordsPerPageParam = (recordsPerPage >= 1) ? $"&RecordsPerPage={recordsPerPage}" : "";
            string tabIdParam = (tabId >= 1) ? $"&tabId={tabId}" : "";
            string sortParam = (sort >= 1) ? $"&Sort={sort}" : "";
            string timeParam = (time >= 1) ? $"&time={time}" : "";
            var Request = new RestRequest($"api/dbs/{databaseId}/searches/{search.Id}/archive/{search.Parent}/documents?SecureId={search.Hash}&SearchCriteria={{{string.Join(",", searchCriteria)}}}{pageParam}{recordsPerPageParam}{tabIdParam}{sortParam}{timeParam}&Count=true");
            var Response = ApiClient.Execute<ArchiveCount>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get search count: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Creates a new search on the server
        /// </summary>
        /// <param name="databaseId">The database id where the search should be created</param>
        /// <param name="search">The search to be created</param>
        /// <returns><see cref="AdminSearch"/></returns>
        public AdminSearch CreateSearch(int databaseId, NewAdminSearch search)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/searches", Method.POST);
            Request.AddJsonBody(search);
            var Response = ApiClient.Execute<AdminSearch>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create search: {Response.Content} \n {JsonConvert.SerializeObject(search)}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Deletes a search on the server
        /// </summary>
        /// <param name="databaseId">Database ID that relates to the search</param>
        /// <param name="searchId">Search ID of the search to be deleted</param>
        public void DeleteSearch(int databaseId, int searchId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/searches/{searchId}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to delete search: {Response.Content}");
            }
        }
        /// <summary>
        /// Edit the properties of a archive
        /// </summary>
        /// <param name="databaseId">Database ID that relates to the archive</param>
        /// <param name="search">Archive to be updated</param>
        /// <returns></returns>
        public AdminSearch UpdateSearch(int databaseId, AdminSearch search)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/searches/{search.Id}", Method.PUT);
            Request.AddJsonBody(search);
            var Response = ApiClient.Execute<AdminSearch>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update search: {Response.Content} \n {JsonConvert.SerializeObject(search)}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests a list of searches from the server
        /// Can be used to request a list of all searches from a database, or individual search
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="searchId">Optional: Search ID</param>
        /// <returns><see cref="AdminSearch"/></returns>
        public List<AdminSearch> GetAdminSearches(int databaseId, int searchId = 0)
        {
            var Request = (searchId >= 1) ? new RestRequest($"api/admin/databases/{databaseId}/searches/{searchId}") : new RestRequest($"api/admin/databases/{databaseId}/searches");
            var Response = ApiClient.Execute<List<AdminSearch>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get searches: {Response.Content}");
            }
            return Response.Data;
        }
        #endregion
    }
}
