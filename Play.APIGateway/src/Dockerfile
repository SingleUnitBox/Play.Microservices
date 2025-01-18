FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY . .

RUN dotnet nuget locals all --clear
COPY ./packages /root/.nuget/packages
RUN dotnet nuget add source /root/.nuget/packages --name LocalFeed
RUN dotnet restore Play.APIGateway

RUN dotnet publish Play.APIGateway -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "Play.APIGateway.dll"]
