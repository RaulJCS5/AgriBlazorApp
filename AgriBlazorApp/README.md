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

   ```dockerfile
   # Stage 1: Build the application
   FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
   WORKDIR /src

   # Copy the project files and restore dependencies
   COPY *.csproj ./
   RUN dotnet restore

   # Copy the rest of the application files
   COPY . ./

   # Build the application
   RUN dotnet publish -c Release -o /app/publish

   # Stage 2: Run the application
   FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
   WORKDIR /app
   COPY --from=build /app/publish .

   # Expose the port the app runs on
   EXPOSE 80

   # Run the application
   ENTRYPOINT ["dotnet", "AgriBlazorApp.dll"]
   ```

3. **Build and Run the Docker Image:**
   Follow the steps to build and run the Docker image.

   ```sh
   cd "C:\Users\rs981105\OneDrive - PGA\Desktop\repo\AgriBlazorApp\AgriBlazorApp"
   docker build -t agriblazorapp .
   docker run -d -p 80:80 --name agriblazorapp-container agriblazorapp
   ```

### Summary
- Ensure your development environment supports .NET 8.0.
- Update the `<TargetFramework>` in your 

AgriBlazorApp.csproj

 file to `net8.0`.
- Update your Dockerfile to use the .NET 8.0 SDK and runtime images.
- Build and run the Docker image.