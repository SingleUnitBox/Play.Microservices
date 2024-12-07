FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY out .
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT dotnet Play.Catalog.Service.dll