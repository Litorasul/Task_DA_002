# .NET 9 API Template Project

This is a template project for creating a .NET 9 API with MSSQL, generic repository pattern, HttpService, Swagger, and unit tests. This template provides a solid foundation for building future .NET 9 API projects.

## Features

- MSSQL database setup
- Generic repository pattern
- HttpService for HTTP requests
- Unit tests using xUnit and Moq
- Swagger for API documentation
- Dependency Injection (DI)

## Getting Started

### Prerequisites

- .NET 9 SDK
- MSSQL Server
- Visual Studio 2022 or later

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/yourusername/dotnet9-api-template.git
   cd dotnet9-api-template
2. **Install NuGet packages:**
    Restore the NuGet packages using the Package Manager Console or running:
   ```bash
   dotnet restore
### Configuration

1. **Connection String:**
   Update the connection string in appsettings.json:

2.**Swagger Configuration:**
Swagger is configured in Program.cs for API documentation.

### Running the Application

1. **Run the application:**

   Use the following command to run the application:

   ```bash
   dotnet run

2. **Swagger UI:**

   Navigate to `https://localhost:{port}/swagger` to access the Swagger UI and explore the API endpoints.
   
