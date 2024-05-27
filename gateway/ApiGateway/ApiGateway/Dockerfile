# Utiliza la imagen de ASP.NET Core con IIS como base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Utiliza la imagen de SDK de .NET para compilar y publicar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ApiGateway.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app

# Utiliza la imagen base para la aplicación publicada
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
