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
        private string Endpoint = "http://192.168.4.234/Square9API";
        private string Username = "SSAdministrator";
        private string Password = "Square9!";
        #endregion

        #region System
        [TestMethod]
        [TestCategory("System")]
        public void BasicAPIConnection()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Console.WriteLine(JsonConvert.SerializeObject(Connection.CreateLicense()));
            DatabaseList TestDatabaseList = Connection.Databases.GetDatabases(1);
            Connection.DeleteLicense();
            Connection.DeleteLicense();
            Assert.AreEqual(1, TestDatabaseList.Databases[0].Id);
        }
        [TestMethod]
        [TestCategory("System")]
        public void ReadDatabasesAndArchives()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Database> Databases = Connection.Databases.GetDatabases().Databases;
            Console.WriteLine($"{Databases[0].Id} : {Databases[0].Name}");
            List<Archive> Archives = Connection.Archives.GetArchives(Databases[0].Id).Archives;
            Console.WriteLine($"    {Archives[0].Id} : {Archives[0].Name}");
            List<Archive> SubArchives = Connection.Archives.GetArchives(Databases[0].Id, Archives[0].Id).Archives;
            Console.WriteLine($"        {SubArchives[0].Id} : {SubArchives[0].Name}");
            Connection.DeleteLicense();
            Assert.IsTrue(SubArchives[0].Name != null);
        }
        [TestMethod]
        [TestCategory("System")]
        public void CheckIfAdmin()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.Administration.IsAdmin());
            Assert.IsTrue(Connection.Administration.IsAdmin());
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("System")]
        public void GetUpdateEmailOptions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            EmailServer emailServer = Connection.Administration.GetEmailOptions(1);
            Console.WriteLine(JsonConvert.SerializeObject(emailServer));

            emailServer.Auth.User = Username;
            Console.WriteLine(JsonConvert.SerializeObject(Connection.Administration.UpdateEmailOptions(emailServer, 1)));

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("System")]
        public void GetCreateDeleteStamp()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            List<Stamp> stamps = Connection.Administration.GetStamps();
            Console.WriteLine(JsonConvert.SerializeObject(stamps));

            Stamp stamp = new Stamp();
            stamp.Text = "This is a test stamp 2";

            Stamp newStamp = Connection.Administration.CreateStamp(stamp);
            Console.WriteLine(JsonConvert.SerializeObject(newStamp));

            Connection.Administration.DeleteStamp(newStamp);

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("System")]
        public void GetRegistration()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            Registration registration = Connection.Administration.GetRegistration();
            Console.WriteLine(JsonConvert.SerializeObject(registration));

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("System")]
        public void GetReleaseLicenses()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            List<License> licenses = Connection.GetLicenses();
            Console.WriteLine(JsonConvert.SerializeObject(licenses));

            Connection.ReleaseLicense(licenses[0]);

            Connection.ReleaseAllLicenses(true);

            Connection.DeleteLicense();
        }
        #endregion

        #region Database
        [TestMethod]
        [TestCategory("Database")]
        public void CreateUpdateDeleteDatabase()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            AdminDatabase NewDatabase = new NewAdminDatabase("NewTestDatabase");
            AdminDatabase Database = Connection.Databases.CreateDatabase(NewDatabase);
            Console.WriteLine(Database.Name);
            Database.Name = "NewDatabaseModified";
            Database = Connection.Databases.UpdateDatabase(Database);
            Console.WriteLine(Database.Name);
            Console.WriteLine(Database.Id);
            Connection.Databases.DeleteDatabase(Database, true);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Database")]
        public void GetAdminDatabases()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminDatabase> Databases = Connection.Databases.GetAdminDatabases();
            Console.WriteLine(JsonConvert.SerializeObject(Databases[0]));
            Databases = Connection.Databases.GetAdminDatabases(Databases[0]);
            Console.WriteLine(Databases[0].Name);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Database")]
        public void RebuildDatabaseIndex()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminDatabase> Databases = Connection.Databases.GetAdminDatabases();
            Connection.Databases.RebuildDatabaseIndex(Databases[0]);
            Connection.DeleteLicense();
        }
        #endregion

        #region Archive
        [TestMethod]
        [TestCategory("Archive")]
        public void GetAdminArchives()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminArchive> Archives = Connection.Archives.GetAdminArchives(1);
            Console.WriteLine(Archives[0].Name);
            Console.WriteLine($"    {Connection.Archives.GetAdminArchives(1, 2)[0].Name}");
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Archive")]
        public void GetArchiveFields()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Field> Fields = Connection.Archives.GetArchiveFields(1,1);
            Field field = Fields[0];
            Console.WriteLine(Fields[2].Prop);
            FieldProp prop = new FieldProp(Fields[2].Prop);
            Console.WriteLine(JsonConvert.SerializeObject(prop));
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
            AdminArchive newArchive = Connection.Archives.CreateArchive(1, archive);
            newArchive.Name = "T1";
            AdminArchive updatedArchive = Connection.Archives.UpdateArchive(1, newArchive);
            Console.WriteLine(updatedArchive.Name);
            Connection.Archives.DeleteArchive(1, updatedArchive.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Archive")]
        public void RebuildContentIndex()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.Archives.RebuildArchiveContentIndex(1, 3, 10000));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Archive")]
        public void GetUpdateGlobalArchiveOptions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            GlobalArchiveOptions options = Connection.Archives.GetGlobalArchiveOptions(1);
            options.ShowAll = false;
            GlobalArchiveOptions updatedOptions = Connection.Archives.UpdateGlobalArchiveOptions(1, options);
            Console.WriteLine(updatedOptions.ShowAll);
            updatedOptions.ShowAll = true;
            Connection.Archives.UpdateGlobalArchiveOptions(1, updatedOptions);
            Console.WriteLine(Connection.Archives.GetGlobalArchiveOptions(1).ShowAll);
            Connection.DeleteLicense();
        }
        #endregion

        #region Search
        [TestMethod]
        [TestCategory("Search")]
        public void GetSearches()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Search> searches = Connection.Searches.GetSearches(1);
            List<Search> archiveSearches = Connection.Searches.GetSearches(1, archiveId: 1);
            List<Search> search = Connection.Searches.GetSearches(1, searchId: 6);
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
            Search search = Connection.Searches.GetSearches(1, searchId: 20)[0];
            search.Detail[0].Val = "test";
            search.Detail[1].Val = "10/29/2020";
            Result results = Connection.Searches.GetSearchResults(1, search);
            Console.WriteLine(JsonConvert.SerializeObject(results));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Search")]
        public void GetSearchCount()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Search> search = Connection.Searches.GetSearches(1, searchId: 20);
            Console.WriteLine(JsonConvert.SerializeObject(search));
            search[0].Detail[0].Val = "test";
            search[0].Detail[1].Val = "10/29/2020";
            Console.WriteLine(JsonConvert.SerializeObject(search));
            ArchiveCount results = Connection.Searches.GetSearchCount(1, search[0]);
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
            AdminSearch search = Connection.Searches.CreateSearch(1, newSearch);
            Console.WriteLine(JsonConvert.SerializeObject(search));
            search.Name = "Newest Test Search";
            AdminSearch updatedSearch = Connection.Searches.UpdateSearch(1, search);
            Console.WriteLine(updatedSearch.Name);
            Connection.Searches.DeleteSearch(1, search.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Search")]
        public void GetAdminSearches()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminSearch> searches = Connection.Searches.GetAdminSearches(1, 6);
            Console.WriteLine(searches[0].Name);
            Connection.DeleteLicense();
        }
        #endregion

        #region Inbox
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetInboxes()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            InboxList inboxes = Connection.Inboxes.GetInboxes();
            Console.WriteLine(JsonConvert.SerializeObject(inboxes));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetInbox()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Inbox inbox = Connection.Inboxes.GetInbox(1);
            Console.WriteLine(JsonConvert.SerializeObject(inbox));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetAdminInboxes()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminInbox> inboxes = Connection.Inboxes.GetAdminInboxes();
            Console.WriteLine(JsonConvert.SerializeObject(inboxes));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetAdminInboxSecurity()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Security> security = Connection.Inboxes.GetAdminInboxSecurity(1);
            Console.WriteLine(JsonConvert.SerializeObject(security));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void GetGlobalInboxOptions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            GlobalInboxOptions options = Connection.Inboxes.GetGlobalInboxOptions();
            Console.WriteLine(JsonConvert.SerializeObject(options));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Inbox")]
        public void UpdateGlobalInboxOptions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            GlobalInboxOptions options = Connection.Inboxes.GetGlobalInboxOptions();
            options.ShowAll = false;
            GlobalInboxOptions updatedOptions = Connection.Inboxes.UpdateGlobalInboxOptions(options);
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
            NewAdminInbox newInbox = new NewAdminInbox("BIGTEST");
            Console.WriteLine(JsonConvert.SerializeObject(newInbox));
            AdminInbox inbox = Connection.Inboxes.CreateInbox(newInbox);
            Console.WriteLine(JsonConvert.SerializeObject(inbox));
            Connection.Inboxes.DeleteInbox(inbox.Id);
            Connection.DeleteLicense();
        }
        #endregion

        #region Field
        [TestMethod]
        [TestCategory("Field")]
        public void GetFields()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminField> fields = Connection.Fields.GetFields(1);
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
            AdminField newField = Connection.Fields.CreateField(1, field);
            Console.WriteLine(JsonConvert.SerializeObject(newField));
            AdminField retrievedField = Connection.Fields.GetFields(1, newField.Id)[0];
            Console.WriteLine(JsonConvert.SerializeObject(retrievedField));
            retrievedField.Name = "Tester Field Updated";
            AdminField updatedField = Connection.Fields.UpdateField(1, retrievedField);
            Console.WriteLine(JsonConvert.SerializeObject(updatedField));
            Connection.Fields.DeleteField(1, updatedField.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Field")]
        public void GetTableFields()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminTableField> fields = Connection.Fields.GetTableFields(1);
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
            AdminTableField newTableField = Connection.Fields.CreateTableField(1, tableField);
            Console.WriteLine(JsonConvert.SerializeObject(newTableField));
            AdminTableField retreivedTableField = Connection.Fields.GetTableField(1, newTableField.Id);
            Console.WriteLine(JsonConvert.SerializeObject(retreivedTableField));
            retreivedTableField.Name = "Tester Table Field Updated";
            AdminTableField updatedTableField = Connection.Fields.UpdateTableField(1, retreivedTableField);
            Console.WriteLine(JsonConvert.SerializeObject(updatedTableField));
            Connection.Fields.DeleteTableField(1, updatedTableField.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Field")]
        public void GetLists()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminList> fields = Connection.Fields.GetLists(1);
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
            AdminList newList = Connection.Fields.CreateList(1, list);
            Console.WriteLine(JsonConvert.SerializeObject(newList));
            AdminList retrievedList = Connection.Fields.GetList(1, newList.Id);
            retrievedList.Name = "Tester List Updated";
            Console.WriteLine(JsonConvert.SerializeObject(retrievedList));
            AdminList updatedList = Connection.Fields.UpdateList(1, retrievedList);
            Console.WriteLine(JsonConvert.SerializeObject(updatedList));
            Connection.Fields.DeleteList(1, updatedList.Id);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Field")]
        public void LoadAssemblyList()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            AdminList list = Connection.Fields.LoadAssemblyList(1, Connection.Fields.GetList(1, 19));
            Console.WriteLine(JsonConvert.SerializeObject(list.Values));
            Connection.DeleteLicense();
        }
        #endregion

        #region Document
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocument()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Result DocResult = Connection.Documents.GetArchiveDocument(2057, 1, 1);
            Console.WriteLine(JsonConvert.SerializeObject(DocResult));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocumentMetaData()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.Searches.GetSearches(2057, searchId: 1)[0];
            Doc document = Connection.Searches.GetSearchResults(2057, search).Docs[0];
            Console.WriteLine(JsonConvert.SerializeObject(document));
            Result result = Connection.Documents.GetArchiveDocumentMetaData(2057, 1, document);
            Console.WriteLine(JsonConvert.SerializeObject(result));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocumentFile()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Result DocResult = Connection.Documents.GetArchiveDocument(2, 1, 1);
            Doc document = DocResult.Docs[0];
            var fileName = Connection.Documents.GetArchiveDocumentFile(2, 1, document, "C:\\test\\testfile.pdf");
            Console.WriteLine(fileName);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocumentThumbnail()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.Searches.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.Searches.GetSearchResults(1, search).Docs[0];
            var fileName = Connection.Documents.GetArchiveDocumentThumbnail(1, 1, document, "C:\\test\\", 1000, 1000);
            Console.WriteLine(fileName);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void UpdateDocumentIndexData()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.Searches.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.Searches.GetSearchResults(1, search).Docs[0];
            Console.WriteLine(JsonConvert.SerializeObject(document));
            Random random = new Random();
            int num = random.Next(1000);
            document.Fields[0].Val = $"{num}";
            Console.WriteLine(JsonConvert.SerializeObject(document));
            Connection.Documents.UpdateDocumentIndexData(1, 1, document);
            Doc updatedDoc = Connection.Searches.GetSearchResults(1, search).Docs[0];
            Console.WriteLine(JsonConvert.SerializeObject(updatedDoc));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void UploadIndexDeleteDocument()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            UploadedFiles files = Connection.Documents.UploadDocument("C:\\test\\testuploadDoc.pdf");
            Console.WriteLine(JsonConvert.SerializeObject(files));
            NewFile newFile = new NewFile();
            newFile.Files = files.Files;
            newFile.Fields.Add(new FileField("2", "Test Last Name 1234"));
            newFile.Fields.Add(new FileField("3", "11/16/2021"));
            Console.WriteLine(JsonConvert.SerializeObject(newFile));
            Connection.Documents.ImportArchiveDocument(1, 1, newFile);
            Search search = Connection.Searches.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.Searches.GetSearchResults(1, search).Docs[0];
            Connection.Documents.DeleteArchiveDocument(1, 1, document);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void ExportDocuments()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.Searches.GetSearches(1, searchId: 20)[0];
            List<FileExport> files = new List<FileExport>();
            Console.WriteLine(JsonConvert.SerializeObject(Connection.Searches.GetSearchResults(1, search)));
            files.Add(new FileExport(1, Connection.Searches.GetSearchResults(1, search).Docs[0]));
            files.Add(new FileExport(1, Connection.Searches.GetSearchResults(1, search).Docs[1]));
            Console.WriteLine(JsonConvert.SerializeObject(files));
            string exportedFiles = Connection.Documents.ExportDocument(1, 2, files, "C:\\test\\");
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
            Search search = Connection.Searches.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.Searches.GetSearchResults(1, search).Docs[0];
            int docID = Connection.Documents.TransferArchiveDocument(1, 1, 2, document);
            Console.WriteLine(docID);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocumentRevisions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Search search = Connection.Searches.GetSearches(1, searchId: 1082)[0];
            Doc document = Connection.Searches.GetSearchResults(1, search).Docs[0];
            Revision revision = Connection.Documents.GetDocumentRevisions(1, 52, document)[0];
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
            Search search = Connection.Searches.GetSearches(1, searchId: 1083)[0];
            Doc document = Connection.Searches.GetSearchResults(1, search).Docs[0];
            Console.WriteLine(JsonConvert.SerializeObject(document));
            Queue documentQueue = Connection.Documents.GetDocumentQueue(1, 53, document);
            Console.WriteLine(JsonConvert.SerializeObject(documentQueue));
            Connection.Documents.FireDocumentQueueAction(1, 53, document, documentQueue.Actions[1]);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void UploadImportIndexInboxDocument()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            UploadedFiles files = Connection.Documents.UploadDocument("C:\\test\\testuploadDoc.pdf");
            Connection.Documents.ImportInboxDocument(1, files.Files[0]);

            Inbox inbox = Connection.Inboxes.GetInbox(1);
            Square9APIHelperLibrary.DataTypes.File file = inbox.Files[0];
            Console.WriteLine(JsonConvert.SerializeObject(file));

            List<FileField> fields = new List<FileField>();
            fields.Add(new FileField("2", "Inbox Index Test"));

            Connection.Documents.IndexInboxDocument(1, 1, 1, file, fields);

            Search search = Connection.Searches.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.Searches.GetSearchResults(1, search).Docs[0];
            Connection.Documents.DeleteArchiveDocument(1, 1, document);

            Connection.Documents.DeleteInboxDocument(1, file);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetInboxDocumentFile()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            Inbox inbox = Connection.Inboxes.GetInbox(1);
            Square9APIHelperLibrary.DataTypes.File file = inbox.Files[0];
            Console.WriteLine(JsonConvert.SerializeObject(file));

            Console.WriteLine(Connection.Documents.GetInboxDocumentFile(1, file, "C:\\test\\inbox\\"));

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void TransferInboxDocument()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            Inbox inbox = Connection.Inboxes.GetInbox(1);
            Square9APIHelperLibrary.DataTypes.File file = inbox.Files[0];
            Console.WriteLine(JsonConvert.SerializeObject(file));

            Console.WriteLine(Connection.Documents.TransferInboxDocument(1, 2, file));

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void TableFieldData()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            TableField tableField = Connection.Documents.GetTableFieldData(2057, 1, 1, 6);
            tableField.Data[2][1] = "TEST VALUE";
            Connection.Documents.UpdateTableFieldData(2057, 1, 1, tableField);

            Connection.DeleteLicense();
        }
        #endregion

        #region Administration
        [TestMethod]
        [TestCategory("Administration")]
        public void GetSecuredAndUnsecuredUsersAndGroups()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<SecuredGroup> securedGroups = Connection.Administration.GetSecuredUsersAndGroups();
            Console.WriteLine(JsonConvert.SerializeObject(securedGroups));
            List<UnsecuredGroup> unsecuredGroups = Connection.Administration.GetUnsecuredUsersAndGroups();
            Console.WriteLine(JsonConvert.SerializeObject(unsecuredGroups));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void GetTreeStructure()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<SecurityNode> securityNodes = Connection.Administration.GetTreeStructure();
            Console.WriteLine(JsonConvert.SerializeObject(securityNodes));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void GetUserArchivePermissions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.Administration.GetUserArchivePermissions(2, 1, "test").Level);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void GetUserInboxPermissions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.Administration.GetUserInboxPermissions(1, "test").Level);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void GetUserSearchProperties()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<SearchProperties> userSearchSecurity = Connection.Administration.GetUserSearchProperties(1, "SSAdministrator");
            Console.WriteLine(JsonConvert.SerializeObject(userSearchSecurity));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void SetArchiveSecurity()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            //Construct archiveSecurity object
            ArchiveSecurity archiveSecurity = new ArchiveSecurity();

            //Add Users
            SecuredGroup securedGroup = Connection.Administration.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
            User user = new User();
            user.ConvertSecuredGroup(securedGroup);
            archiveSecurity.Users.Add(user);

            //Add Targets
            Target target = new Target();
            target.Id = 1; //archive ID
            target.Database = 2; //database ID
            archiveSecurity.Targets.Add(target);

            //Add Permissions
            Console.WriteLine("Existing Database Permissions:");
            ArchivePermission existingPermissions = Connection.Administration.GetUserArchivePermissions(2, 1, "test");
            Console.WriteLine(JsonConvert.SerializeObject(existingPermissions));

            Console.WriteLine("SelectAll, print output");
            existingPermissions.SelectAll();
            Console.WriteLine(JsonConvert.SerializeObject(existingPermissions));

            Console.WriteLine("ClearAll. print output");
            existingPermissions.ClearAll();
            Console.WriteLine(JsonConvert.SerializeObject(existingPermissions));

            Console.WriteLine("string input test");
            ArchivePermission archivePermission = new ArchivePermission("11101010101010101010");
            Console.WriteLine(JsonConvert.SerializeObject(archivePermission));

            ArchivePermission permission = new ArchivePermission(existingPermissions);
            permission.View = true;
            permission.Add = true;
            permission.Delete = true;
            permission.Print = true;
            permission.FullAPIAccess = true;
            archiveSecurity.Permissions = permission;

            Console.WriteLine("Full ArchiveSecurity Object");
            Console.WriteLine(JsonConvert.SerializeObject(archiveSecurity));
            Connection.Administration.SetArchiveSecurity(archiveSecurity);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void SetInboxSecurity()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            //Construct inboxSecurity object
            InboxSecurity inboxSecurity = new InboxSecurity();

            //Add Users
            SecuredGroup securedGroup = Connection.Administration.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
            User user = new User();
            user.ConvertSecuredGroup(securedGroup);
            inboxSecurity.Users.Add(user);

            //Add Targets
            Target target = new Target();
            target.Id = 1; //inbox ID
            inboxSecurity.Targets.Add(target);

            //Add Permissions
            InboxPermission permission = new InboxPermission();
            permission.View = true;
            permission.Add = true;
            permission.Delete = true;
            permission.Print = true;
            //permission.SelectAll();
            permission.CalculatePermissionLevel();
            inboxSecurity.Permissions = permission;

            Console.WriteLine(JsonConvert.SerializeObject(inboxSecurity));
            Connection.Administration.SetInboxSecurity(inboxSecurity);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void SetDatabaseSecurity()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            //Construct Database Security Object
            DatabaseSecurity databaseSecurity = new DatabaseSecurity();

            //Add Users
            SecuredGroup securedGroup = Connection.Administration.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
            User user = new User();
            user.ConvertSecuredGroup(securedGroup);
            databaseSecurity.Users.Add(user);

            //Add Target
            Target databaseTarget = new Target();
            databaseTarget.Id = 1;
            databaseSecurity.Targets.Add(databaseTarget);

            //Add Permissions
            DatabasePermission databasePermission = new DatabasePermission();
            databasePermission.Type = 0;
            databasePermission.License = 1;
            databaseSecurity.Permissions = databasePermission;

            Connection.Administration.SetDatabaseSecurity(databaseSecurity);

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void SetSearchSecurity()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            //Construct Search Security object
            SearchSecurity searchSecurity = new SearchSecurity();

            //Add Users
            SecuredGroup securedGroup = Connection.Administration.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
            User user = new User();
            user.ConvertSecuredGroup(securedGroup);
            searchSecurity.Users.Add(user);

            //Add Target
            Target searchSecurityTarget = new Target();
            searchSecurityTarget.Id = 1;
            searchSecurityTarget.Database = 2;
            searchSecurity.Targets.Add(searchSecurityTarget);

            //Add Permissions 
            SearchPermission searchPermission = new SearchPermission();
            searchPermission.View = true;
            searchSecurity.Permissions = searchPermission;

            Connection.Administration.SetSearchSecurity(searchSecurity);

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void SetSearchProperties()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            //Construct Search Security object
            SearchSecurity searchSecurity = new SearchSecurity();

            //Add Users
            SecuredGroup securedGroup = Connection.Administration.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
            User user = new User();
            user.ConvertSecuredGroup(securedGroup);
            searchSecurity.Users.Add(user);

            //Add Target
            Target searchSecurityTarget = new Target();
            searchSecurityTarget.Id = 1;
            searchSecurityTarget.Database = 2;
            searchSecurity.Targets.Add(searchSecurityTarget);

            //Add Permissions 
            SearchPermission searchPermission = new SearchPermission();
            searchPermission.Type = 8; //4=QueueSearch, 8=DefaultSearch, 16=DirectSearch
            searchSecurity.Permissions = searchPermission;

            Connection.Administration.SetSearchProperties(searchSecurity);

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void CreateUpdateDeleteUser()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            //Construct User Object
            User newUser = new User();
            newUser.Name = "NewTestUser";
            newUser.Password = Password;

            //Create new user
            Connection.Administration.CreateUser(newUser);

            //Get the new user
            UnsecuredGroup unsecuredGroup = Connection.Administration.GetUnsecuredUsersAndGroups().Find(x => x.Name == "NewTestUser");
            User user = new User();
            user.ConvertUnsecuredGroup(unsecuredGroup);

            Console.WriteLine(JsonConvert.SerializeObject(user));

            //Update new user
            user.Password = "NewPassword123!@#";
            Connection.Administration.UpdateUser(user);

            Console.WriteLine(JsonConvert.SerializeObject(user));

            //Delete the user
            Connection.Administration.DeleteUser(user);

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void CreateGetUpdateDeleteGroup()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            //Construct Group Object
            Group newGroup = new Group();
            newGroup.Name = "NewTestGroup";

            //Create new group
            Connection.Administration.CreateGroup(newGroup);

            //Get the new group
            Group group = Connection.Administration.GetGroups().Find(x => x.Name == "NewTestGroup");

            //Get some users
            List<UnsecuredGroup> unsecuredGroups = Connection.Administration.GetUnsecuredUsersAndGroups().FindAll(x => x.Type == 2); //Type 2 is for S9 users

            Console.WriteLine(JsonConvert.SerializeObject(group));
            Console.WriteLine(JsonConvert.SerializeObject(unsecuredGroups));

            //Add users to new group
            group.Users.Add(unsecuredGroups[0].Name);
            group.Users.Add(unsecuredGroups[2].Name);
            group.Users.Add(unsecuredGroups[4].Name);
            group.Users.Add(unsecuredGroups[6].Name);

            //Update group
            Connection.Administration.UpdateGroup(group);

            //Get the updated group
            Group updatedGroup = Connection.Administration.GetGroups().Find(x => x.Name == "NewTestGroup");

            Console.WriteLine(JsonConvert.SerializeObject(updatedGroup));

            //Delete group
            Connection.Administration.DeleteGroup(updatedGroup);

            Connection.DeleteLicense();
        }

        [TestMethod]
        [TestCategory("Administration")]

        public void CreateGetUpdateDeleteAdvancedLink()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            NewAdvancedLink newAdvancedLink = new NewAdvancedLink("Test Advanced Link", 25, "google.com");

            Console.WriteLine(JsonConvert.SerializeObject(newAdvancedLink));

            Connection.Fields.CreateAdvancedLink(1, newAdvancedLink);

            AdvancedLink advancedLink = Connection.Fields.GetAdvancedLinks(1)[0];

            advancedLink.Name = "Testing Advanced Link Name 2";

            AdvancedLink updatedAdvancedLink = Connection.Fields.UpdateAdvancedLink(1, advancedLink);

            Assert.AreEqual(updatedAdvancedLink.Name, advancedLink.Name);

            Connection.Fields.DeleteAdvancedLink(1, updatedAdvancedLink);

            Connection.DeleteLicense();
        }
        #endregion
    }
}
