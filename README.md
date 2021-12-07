# Square9API Helper Library
This .NET Class Library is intended to provide easy access to the [Square9API](http://www.square-9.com/api/) and its endpoints. Built with [RestSharp](https://github.com/restsharp/RestSharp) and [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json), it includes all necessary methods and classes to exchange information with the Square9API. 

## Prerequisites
 - Newtonsoft.Json (>= 13.0.1)
 - RestSharp (>= 106.13.0)

## Documentation
Documentation for this library can be found [here](https://github.com/chrisstoll1/Square9APIHelperLibrary/wiki)

## Installation
To get started, download the Square9APIHelperLibrary.dll from [releases](https://github.com/chrisstoll1/Square9APIHelperLibrary/releases), or install the package via [Nuget](https://www.nuget.org/packages/Square9APIHelperLibrary/)

    dotnet add package Square9APIHelperLibrary --version 0.0.1

Once in your project, use the following code below to get started:

    using Square9APIHelperLibrary; //Required
    using Square9APIHelperLibrary.DataTypes; //Required to use built in data types
    
    string Endpoint = "http://{Your host here}/Square9API";
    string Username = "{Username}";
    string Password = "{Password}";
    
    Square9API Connection = new Square9API(Endpoint, Username, Password); //Create new connection
    
    DatabaseList databases = Connection.GetDatabases(); //Retrieve a list of databases
    
    foreach (Database database in databases.Databases) //Loop through databases
    {
        Console.WriteLine(database.Id); //Print the ID of each database
    }
