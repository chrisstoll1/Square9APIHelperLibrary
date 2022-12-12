﻿using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Square9APIHelperLibrary.DataTypes;

namespace Square9APIHelperLibrary.Square9APIComponents
{
    /// <summary>
    /// Fields are columns in <see cref="Archives"/> that store index information for <see cref="Documents"/>. They are also utilized in <see cref="Searches"/> to retrieve records
    /// </summary>
    public class Fields
    {
        private RestClient ApiClient;
        private string Default;
        private License License;
        internal Fields(RestClient apiClient, string defaultSql, License license)
        {
            ApiClient = apiClient;
            Default = defaultSql;
            License = license;
        }

        #region Methods
        /// <summary>
        /// Returns a list of fields in a given database
        /// </summary>
        /// <param name="databaseId">datbase ID</param>
        /// <param name="fieldId">Optional: Field ID to return</param>
        /// <returns><see cref="AdminField"/></returns>
        /// <exception cref="Exception"></exception>
        public List<AdminField> GetFields(int databaseId, int fieldId = 0)
        {
            var Request = (fieldId >= 1) ? new RestRequest($"api/admin/databases/{databaseId}/fields/{fieldId}") : new RestRequest($"api/admin/databases/{databaseId}/fields");
            var Response = ApiClient.Execute<List<AdminField>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get fields: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Create a new field on the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="field">New Field to be created</param>
        /// <returns><see cref="AdminField"/></returns>
        /// <exception cref="Exception"></exception>
        public AdminField CreateField(int databaseId, NewAdminField field)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/fields", Method.POST);
            Request.AddJsonBody(field);
            var Response = ApiClient.Execute<AdminField>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create field: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Update the passed in field on the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="field">Field to be updated</param>
        /// <returns><see cref="AdminField"/></returns>
        /// <exception cref="Exception"></exception>
        public AdminField UpdateField(int databaseId, AdminField field)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/fields/{field.Id}", Method.PUT);
            Request.AddJsonBody(field);
            var Response = ApiClient.Execute<AdminField>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update field: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Deletes a given field on the database
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="fieldId">Field ID</param>
        /// <exception cref="Exception"></exception>
        public void DeleteField(int databaseId, int fieldId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/fields/{fieldId}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable delete field: {Response.Content}");
            }
        }
        /// <summary>
        /// Requests a individual table field from the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="fieldId">Table Field ID</param>
        /// <returns><see cref="AdminTableField"/></returns>
        /// <exception cref="Exception"></exception>
        public AdminTableField GetTableField(int databaseId, int fieldId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/tablefields/{fieldId}");
            var Response = ApiClient.Execute<AdminTableField>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get table field: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests a list of all table fields in a given database
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <returns><see cref="AdminTableField"/></returns>
        /// <exception cref="Exception"></exception>
        public List<AdminTableField> GetTableFields(int databaseId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/tablefields");
            var Response = ApiClient.Execute<List<AdminTableField>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get table fields: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Creates a new table field in a given database on the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="field">Field ID</param>
        /// <returns><see cref="AdminTableField"/></returns>
        /// <exception cref="Exception"></exception>
        public AdminTableField CreateTableField(int databaseId, NewAdminTableField field)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/tablefields", Method.POST);
            Request.AddJsonBody(field);
            var Response = ApiClient.Execute<AdminTableField>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create table field: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Updates a table field on the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="field">Field ID</param>
        /// <returns><see cref="AdminTableField"/></returns>
        /// <exception cref="Exception"></exception>
        public AdminTableField UpdateTableField(int databaseId, AdminTableField field)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/tablefields/{field.Id}", Method.PUT);
            Request.AddJsonBody(field);
            var Response = ApiClient.Execute<AdminTableField>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update table field: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Deletes the specified table field from the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="fieldId">Field ID</param>
        /// <exception cref="Exception"></exception>
        public void DeleteTableField(int databaseId, int fieldId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/tablefields/{fieldId}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable delete table field: {Response.Content}");
            }
        }
        /// <summary>
        /// Requests a individual list from the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="listId">Field ID</param>
        /// <returns><see cref="AdminList"/></returns>
        /// <exception cref="Exception"></exception>
        public AdminList GetList(int databaseId, int listId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/lists/{listId}");
            var Response = ApiClient.Execute<AdminList>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get list: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Requests a list of lists from the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <returns><see cref="AdminList"/></returns>
        /// <exception cref="Exception"></exception>
        public List<AdminList> GetLists(int databaseId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/lists");
            var Response = ApiClient.Execute<List<AdminList>>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get lists: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Creates a new list on the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="list">The new <see cref="AdminList"/></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public AdminList CreateList(int databaseId, NewAdminList list)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/lists", Method.POST);
            Request.AddJsonBody(list);
            var Response = ApiClient.Execute<AdminList>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to create list: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Updates a list on the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="list">List object to update</param>
        /// <returns><see cref="AdminList"/></returns>
        /// <exception cref="Exception"></exception>
        public AdminList UpdateList(int databaseId, AdminList list)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/lists/{list.Id}", Method.PUT);
            Request.AddJsonBody(list);
            var Response = ApiClient.Execute<AdminList>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to update list: {Response.Content}");
            }
            return Response.Data;
        }
        /// <summary>
        /// Deletes a list on the server
        /// </summary>
        /// <param name="databaseId">Database ID</param>
        /// <param name="listId">List ID to be deleted</param>
        /// <exception cref="Exception"></exception>
        public void DeleteList(int databaseId, int listId)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/lists/{listId}", Method.DELETE);
            var Response = ApiClient.Execute(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable delete list: {Response.Content}");
            }
        }
        /// <summary>
        /// Loads an assembly lists values into the list object
        /// </summary>
        /// <param name="databaseId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public AdminList LoadAssemblyList(int databaseId, AdminList list)
        {
            var Request = new RestRequest($"api/admin/databases/{databaseId}/lists", Method.PUT);
            Request.AddJsonBody(list);
            var Response = ApiClient.Execute<AdminList>(Request);
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get assembly list: {Response.Content}");
            }
            return Response.Data;
        }
        #endregion
    }
}
