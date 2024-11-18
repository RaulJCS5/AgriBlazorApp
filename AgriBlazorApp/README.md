# Blazor server app created with Docker

This is a good source for building a docker image with VScode and Blazor server!
check the original branch ('original-branch') . to see the initial setup.

---

This is how you would set up Docker and Blazor app with VScode
---

## Dockerfile setup

```Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["YourAppName.csproj", "."]
RUN dotnet restore "YourAppName.csproj"
COPY . .
RUN dotnet build "YourAppName.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YourAppName.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourAppName.dll"]

```

---

## VS Code setup

1. Create a new Blazor app within a Directory (folder) you choose.

```
dotnet new blazorserver -o YourAppName --no-https -f net6.0
```

2. Create a Dockerfile (shown above)
3. in a BASH shell termninal RUN

```
docker build -t tag-name-for-container .
```

-t allows you set a tag name for the container instead of having to reference it with GUID
'.' is needed to build the from within the directory you set up the image in
you can set '.' to any path that points to this image.

4. To launch the app

```
docker run -p 8080:80 your-tag-name 

```

```
docker run -p 8080:80 id-of-container
```

5. in a browser go to <http://localhost:8080/>

---

You can also just build and run these using the Docker Desktop. It will also allow you to launch the app from Docker Desktop to bring up the web browser

### General Docker commands (can run from anywhere)

list images

```
docker images

```

remove images

```
docker rmi id or tag-name
```

list containers

```
docker ps
```

push a container

```
docker push container-name
```

pull a container

```
docker pull container-name
```

To use .NET 8.0 in your Blazor Web App, you need to ensure that your development environment supports .NET 8.0. If you have the .NET 8.0 SDK installed, you can update your project and Dockerfile accordingly.

### Step-by-Step Instructions

1. **Update the Target Framework in Your Project File:**
   Open your

AgriBlazorApp.csproj

 file and ensure the `<TargetFramework>` element is set to `net8.0`.

   ```xml
   <Project Sdk="Microsoft.NET.Sdk.BlazorWebAssembly">

     <PropertyGroup>
       <TargetFramework>net8.0</TargetFramework>
       <Nullable>enable</Nullable>
       <ImplicitUsings>enable</ImplicitUsings>
       <UserSecretsId>aspnet-AgriBlazorApp-e9806664-765e-41e1-9bff-7b6ab6331834</UserSecretsId>
     </PropertyGroup>

     <!-- Other project settings -->

   </Project>
   ```

2. **Ensure the Dockerfile Uses the Correct SDK and Runtime Versions:**
   Update your Dockerfile to use the .NET 8.0 SDK and runtime images.

3. **Build and Run the Docker Image:**
   Follow the steps to build and run the Docker image.

### Summary

- Ensure your development environment supports .NET 8.0.
- Update the `<TargetFramework>` in your

AgriBlazorApp.csproj

 file to `net8.0`.

- Update your Dockerfile to use the .NET 8.0 SDK and runtime images.
- Build and run the Docker image.

To run your Docker container with the correct ports, you need to map the exposed ports in the Dockerfile to the host machine's ports. Here is how you can do it:

### Step-by-Step Instructions

1. **Build the Docker Image:**
   Make sure you are in the directory where your Dockerfile is located. Run the following command to build the Docker image:

   ```sh
   docker build -t agriblazorapp .
   ```

2. **Run the Docker Container:**
   Use the `docker run` command to start the container and map the ports. Since your Dockerfile exposes ports 8080 and 8081, you can map these ports to the host machine's ports.

   ```sh
   docker run -d -p 8080:8080 -p 8081:8081 --name agriblazorapp-container agriblazorapp
   ```

### Explanation

- `docker build -t agriblazorapp .`: This command builds the Docker image and tags it as `agriblazorapp`.
- `docker run -d -p 8080:8080 -p 8081:8081 --name agriblazorapp-container agriblazorapp`: This command runs the Docker container in detached mode (`-d`), maps port 8080 of the container to port 8080 of the host, maps port 8081 of the container to port 8081 of the host, and names the container `agriblazorapp-container`.

This will start your Blazor web app in a Docker container, accessible via `http://localhost:8080` and `http://localhost:8081` on your machine.

# Postgres

## [running Blazor project using ASP.NET Identity and PostgreSQL with NET CLI](https://medium.com/@tomislav.medanovic/scaffolding-and-running-blazor-project-using-asp-net-identity-and-postgresql-with-net-cli-1fdb2a0d9778)

