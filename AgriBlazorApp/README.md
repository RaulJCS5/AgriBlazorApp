# Blazor server app created with Docker

This is a good source for building a docker image with VScode and Blazor server!
check the original branch ('original-branch') . to see the initial setup.

---
This is how you would set up Docker and Blazor app with VScode
---

##  Dockerfile setup
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
5. in a browser go to http://localhost:8080/

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

- Install PostgreSQL — remember your user and pass (We will use superuser for this test)
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

-  how to manage sensitive data for an ASP.NET Core app on a development machine. Never store passwords or other sensitive data in source code or configuration files. Production secrets shouldn't be used for development or test. Secrets shouldn't be deployed with the app.

# [How to create a docker-compose setup with PostgreSQL and pgAdmin4](https://www.youtube.com/watch?v=qECVC6t_2mU)

## Overview

This provides a step-by-step guide to setting up a Dockerized ASP.NET Core application with a PostgreSQL database and pgAdmin for database management. It includes troubleshooting steps for common errors encountered during the setup process.

## Prerequisites

- Docker
- Docker Compose

## Configuration

### Docker Compose File

Create a 

docker-compose.yml

 file with the following content:

- this code is for pgAdmin and PostgreSQL

``` yaml
version: "3.8"
services:
  db:
    container_name: postgres_container
    image: postgres
    restart: always
    environment:
      POSTGRES_USER: root
      POSTGRES_PASSWORD: root
      POSTGRES_DB: test_db
    ports:
      - "5434:5432"

  pgadmin:
    container_name: pgadmin4_container
    image: dpage/pgadmin4
    restart: always
    environment:
      PGADMIN_DEFAULT_EMAIL: admin@admin.com
      PGADMIN_DEFAULT_PASSWORD: root
    ports:
      - "5050:80"
```

- and this code is for the Blazor app

``` yaml
  app:
    container_name: agri_blazorapp
    build: .
    ports:
      - "8080:8080"
    depends_on:
      - db
    environment:
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=test_db;Username=root;Password=root
```

- it also can work with environment variables by using the following code instead of using the default connection string

``` yaml
  app:
    container_name: agri_blazorapp
    build: .
    ports:
      - "8080:8080"
    depends_on:
      - db
    environment:
      # - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=test_db;Username=root;Password=root
      - DB_HOST=${DB_HOST:-db}
      - DB_PORT=${DB_PORT:-5432}
      - DB_DATABASE=${DB_DATABASE:-test_db}
      - DB_USERNAME=${DB_USERNAME:-root}
      - DB_PASSWORD=${DB_PASSWORD:-root}
```

## Common Errors and Solutions

### Error: "Database connection failed due to PostgreSQL error: Failed to connect to 127.0.0.1:5432"

**Solution**: Ensure the connection string uses the correct hostname and port. In Docker Compose, use the service name `db` as the hostname and the correct port (5432 this is default one).

### Error: "Database connection failed due to an unexpected error: Name or service not known"

**Solution**: Ensure the hostname in the connection string is `db`, which is the service name defined in 

### Important Note

If you have a `.env` file in your project directory, Docker Compose will automatically load environment variables from it. If the variables in the `.env` file conflict with those defined in the `docker-compose.yml` file, it can cause issues.

**Solution**: Delete or rename the `.env` file if it is causing conflicts.

## Final Steps

1. **Build and Run Containers**:
   ```sh
   docker-compose down
   docker-compose up --build
   ```

2. **Verify PostgreSQL Connection**:
   - Run `docker container ls` to list running containers.
   - Run `docker inspect <postgres_container_id>` to get the IP address of the PostgreSQL container.

3. **Access pgAdmin**:
   - Open a browser and go to `http://localhost:5050`.
   - Use the credentials `admin@admin.com` and `root` to log in.
   - Add a new server in pgAdmin using the following details:
     - **Name**: Any name you prefer (e.g., `whateveruwant`).
     - **Host name/address**: The IP address obtained from the `docker inspect` command ex.(172.18.0.2).
     - **Port**: `5432`
     - **Username**: `root`
     - **Password**: `root`

## Final Result

After following these steps, the application should successfully connect to the PostgreSQL database, and you should be able to manage the database using pgAdmin. The final setup ensures that the application and database services are correctly configured and can communicate with each other within the Docker network.

# [How to add persistent data storage for PostgreSQL in Docker Compose](https://stackoverflow.com/questions/41637505/how-to-persist-data-in-a-dockerized-postgres-database-using-volumes)

To add persistent data storage for the PostgreSQL database, you can define a volume in the docker-compose.yml file. This will ensure that the database data is stored outside of the container and persists even if the container is removed or recreated.

Here is the updated docker-compose.yml with volume persistence:

```yaml
version: "3.8"
services:
  db:
    container_name: postgres_container
    image: postgres
    restart: always
    environment:
      POSTGRES_USER: root
      POSTGRES_PASSWORD: root
      POSTGRES_DB: test_db
    ports:
      - "5434:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

volumes:
  postgres_data:
```

### Explanation

- **Volumes**: The `volumes` section at the bottom of the 

docker-compose.yml

 file defines a named volume called `postgres_data`.
- **Volume Mount**: The `volumes` section under the `db` service mounts the `postgres_data` volume to the `/var/lib/postgresql/data` directory inside the PostgreSQL container. This is where PostgreSQL stores its data files.

## Is it necessary to have the `env_file` in both the `app` and `db`

Yes, it is necessary to have the `env_file` in both the app and `db` services if you want both services to use the environment variables defined in the `postgres.env` file. This ensures that both the application and the PostgreSQL database are configured correctly with the same environment variables.

Here is the updated docker-compose.yml file with the `env_file` specified for both the app and `db` services:

### Updated 

docker-compose.yml

```yaml
version: "3.8"

services:
  db:
    env_file:
      - ./postgres.env

  app:
    env_file:
      - ./postgres.env
```

### `postgres.env` File

Ensure your `postgres.env` file contains the necessary environment variables for PostgreSQL:

```env
DB_HOST=changeme_dawg
DB_PORT=changeme_dawg
DB_DATABASE=changeme_dawg
DB_USERNAME=changeme_dawg
DB_PASSWORD=changeme_dawg
```

# apply changes made in the docker-compose.yml

To apply changes made in the docker-compose.yml file without stopping and deleting the previous run, you can use the `docker-compose up` command with the `--no-deps` and `--build` options. This will rebuild the specified service and apply the changes without affecting the other running services.

Here are the steps to apply the changes:

1. **Rebuild the Service**: Use the `docker-compose up` command with the `--no-deps` and `--build` options to rebuild the specific service you want to update. For example, to rebuild the app

 service:

   ```sh
   docker-compose up --no-deps --build app
   ```

   This command will rebuild the app service and apply the changes without stopping the other services.

2. **Restart the Service**: If you need to restart the service to apply the changes, you can use the `docker-compose restart` command:

   ```sh
   docker-compose restart app
   ```

   This command will restart the app service without affecting the other running services.

### Example Commands

To rebuild and restart the `db` service:

```sh
docker-compose up --no-deps --build db
docker-compose restart db
```
 service:

To rebuild and restart the `app`

```sh
docker-compose up --no-deps --build app
docker-compose restart app
```

### Note

If you have made changes to the environment variables or volumes, you may need to restart the affected services to apply the changes. The `--no-deps` option ensures that the dependent services are not stopped or rebuilt.
