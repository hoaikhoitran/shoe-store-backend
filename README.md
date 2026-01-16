ShoeStore API
ShoeStore API is a backend web application built on .NET 9, exposing a set of RESTful endpoints for managing a shoe store, including authentication, user management, and core store features such as products and orders. The project follows a clean, layered architecture with clear separation between controllers, services, repositories, and data models.
​

Tech Stack
ASP.NET Core 9 – Primary framework for building the RESTful API.
​

Entity Framework Core – Object–relational mapper used to interact with the PostgreSQL database via Code First and migrations.

PostgreSQL – Main relational database for persisting application data.

FluentValidation – Library for expressive, strongly-typed request validation at the DTO level.

JWT Authentication – JSON Web Tokens for stateless authentication and authorization across protected endpoints.

Getting Started
Prerequisites
.NET 9 SDK installed.
​

PostgreSQL instance (local or remote).

Git.

An IDE or editor such as Visual Studio, Rider, or Visual Studio Code.

Installation
Clone the repository

bash
git clone <repository-url>
cd ShoeStore.API
Configure application settings

Create or update appsettings.Development.json (or appsettings.json) with your own values:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=<host>;Database=<database>;Username=<username>;Password=<password>"
  },
  "JwtSettings": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "SecretKey": "your-secret-key"
  }
}
Apply database migrations

bash
dotnet ef database update
Run the application

bash
dotnet run
By default, the API will be available on the configured Kestrel/launchSettings ports, and Swagger (if enabled) will expose interactive documentation for the endpoints.
​

Run tests (if configured)

bash
dotnet test
Project Structure
The solution is organised into clearly defined layers to improve maintainability, testability, and readability:

Controllers/
Contains the API controllers, where each controller defines HTTP endpoints, handles request/response mapping, and delegates business logic to the service layer.

Data/
Includes the DbContext and any database-related configuration such as entity configurations, migrations, and seeding logic.

Models/
Holds domain entities that represent the core data structures persisted in the database.

DTOs/
Contains Data Transfer Objects used to shape request and response payloads, decoupling external contracts from internal entities.

Repositories/
Encapsulates data access operations, providing a clean abstraction over Entity Framework Core and improving testability.

Services/
Implements business logic, orchestrating repositories, validations, and other components to fulfil application use cases.

Middlewares/
Defines custom middleware components used to handle cross-cutting concerns such as error handling, logging, or request processing.

Properties/
Includes configuration files for hosting and debugging, for example launchSettings.json.

appsettings.json*
Central configuration files for environment-specific settings such as connection strings, JWT configuration, and other application options.

Program.cs
Entry point of the application, responsible for bootstrapping the host, registering services (dependency injection), configuring middleware, and wiring up routing and endpoints.
​

Authentication Overview
The API uses JWT-based authentication to secure protected resources. Clients obtain a token by authenticating with valid credentials; the token is then attached to subsequent requests via the Authorization: Bearer <token> header. Token validation is handled in the middleware pipeline, ensuring that only authenticated users can access secured endpoints.
