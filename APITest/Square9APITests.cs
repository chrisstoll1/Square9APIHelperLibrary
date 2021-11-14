using Microsoft.VisualStudio.TestTools.UnitTesting;
using Square9APIHelperLibrary;
using Square9APIHelperLibrary.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            search.Detail[1].Val = "test";
            search.Detail[2].Val = "10/29/2020";
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
            Search search = Connection.GetSearches(1, searchId: 20)[0];
            search.Detail[1].Val = "test";
            search.Detail[2].Val = "10/29/2020";
            ArchiveCount results = Connection.GetSearchCount(1, search);
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
    }
}
