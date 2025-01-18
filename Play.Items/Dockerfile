# Step 1: Use the .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Step 2: Copy the solution file to the container
COPY Play.Items.sln ./

# Step 3: Copy the project files for all the projects in the solution
COPY src/Play.Items.Domain/Play.Items.Domain.csproj src/Play.Items.Domain/
COPY src/Play.Items.Application/Play.Items.Application.csproj src/Play.Items.Application/
COPY src/Play.Items.Infra/Play.Items.Infra.csproj src/Play.Items.Infra/
COPY src/Play.Items.Api/Play.Items.Api.csproj src/Play.Items.Api/
COPY src/Play.Items.Contracts/Play.Items.Contracts.csproj src/Play.Items.Contracts/

COPY tests/Play.Items.Tests.Shared/Play.Items.Tests.Shared.csproj tests/Play.Items.Tests.Shared/
COPY tests/Play.Items.Tests.EndToEnd/Play.Items.Tests.EndToEnd.csproj tests/Play.Items.Tests.EndToEnd/
COPY tests/Play.Items.Tests.Integration/Play.Items.Tests.Integration.csproj tests/Play.Items.Tests.Integration/

RUN dotnet nuget locals all --clear

# Step 4: Copy the local NuGet packages directory to the container
COPY ./packages /root/.nuget/packages

# Step 5: Add the local NuGet feed to the .NET restore process
RUN dotnet nuget add source /root/.nuget/packages --name LocalFeed

# Step 6: Restore all dependencies using the solution file
RUN dotnet restore Play.Items.sln

# Step 7: Copy the rest of the source code for all projects (After restoring dependencies)
COPY ./src ./src
COPY ./tests ./tests

# Step 8: Publish the API project (play.items.api) as it is the entry point
WORKDIR /app/src/Play.Items.Api
RUN dotnet publish -c Release -o /app/publish

# Step 9: Use the .NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Step 10: Copy the published files from the build stage
COPY --from=build-env /app/publish ./

# Step 11: Set the entry point for the application (Program.cs in play.items.api)
ENTRYPOINT ["dotnet", "Play.Items.Api.dll"]