- Install PostgreSQL � remember your user and pass (We will use superuser for this test)
- You can also run pgAdmin to explore your database later on

- - Add following packages to your project

``` shell
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

- Modfify appsettings.json

``` json
{
 "Logging": {
  "LogLevel": {
   "Default": "Information",
   "Microsoft.AspNetCore": "Warning"
  }
 },
 "ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=5432;Database=blazor_test;Username=<user>;Password=<passowrd>"
 }
}
```

- Modify Program.cs to use PgSQL

``` c#
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
 options.UseNpgsql(connectionString));
```

## How to list all databases in PostgreSQL?

How to get the name of the current database from within PostgreSQL?

The function current_database() returns the name of the current database:

``` sql
 SELECT current_database();
```

## [DotNetEnv](https://github.com/tonerdo/dotnet-env)

``` shell

PM> Install-Package DotNetEnv

dotnet add package DotNetEnv
```

``` c#
using DotNetEnv;
DotNetEnv.Env.Load();

Environment.GetEnvironmentVariable("changeme");
```

- [Why do people put the .env into gitignore?](https://stackoverflow.com/questions/43664565/why-do-people-put-the-env-into-gitignore)

## [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows)

- how to manage sensitive data for an ASP.NET Core app on a development machine. Never store passwords or other sensitive data in source code or configuration files. Production secrets shouldn't be used for development or test. Secrets shouldn't be deployed with the app.

# Controller

## XUNIT Tests

To test the `WeatherForecastsController` and the database connection code, you can use unit tests and integration tests. Here are the steps to set up and run these tests:

### Step 1: Set Up Unit Tests for the Controller

1. **Create a Test Project**:

   ```sh
   dotnet new xunit -o AgriBlazorServerApp.Tests
   ```

2. **Add References to the Test Project**:
   - Add a reference to the main project:

     ```sh
     dotnet add AgriBlazorServerApp.Tests/AgriBlazorServerApp.Tests.csproj reference AgriBlazorServerApp/AgriBlazorServerApp.csproj
     ```

   - Add necessary NuGet packages:

     ```sh
     dotnet add AgriBlazorServerApp.Tests/AgriBlazorServerApp.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory
     dotnet add AgriBlazorServerApp.Tests/AgriBlazorServerApp.Tests.csproj package Moq
     dotnet add AgriBlazorServerApp.Tests/AgriBlazorServerApp.Tests.csproj package Microsoft.AspNetCore.Mvc.Testing
     ```

3. **Create a Test Class for the Controller**:
   - Create a new file `WeatherForecastsControllerTests.cs` in the test project:

     ```csharp
     using System.Threading.Tasks;
     using Microsoft.AspNetCore.Mvc;
     using Microsoft.EntityFrameworkCore;
     using Moq;
     using Xunit;
     using AgriBlazorServerApp.Controllers;
     using AgriBlazorServerApp.Data;

     public class WeatherForecastsControllerTests
     {
         private readonly DbContextOptions<AgriBlazorServerAppContext> _options;

         public WeatherForecastsControllerTests()
         {
             _options = new DbContextOptionsBuilder<AgriBlazorServerAppContext>()
                 .UseInMemoryDatabase(databaseName: "TestDatabase")
                 .Options;
         }

         [Fact]
         public async Task Index_ReturnsViewResult_WithAListOfWeatherForecasts()
         {
             // Arrange
             using var context = new AgriBlazorServerAppContext(_options);
             context.WeatherForecast.Add(new WeatherForecast { Id = 1, Date = DateTime.Now, TemperatureC = 25, Summary = "Warm" });
             context.WeatherForecast.Add(new WeatherForecast { Id = 2, Date = DateTime.Now, TemperatureC = 30, Summary = "Hot" });
             context.SaveChanges();

             var controller = new WeatherForecastsController(context);

             // Act
             var result = await controller.Index();

             // Assert
             var viewResult = Assert.IsType<ViewResult>(result);
             var model = Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(viewResult.ViewData.Model);
             Assert.Equal(2, model.Count());
         }

         // Additional tests for Details, Create, Edit, Delete, etc.
     }
     ```

### Step 2: Set Up Integration Tests for the Database Connection

1. **Create a Test Class for the Database Connection**:
   - Create a new file `DatabaseConnectionTests.cs` in the test project:

     ```csharp
     using System;
     using Npgsql;
     using Xunit;

     public class DatabaseConnectionTests
     {
         [Fact]
         public void TestDatabaseConnection_Success()
         {
             // Arrange
             var connectionString = "Host=changeme_dawg;Port=changeme_dawg;Database=changeme_dawg;Username=changeme_dawg;Password=changeme_dawg";

             // Act & Assert
             try
             {
                 using var connection = new NpgsqlConnection(connectionString);
                 connection.Open();
                 Assert.True(connection.State == System.Data.ConnectionState.Open);
             }
             catch (Exception ex)
             {
                 Assert.True(false, $"Database connection failed: {ex.Message}");
             }
         }
     }
     ```

### Step 3: Run the Tests

1. **Run the Tests**:

   ```sh
   dotnet test AgriBlazorServerApp.Tests/AgriBlazorServerApp.Tests.csproj
   ```

By following these steps, you can set up unit tests for the `WeatherForecastsController` and integration tests for the database connection. The unit tests ensure that the controller methods work as expected, while the integration tests verify that the database connection is successful.

dotnet sln AgriBlazorApp/AgriBlazorApp.sln add AgriBlazorServerApp.Tests/AgriBlazorServerApp.Tests.csproj

# Enable migrations

To enable migrations and work with a PostgreSQL database in your Blazor Server app, you need to ensure that Entity Framework Core is properly configured. This involves setting up the `ApplicationDbContext`, configuring the connection string, and adding the necessary services in `Startup.cs`.

### Step-by-Step Guide

1. **Install the necessary NuGet packages**.
2. **Create the database context and entity classes**.
3. **Configure the database context in `Startup.cs`**.
4. **Create and apply migrations**.

### Step 1: Install the Necessary NuGet Packages

### Step 2: Create the Database Context and Entity Classes

Create the `WeatherForecast` entity and the `ApplicationDbContext` class.

**WeatherForecast.cs**:

```csharp
namespace AgriBlazorServer.Data
{
    public class WeatherForecast
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}
```

**ApplicationDbContext.cs**:

```csharp
using Microsoft.EntityFrameworkCore;

