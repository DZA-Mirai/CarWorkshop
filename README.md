# CarWorkshop

### Prerequisites
- .NET 8 SDK
- CloudinaryDotNet (Package)
- Microsoft.EntityFrameworkCore (Package)
- Microsoft.EntityFrameworkCore.SqlServer (Package)
- Microsoft.EntityFrameworkCore.Tools (Package)
- SQL Server Management Studio 19
- Visual Studio 2022

### Add Connection String
1. Open SQL Server and Create new Database "CarWorkshop"
2. Open Visual Studio
3. Search for SQL Server Object Explorer
4. Connect to your SQL Server
5. Open Properties for CarWorkshop database
6. Find Connection string and copy it

### Configuration of ConnectionStrings

1. Update the `appsettings.json` file with your database connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Paste your connection string"
     }
   }

### Setting Up the Database
1. Open Package Manager Console in Visual Studio
2. `Add-Migration InitialCreate`
3. `Update-Database`
4. Open Developer Powershell
5. `dotnet run seeddata`

### Authorization
- Open seeddata file
- Find Login and Password for employee (Usually Login = "name"13 and Password = 12345678)
