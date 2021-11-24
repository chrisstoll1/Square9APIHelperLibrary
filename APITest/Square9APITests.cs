using Microsoft.VisualStudio.TestTools.UnitTesting;
using Square9APIHelperLibrary;
using Square9APIHelperLibrary.DataTypes;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace APITest
{
    [TestClass]
    public class Square9APITests
    {
        #region Variables
        private string Endpoint = "http://10.0.0.220/Square9API";
        private string Username = "SSAdministrator";
        private string Password = "Square9!";
        #endregion

        [TestMethod]
        [TestCategory("Basic")]
        public void BasicAPIConnection()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            DatabaseList TestDatabaseList = Connection.GetDatabases(1);
            Connection.DeleteLicense();
            Assert.AreEqual(1, TestDatabaseList.Databases[0].Id);
        }
        [TestMethod]
        [TestCategory("Basic")]
        public void ReadDatabasesAndArchives()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Database> Databases = Connection.GetDatabases().Databases;
            Console.WriteLine($"{Databases[0].Id} : {Databases[0].Name}");
            List<Archive> Archives = Connection.GetArchives(Databases[0].Id).Archives;
            Console.WriteLine($"    {Archives[0].Id} : {Archives[0].Name}");
            List<Archive> SubArchives = Connection.GetArchives(Databases[0].Id, Archives[0].Id).Archives;
            Console.WriteLine($"        {SubArchives[0].Id} : {SubArchives[0].Name}");
            Connection.DeleteLicense();
            Assert.IsTrue(SubArchives[0].Name != null);
        }
        [TestMethod]
        [TestCategory("Database")]
        public void CreateUpdateDeleteDatabase()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            AdminDatabase NewDatabase = new NewAdminDatabase("NewDatabase");
            AdminDatabase Database = Connection.CreateDatabase(NewDatabase);
            Console.WriteLine(Database.Name);
            Database.Name = "NewDatabaseModified";
            Database = Connection.UpdateDatabase(Database);
            Console.WriteLine(Database.Name);
            Console.WriteLine(Database.Id);
            Connection.DeleteDatabase(Database.Id, true);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Basic")]
        public void CheckIfAdmin()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.IsAdmin());
            Assert.IsTrue(Connection.IsAdmin());
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Database")]
        public void GetAdminDatabases()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminDatabase> Databases = Connection.GetAdminDatabases();
            Console.WriteLine(Databases[0].Id);
            Databases = Connection.GetAdminDatabases(Databases[0].Id);
            Console.WriteLine(Databases[0].Name);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Database")]
        public void RebuildDatabaseIndex()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Connection.RebuildDatabaseIndex(1);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Archive")]
        public void GetAdminArchives()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminArchive> Archives = Connection.GetAdminArchives(1);
            Console.WriteLine(Archives[0].Name);
            Console.WriteLine($"    {Connection.GetAdminArchives(1, 2)[0].Name}");
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Archive")]
        public void GetArchiveFields()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Field> Fields = Connection.GetArchiveFields(1,1);
            Console.WriteLine(Fields[0].Name);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Archive")]
        public void CreateUpdateDeleteArchive()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            NewAdminArchive archive = new NewAdminArchive("T2");
            archive.Fields = new List<int>() { 3 };
            archive.Parent = 3;
            AdminArchive newArchive = Connection.CreateArchive(1, archive);
            newArchive.Name = "T1";
            AdminArchive updatedArchive = Connection.UpdateArchive(1, newArchive);
            Console.WriteLine(updatedArchive.Name);
            Connection.DeleteArchive(1, updatedArchive.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Database")]
        public void GetUpdateGlobalArchiveOptions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            GlobalArchiveOptions options = Connection.GetGlobalArchiveOptions(1);
            options.ShowAll = false;
            GlobalArchiveOptions updatedOptions = Connection.UpdateGlobalArchiveOptions(1, options);
            Console.WriteLine(updatedOptions.ShowAll);
            updatedOptions.ShowAll = true;
            Connection.UpdateGlobalArchiveOptions(1, updatedOptions);
            Console.WriteLine(Connection.GetGlobalArchiveOptions(1).ShowAll);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Archive")]
        public void RebuildContentIndex()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.RebuildArchiveContentIndex(1, 3, 10000));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Search")]
        public void GetSearches()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Search> searches = Connection.GetSearches(1);
            List<Search> archiveSearches = Connection.GetSearches(1, archiveId: 1);
            List<Search> search = Connection.GetSearches(1, searchId: 6);
            Console.WriteLine(JsonConvert.SerializeObject(archiveSearches));
            Console.WriteLine(searches[0].Name);
            Console.WriteLine(archiveSearches[0].Name);
            Console.WriteLine(search[0].Name);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Search")]
        public void GetSearchResults()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            search.Detail[0].Val = "test";
            search.Detail[1].Val = "10/29/2020";
            Result results = Connection.GetSearchResults(1, search);
            Console.WriteLine(JsonConvert.SerializeObject(results));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Search")]
        public void GetSearchCount()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Search> search = Connection.GetSearches(1, searchId: 20);
            Console.WriteLine(JsonConvert.SerializeObject(search));
            search[0].Detail[0].Val = "test";
            search[0].Detail[1].Val = "10/29/2020";
            Console.WriteLine(JsonConvert.SerializeObject(search));
            ArchiveCount results = Connection.GetSearchCount(1, search[0]);
            Console.WriteLine(JsonConvert.SerializeObject(results));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Search")]
        public void CreateUpdateDeleteSearch()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            NewAdminSearch newSearch = new NewAdminSearch("New Test Search");
            newSearch.AddParameter(3, "contains", "Date:");
            newSearch.Parent = 3;
            newSearch.Archives.Add(3);
            AdminSearch search = Connection.CreateSearch(1, newSearch);
            Console.WriteLine(JsonConvert.SerializeObject(search));
            search.Name = "Newest Test Search";
            AdminSearch updatedSearch = Connection.UpdateSearch(1, search);
            Console.WriteLine(updatedSearch.Name);
            Connection.DeleteSearch(1, search.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Search")]
        public void GetAdminSearches()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminSearch> searches = Connection.GetAdminSearches(1, 6);
            Console.WriteLine(searches[0].Name);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetInboxes()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            InboxList inboxes = Connection.GetInboxes();
            Console.WriteLine(JsonConvert.SerializeObject(inboxes));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetInbox()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Inbox inbox = Connection.GetInbox(1);
            Console.WriteLine(JsonConvert.SerializeObject(inbox));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetAdminInboxes()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminInbox> inboxes = Connection.GetAdminInboxes();
            Console.WriteLine(JsonConvert.SerializeObject(inboxes));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetAdminInboxSecurity()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Security> security = Connection.GetAdminInboxSecurity(1);
            Console.WriteLine(JsonConvert.SerializeObject(security));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetGlobalInboxOptions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            GlobalInboxOptions options = Connection.GetGlobalInboxOptions();
            Console.WriteLine(JsonConvert.SerializeObject(options));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void UpdateGlobalInboxOptions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            GlobalInboxOptions options = Connection.GetGlobalInboxOptions();
            options.ShowAll = false;
            GlobalInboxOptions updatedOptions = Connection.UpdateGlobalInboxOptions(options);
            Console.WriteLine(JsonConvert.SerializeObject(options));
            Console.WriteLine(JsonConvert.SerializeObject(updatedOptions));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void CreateDeleteInbox()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            NewAdminInbox newInbox = new NewAdminInbox();
            newInbox.Name = "BIGTEST";
            Console.WriteLine(JsonConvert.SerializeObject(newInbox));
            AdminInbox inbox = Connection.CreateInbox(newInbox);
            Console.WriteLine(JsonConvert.SerializeObject(inbox));
            Connection.DeleteInbox(inbox.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Field")]
        public void GetFields()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminField> fields = Connection.GetFields(1);
            Console.WriteLine(JsonConvert.SerializeObject(fields));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Field")]
        public void CreateGetUpdateDeleteField()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            NewAdminField field = new NewAdminField();
            field.Name = "Tester Field";
            field.Type = "character";
            field.Length = 50;
            Console.WriteLine(JsonConvert.SerializeObject(field));
            AdminField newField = Connection.CreateField(1, field);
            Console.WriteLine(JsonConvert.SerializeObject(newField));
            AdminField retrievedField = Connection.GetFields(1, newField.Id)[0];
            Console.WriteLine(JsonConvert.SerializeObject(retrievedField));
            retrievedField.Name = "Tester Field Updated";
            AdminField updatedField = Connection.UpdateField(1, retrievedField);
            Console.WriteLine(JsonConvert.SerializeObject(updatedField));
            Connection.DeleteField(1, updatedField.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Field")]
        public void GetTableFields()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminTableField> fields = Connection.GetTableFields(1);
            Console.WriteLine(JsonConvert.SerializeObject(fields));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Field")]
        public void CreateGetUpdateDeleteTableField()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            NewAdminTableField tableField = new NewAdminTableField();
            tableField.Name = "Tester Table Field";
            tableField.Fields.Add(2);
            Console.WriteLine(JsonConvert.SerializeObject(tableField));
            AdminTableField newTableField = Connection.CreateTableField(1, tableField);
            Console.WriteLine(JsonConvert.SerializeObject(newTableField));
            AdminTableField retreivedTableField = Connection.GetTableField(1, newTableField.Id);
            Console.WriteLine(JsonConvert.SerializeObject(retreivedTableField));
            retreivedTableField.Name = "Tester Table Field Updated";
            AdminTableField updatedTableField = Connection.UpdateTableField(1, retreivedTableField);
            Console.WriteLine(JsonConvert.SerializeObject(updatedTableField));
            Connection.DeleteTableField(1, updatedTableField.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Field")]
        public void GetLists()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminList> fields = Connection.GetLists(1);
            Console.WriteLine(JsonConvert.SerializeObject(fields));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategoryAttribute("Field")]
        public void CreateGetUpdateDeleteList()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            NewAdminList list = new NewAdminList();
            list.Name = "Tester List";
            list.Values.Add("test value 1");
            Console.WriteLine(JsonConvert.SerializeObject(list));
            AdminList newList = Connection.CreateList(1, list);
            Console.WriteLine(JsonConvert.SerializeObject(newList));
            AdminList retrievedList = Connection.GetList(1, newList.Id);
            retrievedList.Name = "Tester List Updated";
            Console.WriteLine(JsonConvert.SerializeObject(retrievedList));
            AdminList updatedList = Connection.UpdateList(1, retrievedList);
            Console.WriteLine(JsonConvert.SerializeObject(updatedList));
            Connection.DeleteList(1, updatedList.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Field")]
        public void LoadAssemblyList()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            AdminList list = Connection.LoadAssemblyList(1, Connection.GetList(1, 19));
            Console.WriteLine(JsonConvert.SerializeObject(list.Values));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocumentMetaData()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            Console.WriteLine(JsonConvert.SerializeObject(document));
            Result result = Connection.GetDocumentMetaData(1, 1, document);
            Console.WriteLine(JsonConvert.SerializeObject(result));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocumentFile()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            var fileName = Connection.GetDocumentFile(1, 1, document, "C:\\test\\");
            Console.WriteLine(fileName);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocumentThumbnail()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            var fileName = Connection.GetDocumentThumbnail(1, 1, document, "C:\\test\\", 1000, 1000);
            Console.WriteLine(fileName);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void UpdateDocumentIndexData()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            Console.WriteLine(JsonConvert.SerializeObject(document));
            Random random = new Random();
            int num = random.Next(1000);
            document.Fields[0].Val = $"{num}";
            Console.WriteLine(JsonConvert.SerializeObject(document));
            Connection.UpdateDocumentIndexData(1, 1, document);
            Doc updatedDoc = Connection.GetSearchResults(1, search).Docs[0];
            Console.WriteLine(JsonConvert.SerializeObject(updatedDoc));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void UploadIndexDeleteDocument()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            UploadedFiles files = Connection.UploadDocument("C:\\test\\testuploadDoc.pdf");
            Console.WriteLine(JsonConvert.SerializeObject(files));
            NewFile newFile = new NewFile();
            newFile.Files = files.Files;
            newFile.Fields.Add(new FileField("2", "Test Last Name 1234"));
            newFile.Fields.Add(new FileField("3", "11/16/2021"));
            Console.WriteLine(JsonConvert.SerializeObject(newFile));
            Connection.ImportDocument(1, 1, newFile);
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            Connection.DeleteDocument(1, 1, document);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void ExportDocuments()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            List<FileExport> files = new List<FileExport>();
            Console.WriteLine(JsonConvert.SerializeObject(Connection.GetSearchResults(1, search)));
            files.Add(new FileExport(1, Connection.GetSearchResults(1, search).Docs[0]));
            files.Add(new FileExport(1, Connection.GetSearchResults(1, search).Docs[1]));
            Console.WriteLine(JsonConvert.SerializeObject(files));
            string exportedFiles = Connection.ExportDocument(1, 2, files, "C:\\test\\");
            Console.WriteLine(exportedFiles);
            Connection.DeleteLicense();
            //throw new Exception("This method has currently not been tested fully (have observed empty zip files being returned)");
        }
        [TestMethod]
        [TestCategory("Document")]
        public void TransferDocument()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            int docID = Connection.TransferDocument(1, 1, 2, document);
            Console.WriteLine(docID);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocumentRevisions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.GetSearches(1, searchId: 1082)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            Revision revision = Connection.GetDocumentRevisions(1, 52, document)[0];
            Console.WriteLine(JsonConvert.SerializeObject(revision));
            Doc revisionDoc = revision.ReturnDoc();
            Console.WriteLine(JsonConvert.SerializeObject(revisionDoc));
            //Connection.GetDocumentMetaData(1, 15, revisionDoc);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetTriggerDocumentQueue()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.GetSearches(1, searchId: 1083)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            Console.WriteLine(JsonConvert.SerializeObject(document));
            Queue documentQueue = Connection.GetDocumentQueue(1, 53, document);
            Console.WriteLine(JsonConvert.SerializeObject(documentQueue));
            Connection.FireDocumentQueueAction(1, 53, document, documentQueue.Actions[1]);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void GetSecuredAndUnsecuredUsersAndGroups()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<SecuredGroup> securedGroups = Connection.GetSecuredUsersAndGroups();
            Console.WriteLine(JsonConvert.SerializeObject(securedGroups));
            List<SecuredGroup> unsecuredGroups = Connection.GetUnsecuredUsersAndGroups();
            Console.WriteLine(JsonConvert.SerializeObject(unsecuredGroups));
            Connection.DeleteLicense();
        }
    }
}