namespace AgriBlazorServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
```

### Step 3: Configure the Database Context in `Startup.cs`

Update your `Startup.cs` to configure the `ApplicationDbContext` and use environment variables for the connection string.

**Startup.cs**:

```csharp
using AgriBlazorServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgriBlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            // Register the IWeatherForecastService and its implementation
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();

            // Add MVC services
            services.AddControllers();

            // Register HttpClient with custom handler
            services.AddHttpClient<WeatherForecastApiService>()
                    .ConfigurePrimaryHttpMessageHandler(() => new CustomHttpClientHandler());

            // Configure PostgreSQL connection
            var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? 
                $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
                $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //(...)
        }
    }
}
```

### Step 4: Create and Apply Migrations

1. **Create the Initial Migration**:

- In terminal of the solution(sln)

   ```sh
   dotnet ef migrations add InitialCreate -p AgriBlazorServer -s AgriBlazorServer
   ```

2. **Apply the Migration to the Database**:

   ```sh
   dotnet ef database update -p AgriBlazorServer -s AgriBlazorServer
   ```

### Summary

1. **Install the necessary NuGet packages** for Entity Framework Core and PostgreSQL.
2. **Create the `WeatherForecast` entity** and the `ApplicationDbContext` class.
3. **Configure the database context** in `Startup.cs` using environment variables for the connection string.
4. **Create and apply migrations** to set up the database schema.

By following these steps, you can set up your project to use Entity Framework Core migrations for a PostgreSQL database, ensuring that your database schema is managed and updated automatically.

## ERROR 1

``` shell
Argument error: Host can't be null
An error occurred while accessing the Microsoft.Extensions.Hosting services. Continuing without the application service provider. Error: Database connection failed due to an argument error: Host can't be null
Unable to create a 'DbContext' of type 'RuntimeType'. The exception 'Unable to resolve service for type 'Microsoft.EntityFrameworkCore.DbContextOptions`1[AgriBlazorServer.Data.ApplicationDbContext]' while attempting to activate 'AgriBlazorServer.Data.ApplicationDbContext'.' was thrown while attempting to create an instance. For the different patterns supported at design time, see <https://go.microsoft.com/fwlink/?linkid=851728>
```

``` csharp
            // Configure PostgreSQL connection
            var dbHost = "localhost";
            var dbPort = "5433";
            var dbName = "postgres";
            var dbUser = "postgres";
            var dbPassword = "postgres";

            if (string.IsNullOrEmpty(dbHost) || string.IsNullOrEmpty(dbPort) || string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(dbUser) || string.IsNullOrEmpty(dbPassword))
            {
                throw new InvalidOperationException("Database connection information is missing. Please ensure all required environment variables are set.");
            }

            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";
```

