ShoeStore.API
Simple Web API for a shoe store built with ASP.NET Core and Entity Framework Core.

Features
Manage shoes (create, read, update, delete).

Filter and search shoes by name, price, size, and color.

Pagination for shoe lists.

Basic stock check for each shoe and size.

Main Structure
Models/Entities – database entities (Shoe, User, etc.).

Models/DTOs – request/response models and filter parameters.

Repositories – data access logic (ShoeRepository).

Mappings – mapping between entities and DTOs.

Controllers – HTTP endpoints for the API.

Run the project
Set your PostgreSQL connection string in appsettings.json.

Apply database migrations:

bash
dotnet ef database update
Run the API:

bash
dotnet run
The API will be available on https://localhost:<port>.
