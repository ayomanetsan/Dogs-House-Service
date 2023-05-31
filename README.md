# Dog Shelter API

This repository features a REST API for a dog house service. It provides several endpoints to manage shelter, including getting the list of all dogs, supporting sorting and pagination; and adding new pets to the database.

## Tech Stack

The dog shelter API is built with the help of the following:

- .NET 6: A free, open-source, and cross-platform framework for building modern applications.
- ASP.NET Core: A high-performance, cross-platform framework for building web APIs.
- AutoMapper: A convention-based object-to-object mapper for .NET that simplifies object mapping.
- Entity Framework Core: A lightweight, extensible, and open-source object-relational mapping (ORM) framework for .NET.
- xUnit: A popular unit testing framework for .NET applications.
- FakeItEasy: A flexible and easy-to-use mocking library for .NET.

## User Secrets

To run the Dog Shelter API, you need to provide a connection string for the database. This connection string is stored in the user secrets file. Here's how you can set it up:

1. Open the project in your preferred development environment.
2. Locate the `appsettings.json` file.
3. Open the user secrets file associated with your development environment. (For example, in Visual Studio, you can right-click on the project and select "Manage User Secrets.")
4. Add the following JSON structure to the user secrets file, replacing `name`, `dbname`, and any other necessary values with your own configuration:

```json
{
  "ConnectionStrings": {
    "DogsHouseServiceDbConnection": "Server=name;Database=dbname;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
## Getting Started
To get started with the Dog Shelter API, follow these steps:

1. Clone this repository to your local machine.
2. Set up the user secrets file as described in the previous section.
3. Build the solution to restore dependencies and compile the project.
4. Apply the necessary database migrations.
```
dotnet ef database update
```
5. Run the application, and it will start the API server.
6. You can now access the Dog Shelter API and interact with the available endpoints.
7. Feel free to explore the codebase, run tests, and make any necessary modifications to suit your specific requirements.
