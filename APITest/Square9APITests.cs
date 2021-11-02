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
        public void BasicAPIConnection()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            DatabaseList TestDatabaseList = Connection.GetDatabases(1);
            Connection.DeleteLicense();
            Assert.AreEqual(1, TestDatabaseList.Databases[0].Id);
        }
        [TestMethod]
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
        public void CheckIfAdmin()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.IsAdmin());
            Assert.IsTrue(Connection.IsAdmin());
            Connection.DeleteLicense();
        }
        [TestMethod]
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
        public void RebuildDatabaseIndex()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Connection.RebuildDatabaseIndex(1);
            Connection.DeleteLicense();
        }
        [TestMethod]
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
        public void GetArchiveFields()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Field> Fields = Connection.GetArchiveFields(1,1);
            Console.WriteLine(Fields[0].Name);
            Connection.DeleteLicense();
        }
        [TestMethod]
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
        public void RebuildContentIndex()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            Console.WriteLine(Connection.RebuildArchiveContentIndex(1, 3, 10000));
            Connection.DeleteLicense();
        }
        [TestMethod]
        public void GetSearches()
        {
            Square9API Connection = new Square9API(Endpoint, Username, Password);
            Connection.CreateLicense();
            List<Search> searches = Connection.GetSearches(1);
            List<Search> archiveSearches = Connection.GetSearches(1, 1);
            Console.WriteLine(searches[0].Name);
            Console.WriteLine(archiveSearches[0].Name);
            Connection.DeleteLicense();
        }
    }
}
