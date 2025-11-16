# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Technology Stack

- **Framework**: ASP.NET Core MVC 8.0 (.NET 8 LTS)
- **ORM**: Entity Framework Core 8.0 (Code-First with Migrations)
- **Authentication**: ASP.NET Core Identity 8.0
- **Dependency Injection**: Autofac 8.0 (with ASP.NET Core integration)
- **UI Framework**: Material Design (Bootstrap Material Design)
- **Testing**: MSTest 3.2 (with .NET Test SDK)
- **Database**: SQL Server (via Entity Framework Core)

## Build & Development Commands

### Building the Solution
```bash
# Restore NuGet packages
dotnet restore AccountingSystem.sln

# Build the entire solution
dotnet build AccountingSystem.sln --configuration Debug
dotnet build AccountingSystem.sln --configuration Release

# Build a specific project
dotnet build Src/AccountingSystem.Web/AccountingSystem.Web.csproj
```

### Database Migrations
```bash
# Add a new migration
dotnet ef migrations add <MigrationName> --project Src/AccountingSystem.Data --startup-project Src/AccountingSystem.Web

# Update database to latest migration
dotnet ef database update --project Src/AccountingSystem.Data --startup-project Src/AccountingSystem.Web

# Rollback to a specific migration
dotnet ef database update <MigrationName> --project Src/AccountingSystem.Data --startup-project Src/AccountingSystem.Web

# Remove the last migration (if not applied)
dotnet ef migrations remove --project Src/AccountingSystem.Data --startup-project Src/AccountingSystem.Web

# Generate SQL script for migrations
dotnet ef migrations script --project Src/AccountingSystem.Data --startup-project Src/AccountingSystem.Web
```

### Running Tests
```bash
# Run all tests
dotnet test AccountingSystem.sln

# Run tests for a specific project
dotnet test Src/AccountingSystem.Tests/AccountingSystem.Tests.csproj

# Run tests with detailed output
dotnet test --verbosity detailed

# Run tests with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Running the Web Application
```bash
# Run the web application
cd Src/AccountingSystem.Web
dotnet run

# Run with hot reload (development)
dotnet watch run

# The application will be available at:
# - https://localhost:5001 (HTTPS)
# - http://localhost:5000 (HTTP)
```

## Architecture Overview

The solution follows a **layered architecture** with clear separation of concerns:

### Project Structure

```
AccountingSystem.sln
├── AccountingSystem.Entity      # Domain models and entities
├── AccountingSystem.Data        # Data access layer (DbContext, migrations)
├── AccountingSystem.Service     # Business logic layer
├── AccountingSystem.Web         # Presentation layer (MVC controllers, views)
└── AccountingSystem.Tests       # Unit tests
```

### Layer Responsibilities

**1. Entity Layer** (`AccountingSystem.Entity`)
- Contains domain models: `Account`, `User`, `StockCode`, `Language`
- All entities inherit from `EntityBase`
- No dependencies on other layers
- Uses nullable reference types (.NET 8)

**2. Data Layer** (`AccountingSystem.Data`)
- `DataContext`: Main EF Core DbContext (inherits from `IdentityDbContext<User>`)
- Connection string: "DefaultConnection" (configured in `appsettings.json`)
- Uses constructor injection with `DbContextOptions<DataContext>`
- `IDataContext` and `IDataContextFactory` interfaces for testability
- Migrations managed via EF Core CLI tools
- Uses decimal precision of (18, 3) for all decimal properties

**3. Service Layer** (`AccountingSystem.Service`)
- `ServiceBase`: Base class for all services
- `ApiService`: API-related business logic
- Identity folder contains user/role management services
- Services should depend only on Data and Entity layers

**4. Web Layer** (`AccountingSystem.Web`)
- ASP.NET Core MVC with Razor views
- Controllers:
  - `HomeController`: Landing pages
  - `AccountController`: Authentication/registration
  - `UserController`: User management
  - `StockController`: Stock management
  - `MaterialController`: Material design demo pages
- Core services: `AppService`, `AuthService`, `CookieService`
- Autofac DI configuration in `App_Start/AutofacConfig.cs`
- Application startup configured in `Program.cs`

### Dependency Injection (Autofac + ASP.NET Core)

Services are registered in two places:

**Built-in DI (Program.cs):**
- DbContext registration
- ASP.NET Core Identity services
- MVC and Razor Pages services

**Autofac Container (AutofacConfig.cs):**
- **InstancePerLifetimeScope**: `IAppService`, `IAuthService`, `ICookieService`
- **SingleInstance**: `IDataContextFactory`

When adding new services:
1. Define an interface in the appropriate layer
2. Implement the service class
3. Register in `AutofacConfig.ConfigureContainer()` or `Program.cs`

### Database Access Pattern

The solution uses the **Factory pattern** for DbContext creation:
```csharp
// Inject IDataContextFactory
public class MyService
{
    private readonly IDataContextFactory _contextFactory;

