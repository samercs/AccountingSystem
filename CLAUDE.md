# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Technology Stack

- **Framework**: ASP.NET MVC 5.2.3 (.NET Framework 4.5)
- **ORM**: Entity Framework 6.1.3 (Code-First with Migrations)
- **Authentication**: ASP.NET Identity 2.2.1
- **Dependency Injection**: Autofac 3.4.0
- **UI Framework**: Material Design (Bootstrap Material Design)
- **Testing**: MSTest (Visual Studio Unit Testing Framework)
- **Database**: SQL Server (via Entity Framework)

## Build & Development Commands

### Building the Solution
```bash
# Restore NuGet packages
nuget restore AccountingSystem.sln

# Build the entire solution
msbuild AccountingSystem.sln /p:Configuration=Debug
msbuild AccountingSystem.sln /p:Configuration=Release
```

### Database Migrations
```bash
# Run migrations to update database (execute from Package Manager Console in Visual Studio)
Update-Database

# Add a new migration
Add-Migration <MigrationName>

# Rollback to a specific migration
Update-Database -TargetMigration <MigrationName>
```

### Running Tests
```bash
# Run all tests using MSTest
mstest /testcontainer:Src/AccountingSystem.Tests/bin/Debug/AccountingSystem.Tests.dll

# Or use Visual Studio Test Explorer (Ctrl+E, T)
```

### Running the Web Application
The web application runs on IIS Express at http://localhost:13920/. Visual Studio handles the startup automatically, or use:
```bash
# From Visual Studio: F5 (Debug) or Ctrl+F5 (Run without debugging)
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

**2. Data Layer** (`AccountingSystem.Data`)
- `DataContext`: Main EF DbContext (inherits from `IdentityDbContext<User>`)
- Connection string: "DefaultConnection"
- Lazy loading and proxy creation are **disabled**
- `IDataContext` and `IDataContextFactory` interfaces for testability
- All migrations stored in `Migrations` folder
- Uses decimal precision of (18, 3) for all decimal properties

**3. Service Layer** (`AccountingSystem.Service`)
- `ServiceBase`: Base class for all services
- `ApiService`: API-related business logic
- Identity folder contains user/role management services
- Services should depend only on Data and Entity layers

**4. Web Layer** (`AccountingSystem.Web`)
- ASP.NET MVC 5 with Razor views
- Controllers:
  - `HomeController`: Landing pages
  - `AccountController`: Authentication/registration
  - `UserController`: User management
  - `StockController`: Stock management
  - `MaterialController`: Material design demo pages
- Core services: `AppService`, `AuthService`, `CookieService`, `SigninManager`
- Autofac DI configuration in `App_Start/AutofacConfig.cs`

### Dependency Injection (Autofac)

Services are registered in `App_Start/AutofacConfig.cs`:
- **InstancePerRequest**: `IAppService`, `IAuthService`, `ICookieService`
- **SingleInstance**: `IDataContextFactory`

When adding new services:
1. Define an interface in the appropriate layer
2. Implement the service class
3. Register in `AutofacConfig.RegisterAll()`

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
        using (var context = _contextFactory.Create())
        {
            // Use context
            context.SaveChange(); // Note: Custom method, not SaveChanges()
        }
    }
}
```

**Important**: Use `SaveChange()` (custom method) instead of `SaveChanges()` - it includes validation error tracing.

## Key Implementation Details

### Entity Framework Configuration
- Code-First approach with automatic migrations
- Connection string must be named "DefaultConnection" in Web.config
- All decimal properties automatically configured with precision (18, 3)
- Validation errors are traced to Debug output on save failures

### ASP.NET Identity
- User entity is in `AccountingSystem.Entity.User`
- Extends `IdentityUser` with custom properties
- Authentication configured in `App_Start/Startup.Auth.cs`
- Custom `SigninManager` in Web layer

### Material Design UI
- Uses Bootstrap Material Design components
- Multiple layout templates in `Views/Shared/`:
  - `_Layout.cshtml` (default)
  - `_Layout2.cshtml`, `_Layout3.cshtml`, `_Layout4.cshtml` (alternatives)
- Material-specific CSS/JS in `Content/MaterialBootstrap/` and `Scripts/MaterialBootstrap/`

### Testing
- Uses MSTest framework
- Controller tests extend Visual Studio's unit testing framework
- Test project references the Web project directly

## Initial Setup (from SetUp/Read Me.txt)

When setting up a new instance:
1. Ensure SQL Server is accessible
2. Update database connection string in `Web.config`
3. Delete any existing database in `App_Data` folder (if applicable)
4. Run `Update-Database` from Package Manager Console
5. Restore NuGet packages
6. Build solution and run locally to verify

## Naming Conventions

- The assembly names have a "CrossOver" namespace in some places (legacy from template)
- New code should use "AccountingSystem" namespace consistently
- Entity classes are simple POCOs in the Entity project
- Service classes end with "Service" suffix
- Controllers end with "Controller" suffix
