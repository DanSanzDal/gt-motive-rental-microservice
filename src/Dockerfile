# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solución y proyectos
COPY microservice.sln .
COPY GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj GtMotive.Estimate.Microservice.Host/
COPY GtMotive.Estimate.Microservice.Api/GtMotive.Estimate.Microservice.Api.csproj GtMotive.Estimate.Microservice.Api/
COPY GtMotive.Estimate.Microservice.ApplicationCore/GtMotive.Estimate.Microservice.ApplicationCore.csproj GtMotive.Estimate.Microservice.ApplicationCore/
COPY GtMotive.Estimate.Microservice.Domain/GtMotive.Estimate.Microservice.Domain.csproj GtMotive.Estimate.Microservice.Domain/
COPY GtMotive.Estimate.Microservice.Infrastructure/GtMotive.Estimate.Microservice.Infrastructure.csproj GtMotive.Estimate.Microservice.Infrastructure/

# Restaurar dependencias
RUN dotnet restore

# Copiar todo el código fuente
COPY . .

# Publicar el proyecto Host
WORKDIR /src/GtMotive.Estimate.Microservice.Host
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENV MongoDB__ConnectionString=mongodb://mongo:27017
ENV MongoDB__DatabaseName=EstimateDb
EXPOSE 80
ENTRYPOINT ["dotnet", "GtMotive.Estimate.Microservice.Host.dll"]


