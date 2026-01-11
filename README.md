# Inventory Management System with Blazor

_Business diagram:_

![Screenshot 2022-12-27 223312](https://user-images.githubusercontent.com/43082250/209690923-0d85c332-464e-4f8c-8187-77f93e7c226c.png)

### What's the experiences?

- The basics that an inventory management system must have.
- Razor / Blazor Components
- Forms Submission, Validation in Blazor
- Custom Validation with ValidationAttribute
- Dependency Injection
- ASPNET Core Identity
- Clean Architecture with Use Case Driven Development
- Create two type of data stores (In-Memory plugin and EF Core plugin) with repository pattern as plugins following clean architecture
- Entity Framework Core connecting to SQL Server

### Technologies/libraries

- .NET 10 Blazor Server
- Entity Framework Core 10 with MSSQL
- ASPNET Core Identity

### Getting Started

1. **Prerequisites**:

   - .NET 10 SDK
   - SQL Server (LocalDB or Standard)

2. **Configuration**:

   - Update `appsettings.json` with your SQL Server connection string:
     ```json
     "ConnectionStrings": {
       "InventoryManagement": "Server=(localdb)\\mssqllocaldb;Database=InventoryManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
     ```

3. **Run the Application**:
   ```bash
   dotnet run --project IMS.WebApp
   ```

### Why use Blazor?

Blazor (.NET 10 Blazor) is Microsoft latest SPA application framework. With Blazor you can build reactive full stack single page web applications with C# without much help from JavaScript. More and more companies are adopting Blazor as part of their technology stack.

_References_: https://www.udemy.com/course/learn-blazor-while-creating-an-inventory-management-system
