# Welcome to the Square9APIHelperLibrary wiki

This wiki is still a work in progress, please feel free to come back to this page to see new content as it is added.

To start using this library add the [.dll](https://github.com/chrisstoll1/Square9APIHelperLibrary/releases) as a reference in your project along with the dependencies, or install the [NuGet](https://www.nuget.org/packages/Square9APIHelperLibrary/) package.

# Quickstart

1. Add the following using statements to the top of your application

   ```
   using Square9APIHelperLibrary;
   using Square9APIHelperLibrary.DataTypes;
   ```
2. Create a new [Square9API](../api/Square9APIHelperLibrary.Square9API.html) object, it will take three [parameters](../api/Square9APIHelperLibrary.Square9API.html#Square9APIHelperLibrary_Square9API__ctor_System_String_System_String_System_String_) for the **Endpoint**, **Username**, and **Password**

   ```
   Square9API Connection = new Square9API(Endpoint, Username, Password);
   ```
3. Before you can start interfacing with the API, you must first get a license from the server by calling the [CreateLicense](../api/Square9APIHelperLibrary.Square9API.html#Square9APIHelperLibrary_Square9API_CreateLicense) method

   ```
   Connection.CreateLicense();
   ```
4. In this example, we will loop through and print the ID of each database on the server

   ```
   DatabaseList databases = Connection.Databases.GetDatabases();
   
   foreach (Database database in databases.Databases)
   {
   Console.WriteLine(database.Name);
   }
   ```
5. Once you are done with your connection, it is best practice to delete the license on the server to free it up for anyone who may need it

   ```
   Connection.DeleteLicense();
   ```