    public MyService(IDataContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public void DoWork()
    {
        using (var context = _contextFactory.GetContext())
        {
            // Use context
            context.SaveChange(); // Note: Custom method, not SaveChanges()
        }
    }
}
```

**Important**: Use `SaveChange()` (custom method) instead of `SaveChanges()` - it includes validation error tracing.

**Alternative (Recommended for ASP.NET Core):**
For new code, consider injecting `DataContext` directly instead of using the factory pattern:
```csharp
public class MyService
{
    private readonly DataContext _context;

    public MyService(DataContext context)
    {
        _context = context;
    }

    public void DoWork()
    {
        // Use context
        _context.SaveChange();
    }
}
```

## Key Implementation Details

### Entity Framework Core Configuration
- Code-First approach with migrations
- Connection string configured in `appsettings.json`
- DbContext registered as scoped service in dependency injection container
- All decimal properties automatically configured with precision (18, 3) via `OnModelCreating`
- Validation errors are traced to Debug output on save failures

### ASP.NET Core Identity
- User entity is in `AccountingSystem.Entity.User`
- Extends `IdentityUser` with custom properties: `FirstName`, `LastName`, `Token`, `CreatedUtc`
- Authentication configured in `Program.cs`
- Cookie-based authentication
- Identity UI scaffolding available via `Microsoft.AspNetCore.Identity.UI` package

### Configuration Management
- Configuration files:
  - `appsettings.json`: Main configuration (connection strings, logging)
  - `appsettings.Development.json`: Development-specific settings
- Access configuration via `IConfiguration` interface
- Connection string location: `ConnectionStrings:DefaultConnection`

### Material Design UI
- Uses Bootstrap Material Design components
- Multiple layout templates in `Views/Shared/`:
  - `_Layout.cshtml` (default)
  - `_Layout2.cshtml`, `_Layout3.cshtml`, `_Layout4.cshtml` (alternatives)
- Static files (CSS/JS) served from `wwwroot` folder (ASP.NET Core convention)
- Material-specific CSS/JS in `wwwroot/css/MaterialBootstrap/` and `wwwroot/js/MaterialBootstrap/`

### Testing
- Uses MSTest framework (MSTest 3.2)
- Test SDK: Microsoft.NET.Test.Sdk 17.9
- Run tests via `dotnet test` command
- Test project references the Web project directly

## Initial Setup

When setting up a new instance:
1. Ensure .NET 8 SDK is installed (`dotnet --version`)
2. Ensure SQL Server is accessible
3. Update database connection string in `appsettings.json`
4. Restore NuGet packages: `dotnet restore`
5. Create initial migration (if needed): `dotnet ef migrations add InitialCreate --project Src/AccountingSystem.Data --startup-project Src/AccountingSystem.Web`
6. Apply migrations: `dotnet ef database update --project Src/AccountingSystem.Data --startup-project Src/AccountingSystem.Web`
7. Build solution: `dotnet build`
8. Run application: `dotnet run --project Src/AccountingSystem.Web`

## Migration from .NET Framework

This project was migrated from ASP.NET MVC 5 (.NET Framework 4.5) to ASP.NET Core MVC 8.0:

**Key Changes:**
- Entity Framework 6 → Entity Framework Core 8
- ASP.NET Identity 2.2 → ASP.NET Core Identity 8
- System.Web → ASP.NET Core middleware pipeline
- Web.config → appsettings.json
- Global.asax/Startup.cs → Program.cs
- packages.config → PackageReference in .csproj
- Old-style .csproj → SDK-style .csproj

**Breaking Changes to Note:**
- Old EF 6 migrations must be recreated for EF Core
- OWIN middleware replaced with ASP.NET Core middleware
- `InstancePerRequest` → `InstancePerLifetimeScope` in Autofac
- HttpContext access patterns have changed
- Routing configured differently in ASP.NET Core

## Naming Conventions

- The assembly names have a "CrossOver" namespace in some places (legacy from template)
- New code should use "AccountingSystem" namespace consistently
- Entity classes are simple POCOs in the Entity project
- Service classes end with "Service" suffix
- Controllers end with "Controller" suffix
- All projects use nullable reference types (enabled in .NET 8)
