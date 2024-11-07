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