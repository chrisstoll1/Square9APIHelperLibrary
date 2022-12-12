
# Why Powershell?

  

The Square9API Helper library and Powershell make a great combination because they offer a number of benefits when used together. First, the Square9API Helper library provides a set of easy-to-use, pre-defined functions that can be used to interact with the Square9 API, making it easier for developers to access and manipulate data stored in Square9. This can help to streamline development and reduce the amount of time and effort required to build applications that integrate with Square9. Additionally, Powershell is a powerful scripting language that allows developers to automate and manage a wide range of tasks, including interacting with APIs such as the Square9API. This means that developers can use Powershell to quickly and easily access data from Square9, making it possible to build complex, data-driven applications with minimal effort. Overall, the combination of the Square9API Helper library and Powershell provides a powerful and flexible toolset for working with the Square9 API and building applications that integrate with Square9.

  

## Getting Started

1. Download the [.dll](https://github.com/chrisstoll1/Square9APIHelperLibrary/releases) from releases along with the [prerequisites](https://github.com/chrisstoll1/Square9APIHelperLibrary#prerequisites). Place these in the same directory as your powershell script

2. Open up PowerShell ISE and create a new script

3. Add the following using statements to the top of your new script

```
using assembly ".\Square9APIHelperLibrary.dll"
using namespace Square9APIHelperLibrary
using namespace Square9APIHelperLibrary.DataTypes
```

4. Create a new [Square9API](../api/Square9APIHelperLibrary.Square9API.html) object, it will take three [parameters](../api/Square9APIHelperLibrary.Square9API.html#Square9APIHelperLibrary_Square9API__ctor_System_String_System_String_System_String_) for the **Endpoint**, **Username**, and **Password**

```
$connection = New-Object Square9APIHelperLibrary.Square9API($Endpoint, $User, $Pass)
```

5. Before you can start interfacing with the API, you must first get a license from the server by calling the [CreateLicense](../api/Square9APIHelperLibrary.Square9API.html#Square9APIHelperLibrary_Square9API_CreateLicense) method

```
$connection.CreateLicense()
```

You are now ready to start using the Square9API Helper Library in Powershell!

### Example: Move documents between archives
To move documents between archives we need to run through the following steps

 - Get a Search Object from the API on our source archive
> This code calls [Searches.GetSearches](../api/Square9APIHelperLibrary.Square9APIComponents.Searches.html#Square9APIHelperLibrary_Square9APIComponents_Searches_GetSearches_System_Int32_System_Int32_System_Int32_) which retrieves a list of searches from our database > source archive and stores the first one in our $search variable
 ```
 [Search]$search = $connection.Searches.GetSearches($database.Id, $sourceArchive.Id)[0]
 ```
 - Get a list of documents by running $search
 > This code calls [Searches.GetSearchResults](../api/Square9APIHelperLibrary.Square9APIComponents.Searches.html#Square9APIHelperLibrary_Square9APIComponents_Searches_GetSearchResults_System_Int32_Square9APIHelperLibrary_DataTypes_Search_System_Int32_System_Int32_System_Int32_System_Int32_System_Int32_) which retrieves the first 1000 documents from the first page of search results and stores it in our $searchResult variable (based on additonal parameters). 
 ```
 [Result]$searchResult = $connection.Searches.GetSearchResults($database.Id, $search, 1, 1000)
 ```
 - Loop through list of documents and move each one to our destination archive
 > This code loops through our search results and calls [Documents.TransferArchiveDocument](../api/Square9APIHelperLibrary.Square9APIComponents.Documents.html#Square9APIHelperLibrary_Square9APIComponents_Documents_TransferArchiveDocument_System_Int32_System_Int32_System_Int32_Square9APIHelperLibrary_DataTypes_Doc_System_Boolean_) which takes a source archive, and a destination archive and either copies or moves the file based on the move boolean parameters (if move is set to true, document in the source archive will be removed). 
 ```
 foreach($doc in $searchResult.Docs){
        #move document
        $connection.Documents.TransferArchiveDocument($database.Id, $sourceArchive.Id, $destArchive.Id, $doc, $true)
}
 ```

Your documents should now be moving between archives through powershell! 



