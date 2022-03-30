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

        #region System
        [TestMethod]
        [TestCategory("System")]
        public void BasicAPIConnection()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Console.WriteLine(JsonConvert.SerializeObject(Connection.CreateLicense()));
            DatabaseList TestDatabaseList = Connection.GetDatabases(1);
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
        [TestCategory("System")]
        public void CheckIfAdmin()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.IsAdmin());
            Assert.IsTrue(Connection.IsAdmin());
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("System")]
        public void GetUpdateEmailOptions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            EmailServer emailServer = Connection.GetEmailOptions(1);
            Console.WriteLine(JsonConvert.SerializeObject(emailServer));

            emailServer.Auth.User = Username;
            Console.WriteLine(JsonConvert.SerializeObject(Connection.UpdateEmailOptions(1, emailServer)));

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("System")]
        public void GetCreateDeleteStamp()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            List<Stamp> stamps = Connection.GetStamps();
            Console.WriteLine(JsonConvert.SerializeObject(stamps));

            Stamp stamp = new Stamp();
            stamp.Text = "This is a test stamp 2";

            Stamp newStamp = Connection.CreateStamp(stamp);
            Console.WriteLine(JsonConvert.SerializeObject(newStamp));

            Connection.DeleteStamp(newStamp);

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("System")]
        public void GetRegistration()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            Registration registration = Connection.GetRegistration();
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
        [TestCategory("Database")]
        public void GetAdminDatabases()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<AdminDatabase> Databases = Connection.GetAdminDatabases();
            Console.WriteLine(JsonConvert.SerializeObject(Databases[0]));
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
        #endregion

        #region Archive
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
            AdminArchive newArchive = Connection.CreateArchive(1, archive);
            newArchive.Name = "T1";
            AdminArchive updatedArchive = Connection.UpdateArchive(1, newArchive);
            Console.WriteLine(updatedArchive.Name);
            Connection.DeleteArchive(1, updatedArchive.Id);
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
        [TestCategory("Archive")]
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
        #endregion

        #region Search
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
        #endregion

        #region Inbox
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
            NewAdminInbox newInbox = new NewAdminInbox("BIGTEST");
            Console.WriteLine(JsonConvert.SerializeObject(newInbox));
            AdminInbox inbox = Connection.CreateInbox(newInbox);
            Console.WriteLine(JsonConvert.SerializeObject(inbox));
            Connection.DeleteInbox(inbox.Id);
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
        #endregion

        #region Document
        [TestMethod]
        [TestCategory("Document")]
        public void GetDocument()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Result DocResult = Connection.GetArchiveDocument(1, 14, 1);
            Console.WriteLine(JsonConvert.SerializeObject(DocResult));
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
            Result result = Connection.GetArchiveDocumentMetaData(1, 1, document);
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
            var fileName = Connection.GetArchiveDocumentFile(1, 1, document, "C:\\test\\");
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
            var fileName = Connection.GetArchiveDocumentThumbnail(1, 1, document, "C:\\test\\", 1000, 1000);
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
            Connection.ImportArchiveDocument(1, 1, newFile);
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            Connection.DeleteArchiveDocument(1, 1, document);
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
            int docID = Connection.TransferArchiveDocument(1, 1, 2, document);
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
        [TestCategory("Document")]
        public void UploadImportIndexInboxDocument()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            UploadedFiles files = Connection.UploadDocument("C:\\test\\testuploadDoc.pdf");
            Connection.ImportInboxDocument(1, files.Files[0]);

            Inbox inbox = Connection.GetInbox(1);
            Square9APIHelperLibrary.DataTypes.File file = inbox.Files[0];
            Console.WriteLine(JsonConvert.SerializeObject(file));

            List<FileField> fields = new List<FileField>();
            fields.Add(new FileField("2", "Inbox Index Test"));

            Connection.IndexInboxDocument(1, 1, 1, file, fields);

            Search search = Connection.GetSearches(1, searchId: 20)[0];
            Doc document = Connection.GetSearchResults(1, search).Docs[0];
            Connection.DeleteArchiveDocument(1, 1, document);

            Connection.DeleteInboxDocument(1, file);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void GetInboxDocumentFile()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            Inbox inbox = Connection.GetInbox(1);
            Square9APIHelperLibrary.DataTypes.File file = inbox.Files[0];
            Console.WriteLine(JsonConvert.SerializeObject(file));

            Console.WriteLine(Connection.GetInboxDocumentFile(1, file, "C:\\test\\inbox\\"));

            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Document")]
        public void TransferInboxDocument()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();

            Inbox inbox = Connection.GetInbox(1);
            Square9APIHelperLibrary.DataTypes.File file = inbox.Files[0];
            Console.WriteLine(JsonConvert.SerializeObject(file));

            Console.WriteLine(Connection.TransferInboxDocument(1, 2, file));

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
            List<SecuredGroup> securedGroups = Connection.GetSecuredUsersAndGroups();
            Console.WriteLine(JsonConvert.SerializeObject(securedGroups));
            List<UnsecuredGroup> unsecuredGroups = Connection.GetUnsecuredUsersAndGroups();
            Console.WriteLine(JsonConvert.SerializeObject(unsecuredGroups));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void GetTreeStructure()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<SecurityNode> securityNodes = Connection.GetTreeStructure();
            Console.WriteLine(JsonConvert.SerializeObject(securityNodes));
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void GetUserArchivePermissions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.GetUserArchivePermissions(2, 1, "test").Level);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void GetUserInboxPermissions()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.GetUserInboxPermissions(1, "test").Level);
            Connection.DeleteLicense();
        }
        [TestMethod]
        [TestCategory("Administration")]
        public void GetUserSearchProperties()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<SearchProperties> userSearchSecurity = Connection.GetUserSearchProperties(1, "SSAdministrator");
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
            SecuredGroup securedGroup = Connection.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
            User user = new User();
            user.ConvertSecuredGroup(securedGroup);
            archiveSecurity.Users.Add(user);

            //Add Targets
            Target target = new Target();
            target.Id = 1; //archive ID
            target.Database = 2; //database ID
            archiveSecurity.Targets.Add(target);

            //Add Permissions
            ArchivePermission permission = new ArchivePermission(Connection.GetUserArchivePermissions(2, 1, "test").Level);
            permission.View = true;
            permission.Add = true;
            permission.Delete = true;
            permission.Print = true;
            permission.FullAPIAccess = true;
            permission.CalculatePermissionLevel();
            archiveSecurity.Permissions = permission;

            Console.WriteLine(JsonConvert.SerializeObject(archiveSecurity));
            Connection.SetArchiveSecurity(archiveSecurity);
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
            SecuredGroup securedGroup = Connection.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
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
            Connection.SetInboxSecurity(inboxSecurity);
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
            SecuredGroup securedGroup = Connection.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
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

            Connection.SetDatabaseSecurity(databaseSecurity);

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
            SecuredGroup securedGroup = Connection.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
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

            Connection.SetSearchSecurity(searchSecurity);

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
            SecuredGroup securedGroup = Connection.GetSecuredUsersAndGroups().Find(x => x.Name == "test");
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

            Connection.SetSearchProperties(searchSecurity);

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
            Connection.CreateUser(newUser);

            //Get the new user
            UnsecuredGroup unsecuredGroup = Connection.GetUnsecuredUsersAndGroups().Find(x => x.Name == "NewTestUser");
            User user = new User();
            user.ConvertUnsecuredGroup(unsecuredGroup);

            Console.WriteLine(JsonConvert.SerializeObject(user));

            //Update new user
            user.Password = "NewPassword123!@#";
            Connection.UpdateUser(user);

            Console.WriteLine(JsonConvert.SerializeObject(user));

            //Delete the user
            Connection.DeleteUser(user);

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
            Connection.CreateGroup(newGroup);

            //Get the new group
            Group group = Connection.GetGroups().Find(x => x.Name == "NewTestGroup");

            //Get some users
            List<UnsecuredGroup> unsecuredGroups = Connection.GetUnsecuredUsersAndGroups().FindAll(x => x.Type == 2); //Type 2 is for S9 users

            Console.WriteLine(JsonConvert.SerializeObject(group));
            Console.WriteLine(JsonConvert.SerializeObject(unsecuredGroups));

            //Add users to new group
            group.Users.Add(unsecuredGroups[0].Name);
            group.Users.Add(unsecuredGroups[2].Name);
            group.Users.Add(unsecuredGroups[4].Name);
            group.Users.Add(unsecuredGroups[6].Name);

            //Update group
            Connection.UpdateGroup(group);

            //Get the updated group
            Group updatedGroup = Connection.GetGroups().Find(x => x.Name == "NewTestGroup");

            Console.WriteLine(JsonConvert.SerializeObject(updatedGroup));

            //Delete group
            Connection.DeleteGroup(updatedGroup);

            Connection.DeleteLicense();
        }
        #endregion
    }
}
