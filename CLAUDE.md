# AccountingSystem - AI Assistant Guide

This document provides a comprehensive overview of the AccountingSystem codebase for AI assistants working on this project.

## Table of Contents

- [Project Overview](#project-overview)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Key Components](#key-components)
- [Development Workflows](#development-workflows)
- [Conventions & Patterns](#conventions--patterns)
- [Database](#database)
- [Authentication & Authorization](#authentication--authorization)
- [Testing](#testing)
- [Common Tasks](#common-tasks)

---

## Project Overview

**Type:** ASP.NET MVC 5 Web Application with Web API
**Framework:** .NET Framework 4.5 (Traditional .NET Framework, NOT .NET Core)
**Purpose:** Accounting system with user management, authentication, and financial tracking capabilities
**Status:** Educational/Portfolio project

### Important Note
This is **ASP.NET MVC 5**, not ASP.NET Core. Be mindful of this when suggesting libraries, patterns, or configurations.

---

## Architecture

### Layered Architecture (4-Layer Pattern)

```
┌─────────────────────────────────────────┐
│   AccountingSystem.Web (Presentation)   │
│   - MVC Controllers & Views             │
│   - Web API Controllers                 │
│   - ViewModels, Filters, Startup        │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│   AccountingSystem.Service (Business)   │
│   - ServiceBase, UserService            │
│   - Business Logic, Validation          │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│   AccountingSystem.Data (Data Access)   │
│   - DataContext (EF DbContext)          │
│   - Migrations, Factories               │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│   AccountingSystem.Entity (Domain)      │
│   - User, Account, StockCode            │
│   - Domain Models, Validation           │
└─────────────────────────────────────────┘
```

**Test Project:** `AccountingSystem.Tests` (MSTest framework)

---

## Technology Stack

### Core Framework
- **ASP.NET MVC:** 5.2.3
- **.NET Framework:** 4.5
- **Entity Framework:** 6.1.3
- **ASP.NET Identity:** 2.2.1
- **ASP.NET Web API:** 5.2.3

### Authentication & Security
- **OWIN:** 3.0.1
- **Microsoft.Owin.Security.OAuth:** OAuth 2.0 bearer tokens
- **Microsoft.Owin.Security.Cookies:** Cookie-based authentication

### Dependency Injection
- **Autofac:** 3.4.0
- **Autofac.Mvc5:** 3.3.4

### Frontend
- **jQuery:** 1.10.2
- **Bootstrap:** 3.0.0
- **Bootstrap Material Design:** Custom theme
- **Validation:** jQuery.Validation 1.11.1

### Database
- **SQL Server:** LocalDB (development)
- **Connection:** Integrated Security with MDF attachment

### Testing
- **MSTest:** Microsoft.VisualStudio.QualityTools.UnitTestFramework

### Other Key Libraries
- **Newtonsoft.Json:** 6.0.4
- **Antlr:** 3.4.1.9004
- **WebGrease:** 1.5.2
- **Respond:** 1.2.0

---

## Project Structure

```
AccountingSystem/
├── Src/
│   ├── AccountingSystem.Entity/          # Domain Models
│   │   ├── User.cs                       # Extends IdentityUser
│   │   ├── Account.cs                    # Financial accounts
│   │   ├── StockCode.cs                  # Stock codes
│   │   ├── Language.cs                   # Language support
│   │   └── EntityBase.cs                 # Abstract base entity
│   │
│   ├── AccountingSystem.Data/            # Data Access Layer
│   │   ├── DataContext.cs                # EF DbContext (IdentityDbContext<User>)
│   │   ├── IDataContext.cs               # Context interface
│   │   ├── DataContextFactory.cs         # Factory pattern implementation
│   │   └── Migrations/                   # EF Code-First migrations
│   │       ├── Configuration.cs          # Migration config & seeding
│   │       └── [6 migration files]
│   │
│   ├── AccountingSystem.Service/         # Business Logic Layer
│   │   └── AccountingSystem.service/
│   │       ├── ServiceBase.cs            # Abstract service base
│   │       ├── UserService.cs            # User business logic
│   │       ├── ApiService.cs             # API token service
│   │       └── Identity/
│   │           ├── UserManager.cs        # ASP.NET Identity UserManager
│   │           └── RoleManager.cs        # ASP.NET Identity RoleManager
│   │
│   ├── AccountingSystem.Web/             # Presentation Layer
│   │   ├── Controllers/                  # MVC & API Controllers
│   │   │   ├── ApplicationController.cs  # Base controller
│   │   │   ├── AccountController.cs      # Login, Register, Auth
│   │   │   ├── UserController.cs         # User dashboard
│   │   │   ├── HomeController.cs         # Home, About, Contact
│   │   │   ├── StockController.cs        # API: Stock prices
│   │   │   ├── MaterialController.cs     # UI templates
│   │   │   └── TestUiController.cs       # UI testing
│   │   ├── Models/                       # ViewModels
│   │   │   ├── AccountViewModels.cs      # Login, Register
│   │   │   ├── UserViewModels.cs         # User dashboard, stocks
│   │   │   └── ManageViewModels.cs       # Password, 2FA
│   │   ├── Views/                        # Razor views
│   │   │   ├── Account/                  # Auth views
│   │   │   ├── User/                     # User views
│   │   │   ├── Home/                     # Static pages
│   │   │   ├── Material/                 # Template demos
│   │   │   └── Shared/                   # Layouts, partials
│   │   ├── App_Start/                    # Startup configuration
│   │   │   ├── Startup.Auth.cs           # OWIN/OAuth config
│   │   │   ├── AutofacConfig.cs          # DI registration
│   │   │   ├── BundleConfig.cs           # CSS/JS bundles
│   │   │   ├── FilterConfig.cs           # Global filters
│   │   │   ├── RouteConfig.cs            # MVC routes
│   │   │   └── WebApiConfig.cs           # Web API routes
│   │   ├── Services/                     # Web layer services
│   │   │   ├── AppService.cs             # Application service container
│   │   │   ├── AuthService.cs            # Authentication wrapper
│   │   │   ├── CookieService.cs          # Cookie management
│   │   │   └── SignInManager.cs          # OWIN sign-in manager
│   │   ├── Providers/
│   │   │   └── ApplicationOAuthProvider.cs  # OAuth token provider
│   │   ├── Global.asax.cs                # Application startup
│   │   ├── Startup.cs                    # OWIN startup
│   │   └── Web.config                    # Configuration
│   │
│   └── AccountingSystem.Tests/           # Unit Tests
│       └── Controllers/
│           └── HomeControllerTest.cs     # Basic controller tests
│
└── AccountingSystem.sln                  # Solution file
```

---

## Key Components

### 1. Entity Layer (`AccountingSystem.Entity`)

**Location:** `/home/user/AccountingSystem/Src/AccountingSystem.Entity/`

#### Domain Models

**User** (`User.cs`):
```csharp
public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Token { get; set; }
    public DateTime CreatedUtc { get; }  // Computed property
}
```
- Extends ASP.NET Identity's `IdentityUser`
- Implements `IValidatableObject` for custom validation
- Reference: `Src/AccountingSystem.Entity/User.cs:1`

**Account** (`Account.cs`):
```csharp
public class Account
{
    public int AccountId { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
    public int Level { get; set; }
    public decimal InitBalance { get; set; }  // Precision: 18,3
}
```
- Chart of accounts structure
- Supports hierarchical relationships via ParentId
- Reference: `Src/AccountingSystem.Entity/Account.cs:1`

**StockCode** (`StockCode.cs`):
- Many-to-many relationship with User
- Reference: `Src/AccountingSystem.Entity/StockCode.cs:1`

**EntityBase** (`EntityBase.cs`):
- Abstract base class
- Provides `CreatedUtc` property (database-computed)
- Reference: `Src/AccountingSystem.Entity/EntityBase.cs:1`

### 2. Data Layer (`AccountingSystem.Data`)

**Location:** `/home/user/AccountingSystem/Src/AccountingSystem.Data/`

#### DataContext (`DataContext.cs`)

```csharp
public class DataContext : IdentityDbContext<User>, IDataContext
{
    public IDbSet<Account> Accounts { get; set; }

    // Configuration
    - Lazy loading: DISABLED
    - Proxy creation: DISABLED
    - Decimal precision: (18, 3) for all decimals
}
```
- Inherits `IdentityDbContext<User>` for ASP.NET Identity integration
- Implements `IDataContext` for abstraction
- Reference: `Src/AccountingSystem.Data/DataContext.cs:1`

#### Connection String
```
Data Source=(LocalDb)\MSSQLLocalDB;
AttachDbFilename=|DataDirectory|\AccountingSystemDB.mdf;
Initial Catalog=aspnet-AccountingSystem-20160729053308;
Integrated Security=True
```
- Uses SQL Server LocalDB
- MDF file attachment pattern
- Located in: `Web.config`

#### Factory Pattern

**DataContextFactory** (`DataContextFactory.cs`):
```csharp
public class DataContextFactory : IDataContextFactory
{
    public IDataContext GetDataContext()
        => new DataContext();
}
```
- Implements factory pattern for context creation
- Registered as `SingleInstance` in Autofac
- Reference: `Src/AccountingSystem.Data/DataContextFactory.cs:1`

#### Migrations

**Location:** `/home/user/AccountingSystem/Src/AccountingSystem.Data/Migrations/`

Migration history:
1. `InitMigration` - ASP.NET Identity tables
2. `AddStockCodeUserRelation` - Stock code many-to-many
3. `InsertStockCodeTestData` - Seed data
4. `AddUserToken` - User.Token column
5. `AddLanguage` - Language table
6. `AddAccounts` - Accounts table

**Note:** Migration `AddAccounts` drops existing StockCodes and Languages tables.

### 3. Service Layer (`AccountingSystem.Service`)

**Location:** `/home/user/AccountingSystem/Src/AccountingSystem.Service/AccountingSystem.service/`

#### ServiceBase (`ServiceBase.cs`)

```csharp
public abstract class ServiceBase
{
    protected readonly IDataContextFactory _dataContextFactory;

    protected IDataContext DataContext()
        => _dataContextFactory.GetDataContext();
}
```
- Abstract base for all services
- Provides data context access
- Reference: `Src/AccountingSystem.Service/AccountingSystem.service/ServiceBase.cs:1`

#### UserService (`UserService.cs`)

Key operations:
- `GetUserById(string userId)`
- `GetUserByEmail(string email)`
- `GetUsers()` - All users
- `GetUsersByQuery<T>(Expression<Func<User, T>> select)` - Custom projections
- `CreateUser(User user, string password)`
- `UpdateUser(User user)`
- `ChangePassword(string userId, string currentPassword, string newPassword)`
- Role management: `GetAllRoles()`, `AddUserToRole()`, `RemoveUserFromRole()`
- `ValidateUserIdentity(User user, User existingUser)` - Checks duplicate email/phone

Reference: `Src/AccountingSystem.Service/AccountingSystem.service/UserService.cs:1`

#### UserManager Configuration

```csharp
PasswordValidator:
- RequiredLength: 8
- RequireNonLetterOrDigit: false
- RequireDigit: false
- RequireLowercase: false
- RequireUppercase: false

UserLockout:
- MaxFailedAccessAttemptsBeforeLockout: 10
- DefaultAccountLockoutTimeSpan: 10 minutes

UserName:
- AllowOnlyAlphanumericUserNames: false
- RequireUniqueEmail: false
```
Reference: `Src/AccountingSystem.Service/AccountingSystem.service/Identity/UserManager.cs:1`

### 4. Web Layer (`AccountingSystem.Web`)

**Location:** `/home/user/AccountingSystem/Src/AccountingSystem.Web/`

#### Controllers

**ApplicationController** (`Controllers/ApplicationController.cs`):
- Base controller for all MVC controllers
- Dependencies: `IAppService`, `IDataContextFactory`, `IAuthService`, `ICookieService`
- Provides: `SetStatusMessage()`, `GetStatusMessage()` via TempData
- Reference: `Src/AccountingSystem.Web/Controllers/ApplicationController.cs:1`

**AccountController** (`Controllers/AccountController.cs`):
- `GET/POST Login` - Form-based authentication
- `GET/POST Register` - User registration
- `GET ForgotPassword` - Password recovery (placeholder)
- `POST LogOff` - Sign out
- Reference: `Src/AccountingSystem.Web/Controllers/AccountController.cs:1`

**UserController** (`Controllers/UserController.cs`):
- `[Authorize]` required
- `GET Index` - User dashboard with stock data
- `GET/POST SetPassword` - Password change form
- Reference: `Src/AccountingSystem.Web/Controllers/UserController.cs:1`

**StockController** (`Controllers/StockController.cs`):
- **Type:** ApiController (Web API)
- `GET api/stock?codes=1&codes=2` - Returns mock stock prices
- Returns: `List<StockResponse>`
- Reference: `Src/AccountingSystem.Web/Controllers/StockController.cs:1`

#### Services

**IAuthService / AuthService** (`Services/AuthService.cs`):
```csharp
Interface methods:
- bool IsAuthenticated()
- bool IsLocal()
- string CurrentUserId()
- Task SignIn(User user, bool isPersistent)
- void SignOut()
- Task<User> CurrentUser()
- Task<bool> CreateUser(User user, string password)
- Task<bool> ChangePassword(...)
```
- Wraps UserService and SignInManager
- Validates email + phone uniqueness
- Sets login cookies (DisplayName, LastSignInEmail)
- Reference: `Src/AccountingSystem.Web/Services/AuthService.cs:1`

**IAppService / AppService** (`Services/AppService.cs`):
```csharp
Properties:
- IDataContextFactory DataContextFactory
- IAuthService AuthService
- ICookieService CookieService
```
- Service locator pattern
- Registered as `InstancePerRequest` in Autofac
- Reference: `Src/AccountingSystem.Web/Services/AppService.cs:1`

**ICookieService / CookieService** (`Services/CookieService.cs`):
- `string Get(string key)`
- `bool TryGetBool(string key)`
- `void Add(string key, string value, int? expireDays = null)`
- Sets `Secure` and `HttpOnly` flags
- Reference: `Src/AccountingSystem.Web/Services/CookieService.cs:1`

#### ViewModels

**Location:** `/home/user/AccountingSystem/Src/AccountingSystem.Web/Models/`

**LoginViewModel**:
```csharp
[Required] string Email
[Required, DataType(Password)] string Password
[Display(Name = "Remember me?")] bool RememberMe
```

**RegisterViewModel**:
```csharp
[Required, EmailAddress] string Email
[Required] string FirstName
[Required] string LastName
[Phone] string PhoneNumber
[Required, StringLength(100, MinimumLength = 6)] string Password
[Compare("Password")] string ConfirmPassword
```

**User.IndexViewModel**:
```csharp
User User
IList<StockViewModel> Stocks
```

Reference: `Src/AccountingSystem.Web/Models/AccountViewModels.cs:1`

---

## Development Workflows

### Building the Project

```bash
# Using Visual Studio
1. Open AccountingSystem.sln
2. Build > Build Solution (Ctrl+Shift+B)

# Using MSBuild (command line)
msbuild AccountingSystem.sln /p:Configuration=Debug
```

### Running the Application

```bash
# IIS Express (default)
- Debug port: 13920
- F5 in Visual Studio

# Database initialization
- Migrations run automatically on first access
- LocalDB creates AccountingSystemDB.mdf in App_Data
```

### Database Migrations

```bash
# Enable migrations (already done)
Enable-Migrations

# Add new migration
Add-Migration MigrationName

# Update database
Update-Database

# Rollback to specific migration
Update-Database -TargetMigration: MigrationName
```

**Location:** Package Manager Console in Visual Studio
**Project:** Set to `AccountingSystem.Data`

### Adding New Features

#### 1. Adding a New Entity

```
1. Create entity class in AccountingSystem.Entity/
2. Add DbSet to DataContext.cs
3. Create migration: Add-Migration AddEntityName
4. Update database: Update-Database
5. Add service methods in ServiceBase-derived class
6. Create controller and views in Web project
```

#### 2. Adding a New API Endpoint

```
1. Create ApiController in Controllers/
2. Add methods with [HttpGet], [HttpPost], etc.
3. Return IHttpActionResult or typed data
4. Test via /api/{controller}/{action}
```

#### 3. Adding a New Service

```
1. Create interface in AccountingSystem.Service/
2. Implement service extending ServiceBase
3. Register in AutofacConfig.cs
4. Inject via constructor in controllers
```

### Testing

```bash
# Run all tests
Test > Run > All Tests (Ctrl+R, A)

# Run specific test
Right-click test method > Run Tests
```

**Test Project:** `AccountingSystem.Tests`
**Framework:** MSTest
**Reference:** `Src/AccountingSystem.Tests/Controllers/HomeControllerTest.cs:1`

---

## Conventions & Patterns

### Code Conventions

1. **Naming:**
   - PascalCase for classes, methods, properties
   - camelCase for parameters, local variables
   - Prefix interfaces with `I`
   - Prefix private fields with `_`

2. **File Organization:**
   - One class per file
   - File name matches class name
   - Group related classes in folders

3. **Async/Await:**
   - Use async methods for I/O operations
   - Suffix async methods with `Async`
   - Example: `CreateUserAsync()`, `ChangePasswordAsync()`

### Design Patterns Used

1. **Repository Pattern** (Implicit):
   - DataContext acts as repository
   - IDataContext provides abstraction

2. **Factory Pattern**:
   - `DataContextFactory` creates DataContext instances
   - Registered as singleton in Autofac

3. **Service Layer Pattern**:
   - Business logic isolated in Service project
   - ServiceBase provides common functionality

4. **Dependency Injection**:
   - Autofac container
   - Constructor injection
   - InstancePerRequest for web services

5. **Template Method Pattern**:
   - ServiceBase defines structure
   - Derived services implement specifics

6. **OAuth 2.0 Provider Pattern**:
   - ApplicationOAuthProvider handles token generation
   - Implements `IOAuthAuthorizationServerProvider`

### Architectural Principles

1. **Separation of Concerns:**
   - Each layer has distinct responsibility
   - No business logic in controllers
   - No data access in views

2. **Dependency Inversion:**
   - Depend on abstractions (IDataContext, IAuthService)
   - Not on concrete implementations

3. **Single Responsibility:**
   - Controllers handle HTTP concerns
   - Services handle business logic
   - Entities are pure data models

4. **DRY (Don't Repeat Yourself):**
   - ServiceBase for common service functionality
   - ApplicationController for common controller logic
   - EntityBase for common entity properties

---

## Database

### Schema Overview

**ASP.NET Identity Tables:**
- `AspNetUsers` - User accounts
- `AspNetRoles` - Role definitions
- `AspNetUserRoles` - User-Role mappings (junction)
- `AspNetUserClaims` - User claims
- `AspNetUserLogins` - External authentication providers

**Custom Tables:**
- `Accounts` - Chart of accounts (AccountId, Name, ParentId, Level, InitBalance)

**Removed Tables** (by AddAccounts migration):
- `StockCodes` - Removed in final migration
- `Languages` - Removed in final migration

### Entity Framework Configuration

**Decimal Precision:**
```csharp
modelBuilder.Properties<decimal>()
    .Configure(c => c.HasPrecision(18, 3));
```
- All decimal properties use precision (18, 3)
- Reference: `Src/AccountingSystem.Data/DataContext.cs:36`

**Performance Settings:**
```csharp
Configuration.LazyLoadingEnabled = false;
Configuration.ProxyCreationEnabled = false;
```
- Explicit loading required
- No change tracking proxies

### Seeding Data

**Location:** `Src/AccountingSystem.Data/Migrations/Configuration.cs`

Current seeding:
- Stock codes with test data (may be removed)
- Check migration history for active seed data

---

## Authentication & Authorization

### OWIN Configuration

**Location:** `Src/AccountingSystem.Web/App_Start/Startup.Auth.cs`

#### Cookie Authentication

```csharp
UseCookieAuthentication(new CookieAuthenticationOptions
{
    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
    LoginPath = new PathString("/Account/Login"),
    Provider = new CookieAuthenticationProvider
    {
        OnValidateIdentity = SecurityStampValidator
            .OnValidateIdentity<ApplicationUserManager, User>(
                validateInterval: TimeSpan.FromMinutes(30),
                regenerateIdentity: (manager, user) =>
                    user.GenerateUserIdentityAsync(manager))
    }
});
```

#### OAuth Bearer Tokens

```csharp
UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
{
    TokenEndpointPath = new PathString("/api/token"),
    Provider = new ApplicationOAuthProvider(),
    AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
    AllowInsecureHttp = true  // DEBUG only - disable in production
});
```

**Token Endpoint:** `POST /api/token`
**Request Body:**
```
grant_type=password&username={email}&password={password}
```

**Response:**
```json
{
  "access_token": "...",
  "token_type": "bearer",
  "expires_in": 31535999,
  "userName": "user@example.com",
  ".issued": "...",
  ".expires": "..."
}
```

### Authorization Attributes

```csharp
[Authorize]                          // Requires authentication
[Authorize(Roles = "Admin")]         // Requires specific role
[AllowAnonymous]                     // Public access
```

### Security Considerations

⚠️ **Important Security Notes:**

1. **AllowInsecureHttp = true** in OAuth config
   - Only for development
   - MUST be `false` in production
   - Reference: `Src/AccountingSystem.Web/App_Start/Startup.Auth.cs:67`

2. **Token Expiration:**
   - Currently set to 365 days
   - Consider shorter expiration for production (e.g., 1 hour with refresh tokens)

3. **RequireUniqueEmail = false:**
   - Allows duplicate emails
   - Consider enabling for better security
   - Reference: `Src/AccountingSystem.Service/AccountingSystem.service/Identity/UserManager.cs:1`

4. **Password Policy:**
   - Minimum 8 characters
   - No complexity requirements
   - Consider strengthening for production

---

## Testing

### Test Framework: MSTest

**Location:** `/home/user/AccountingSystem/Src/AccountingSystem.Tests/`

### Test Structure

```csharp
[TestClass]
public class HomeControllerTest
{
    [TestMethod]
    public void Index()
    {
        // Arrange
        var controller = new HomeController();

        // Act
        var result = controller.Index() as ViewResult;

        // Assert
        Assert.IsNotNull(result);
    }
}
```

### Current Test Coverage

**Existing Tests:**
- `HomeControllerTest` - Basic controller tests

**Missing Test Coverage:**
- Service layer (UserService, AuthService)
- Data layer (DataContext, repositories)
- Controllers with dependencies (AccountController, UserController)
- Integration tests
- API endpoint tests

### Testing Best Practices

When adding tests:

1. **Arrange-Act-Assert Pattern:**
   ```csharp
   // Arrange - Set up test data
   // Act - Execute the code under test
   // Assert - Verify the results
   ```

2. **Mocking Dependencies:**
   ```csharp
   // Use mocking framework (e.g., Moq)
   var mockService = new Mock<IUserService>();
   mockService.Setup(s => s.GetUserById(It.IsAny<string>()))
              .Returns(new User { ... });
   ```

3. **Test Naming:**
   ```csharp
   [TestMethod]
   public void MethodName_Scenario_ExpectedBehavior()
   {
       // Example: CreateUser_WithValidData_ReturnsSuccess
   }
   ```

4. **Test Categories:**
   ```csharp
   [TestMethod]
   [TestCategory("Unit")]
   [TestCategory("Integration")]
   ```

---

## Common Tasks

### Task 1: Add a New Controller

```csharp
// 1. Create controller class
public class MyController : ApplicationController
{
    public MyController(IAppService appService)
        : base(appService)
    {
    }

    public ActionResult Index()
    {
        return View();
    }
}

// 2. Create view in Views/My/Index.cshtml
// 3. Add route if needed in RouteConfig.cs
```

### Task 2: Add a New API Endpoint

```csharp
// 1. Create ApiController
public class MyApiController : ApiController
{
    [HttpGet]
    [Route("api/myapi/data/{id}")]
    public IHttpActionResult GetData(int id)
    {
        var data = // ... fetch data
        return Ok(data);
    }

    [HttpPost]
    [Route("api/myapi/data")]
    public IHttpActionResult CreateData([FromBody] MyModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // ... create data
        return Created($"api/myapi/data/{model.Id}", model);
    }
}

// 2. Test via: GET /api/myapi/data/123
```

### Task 3: Add a New Entity with Migration

```csharp
// 1. Create entity in AccountingSystem.Entity/
public class Invoice : EntityBase
{
    public int InvoiceId { get; set; }
    public string InvoiceNumber { get; set; }
    public decimal Amount { get; set; }
    public DateTime InvoiceDate { get; set; }
}

// 2. Add DbSet to DataContext.cs
public IDbSet<Invoice> Invoices { get; set; }

// 3. Create migration in Package Manager Console
Add-Migration AddInvoice

// 4. Update database
Update-Database

// 5. Verify migration in Migrations/ folder
```

### Task 4: Add Service with Dependency Injection

```csharp
// 1. Create interface in AccountingSystem.Service/
public interface IInvoiceService
{
    Invoice GetInvoiceById(int id);
    IList<Invoice> GetInvoices();
}

// 2. Implement service
public class InvoiceService : ServiceBase, IInvoiceService
{
    public InvoiceService(IDataContextFactory factory)
        : base(factory)
    {
    }

    public Invoice GetInvoiceById(int id)
    {
        using (var context = DataContext())
        {
            return context.Set<Invoice>()
                          .FirstOrDefault(i => i.InvoiceId == id);
        }
    }
}

// 3. Register in AutofacConfig.cs
builder.RegisterType<InvoiceService>()
       .As<IInvoiceService>()
       .InstancePerRequest();

// 4. Inject in controller
public class InvoiceController : ApplicationController
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(
        IAppService appService,
        IInvoiceService invoiceService)
        : base(appService)
    {
        _invoiceService = invoiceService;
    }
}
```

### Task 5: Add Custom Validation

```csharp
// Entity validation
public class User : IdentityUser, IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            yield return new ValidationResult(
                "First name is required",
                new[] { nameof(FirstName) }
            );
        }
    }
}

// ViewModel validation
public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [CustomValidation(typeof(RegisterViewModel),
                      nameof(ValidateEmail))]
    public string Email { get; set; }

    public static ValidationResult ValidateEmail(
        string email,
        ValidationContext context)
    {
        // Custom validation logic
        if (email.EndsWith("@test.com"))
            return new ValidationResult(
                "Test emails not allowed");

        return ValidationResult.Success;
    }
}
```

### Task 6: Working with OAuth Tokens

```bash
# Get access token
curl -X POST http://localhost:13920/api/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password&username=user@example.com&password=Password123"

# Use token in API request
curl -X GET http://localhost:13920/api/stock?codes=1 \
  -H "Authorization: Bearer {access_token}"
```

### Task 7: Debugging Common Issues

**Issue: Migration fails**
```bash
# Reset database to specific migration
Update-Database -TargetMigration: MigrationName

# Re-run failed migration
Update-Database
```

**Issue: Autofac dependency not resolved**
```csharp
// Verify registration in AutofacConfig.cs
builder.RegisterType<MyService>()
       .As<IMyService>()
       .InstancePerRequest();  // or .SingleInstance()

// Check constructor injection
public MyController(IMyService myService) { ... }
```

**Issue: Cookie/Token authentication not working**
```csharp
// Verify OWIN startup called
// Check Startup.cs contains: OwinStartup attribute
[assembly: OwinStartup(typeof(AccountingSystem.Web.Startup))]

// Verify authentication middleware added
app.UseCookieAuthentication(...);
app.UseOAuthBearerTokens(...);
```

---

## Quick Reference

### File Locations Quick Reference

| Component | Path |
|-----------|------|
| **Solution** | `/home/user/AccountingSystem/AccountingSystem.sln` |
| **Entities** | `/home/user/AccountingSystem/Src/AccountingSystem.Entity/` |
| **Data Context** | `/home/user/AccountingSystem/Src/AccountingSystem.Data/DataContext.cs` |
| **Migrations** | `/home/user/AccountingSystem/Src/AccountingSystem.Data/Migrations/` |
| **Services** | `/home/user/AccountingSystem/Src/AccountingSystem.Service/AccountingSystem.service/` |
| **Controllers** | `/home/user/AccountingSystem/Src/AccountingSystem.Web/Controllers/` |
| **Views** | `/home/user/AccountingSystem/Src/AccountingSystem.Web/Views/` |
| **ViewModels** | `/home/user/AccountingSystem/Src/AccountingSystem.Web/Models/` |
| **Startup Config** | `/home/user/AccountingSystem/Src/AccountingSystem.Web/App_Start/` |
| **Web Config** | `/home/user/AccountingSystem/Src/AccountingSystem.Web/Web.config` |
| **Tests** | `/home/user/AccountingSystem/Src/AccountingSystem.Tests/` |

### Common Commands

```bash
# Build solution
msbuild AccountingSystem.sln

# Restore NuGet packages
nuget restore AccountingSystem.sln

# Entity Framework migrations
Add-Migration MigrationName
Update-Database
Update-Database -TargetMigration: MigrationName

# Run tests
vstest.console AccountingSystem.Tests.dll
```

### Important Configuration Values

| Setting | Value | Location |
|---------|-------|----------|
| **Debug Port** | 13920 | IIS Express |
| **Database** | LocalDB | Web.config |
| **Token Expiration** | 365 days | Startup.Auth.cs:67 |
| **Session Timeout** | 30 minutes | Startup.Auth.cs |
| **Password Min Length** | 8 chars | UserManager.cs |
| **Max Login Attempts** | 10 | UserManager.cs |
| **Lockout Duration** | 10 minutes | UserManager.cs |

---

## Additional Notes for AI Assistants

### When Modifying This Codebase:

1. **Framework Awareness:**
   - This is ASP.NET MVC 5 (.NET Framework 4.5), NOT ASP.NET Core
   - Use System.Web namespaces, not Microsoft.AspNetCore
   - Use packages.config, not .csproj PackageReference

2. **Dependency Injection:**
   - All web services use constructor injection
   - Register new services in `AutofacConfig.cs`
   - Use appropriate lifetime: `InstancePerRequest` for web services, `SingleInstance` for shared resources

3. **Database Changes:**
   - Always create migrations for entity changes
   - Test migrations on development database first
   - Seed data in Configuration.cs Seed method

4. **Security:**
   - Never commit connection strings with production credentials
   - Keep `AllowInsecureHttp = false` in production
   - Validate all user input in ViewModels and Controllers
   - Use parameterized queries (EF handles this)

5. **Testing:**
   - Add tests for new services and controllers
   - Mock dependencies using a mocking framework
   - Follow Arrange-Act-Assert pattern

6. **Code Style:**
   - Follow existing naming conventions
   - Keep controllers thin (delegate to services)
   - Use async/await for I/O operations
   - Add XML documentation for public APIs

### Known Limitations:

1. **Minimal test coverage** - Expand tests when adding features
2. **Weak password policy** - Consider strengthening
3. **Long token expiration** - Consider refresh tokens
4. **No API versioning** - Consider adding for production
5. **Limited error handling** - Add global exception handling
6. **No logging framework** - Consider adding Serilog or NLog
7. **StockCode/Language tables removed** - Check if needed for features

---

## Changelog

| Date | Version | Changes |
|------|---------|---------|
| 2025-12-08 | 1.0 | Initial comprehensive documentation |

---

## Contact & Support

For questions about this codebase, refer to:
- **Solution file:** `AccountingSystem.sln`
- **This guide:** `CLAUDE.md`
- **Git history:** For understanding evolution of features

---

*This document is maintained for AI assistants working on the AccountingSystem codebase. Keep it updated when making significant architectural changes.*