## ERROR 2

``` shell
Build started...
Build succeeded.
Database connection successful.
Unable to create a 'DbContext' of type 'RuntimeType'. The exception 'Method 'get_LockReleaseBehavior' in type 'Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal.NpgsqlHistoryRepository' from assembly 'Npgsql.EntityFrameworkCore.PostgreSQL, Version=8.0.10.0, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7' does not have an implementation.' was thrown while attempting to create an instance. For the different patterns supported at design time, see <https://go.microsoft.com/fwlink/?linkid=851728>
```

The error message indicates that the `dotnet ef` command could not be executed. This is often due to missing or misconfigured Entity Framework Core tools. Here are the steps to ensure that everything is set up correctly:

### Step-by-Step Solution

1. **Install the necessary NuGet packages**.
2. **Ensure the `dotnet-ef` tool is installed**.
3. **Run the migration command**.

### Step 1: Install the Necessary NuGet Packages

Ensure that your project has the necessary NuGet packages installed:

```sh
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

### Step 2: Ensure the `dotnet-ef` Tool is Installed

Install the `dotnet-ef` tool globally if it is not already installed:

```sh
dotnet tool install --global dotnet-ef
```

If it is already installed, you can update it to the latest version:

```sh
dotnet tool update --global dotnet-ef
```

### Step 3: Run the Migration Command

Navigate to the directory containing your solution file (`.sln`) and run the migration command:

```sh
dotnet ef migrations add InitialCreate -p AgriBlazorServer -s AgriBlazorServer
```

``` sh
Build started...
Build succeeded.
Database connection successful.
Done. To undo this action, use 'ef migrations remove'
```

### Additional Configuration

Ensure that your `AgriBlazorServer.csproj` file includes the necessary references for Entity Framework Core:

**AgriBlazorServer.csproj**:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UserSecretsId>aspnet-AgriBlazorApp-e9806664-765e-41e1-9bff-7b6ab6331834</UserSecretsId>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="DotNetEnv" Version="3.1.1" />
    <PackageReference Include="MailKit" Version="4.8.0" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.Facebook" Version="8.0.10" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.Google" Version="8.0.10" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.MicrosoftAccount" Version="8.0.10" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.Twitter" Version="8.0.10" />
    <PackageReference Include="Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" Version="8.0.10" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.10" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.10" />
  </ItemGroup>

  <ItemGroup>
    <Folder Include="wwwroot\css\" />
  </ItemGroup>

</Project>
```

## ERROR 3

``` shell
Unable to create a 'DbContext' of type ''. The exception 'The entity type 'WeatherForecaso.microsoft.com/fwlink/?linkid=851728t' requires a primary key to be defined. If you intended to use a keyless entity type, call 'HasNoKey' in 'OnModelCreating'. For more information on keyless entity types, see httt Productionps://go.microsoft.com/fwlink/?linkid=2141943.' was thrown while attempting to create an instance. For the different patterns supported at design time, see <https://go.microsoft.com/fwlink/?linkid=851728>
```

- The error message indicates that the `WeatherForecast` entity does not have a primary key defined. In Entity Framework Core, every entity must have a primary key. If you intend to use a keyless entity, you need to configure it explicitly.

### Step-by-Step Solution

1. **Ensure the `WeatherForecast` entity has a primary key**.
2. **Update the `ApplicationDbContext` to configure the entity**.

### Step 1: Ensure the `WeatherForecast` Entity Has a Primary Key

Make sure your `WeatherForecast` entity has a primary key defined. Typically, this is done by adding an `Id` property.

**WeatherForecast.cs**:

```csharp
namespace AgriBlazorServer.Data
{
    public class WeatherForecast
    {
        public int Id { get; set; } // Primary key
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}
```

### Step 2: Update the `ApplicationDbContext` to Configure the Entity

Ensure that the `ApplicationDbContext` is correctly configured to include the `WeatherForecast` entity.

**ApplicationDbContext.cs**:

```csharp
using Microsoft.EntityFrameworkCore;

namespace AgriBlazorServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the WeatherForecast entity
            modelBuilder.Entity<WeatherForecast>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.TemperatureC).IsRequired();
                entity.Property(e => e.Summary).HasMaxLength(200);
            });
        }
    }
}
```
