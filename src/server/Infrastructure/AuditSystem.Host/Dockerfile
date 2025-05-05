# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/server/Infrastructure/AuditSystem.Host/AuditSystem.Host.csproj", "src/server/Infrastructure/AuditSystem.Host/"]
COPY ["src/server/Core/AuditSystem.Application/AuditSystem.Application.csproj", "src/server/Core/AuditSystem.Application/"]
COPY ["src/server/Core/AuditSystem.Contract/AuditSystem.Contract.csproj", "src/server/Core/AuditSystem.Contract/"]
COPY ["src/server/Core/AuditSystem.Domain/AuditSystem.Domain.csproj", "src/server/Core/AuditSystem.Domain/"]
COPY ["src/server/Infrastructure/AuditSystem.DataAccess/AuditSystem.DataAccess.csproj", "src/server/Infrastructure/AuditSystem.DataAccess/"]
COPY ["src/server/Core/AuditSystem.BusinessLogic/AuditSystem.BusinessLogic.csproj", "src/server/Core/AuditSystem.BusinessLogic/"]
COPY ["src/server/Infrastructure/AuditSystem.Auth/AuditSystem.Auth.csproj", "src/server/Infrastructure/AuditSystem.Auth/"]
RUN dotnet restore "./src/server/Infrastructure/AuditSystem.Host/AuditSystem.Host.csproj"
COPY . .
WORKDIR "/src/src/server/Infrastructure/AuditSystem.Host"
RUN dotnet build "./AuditSystem.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuditSystem.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuditSystem.Host.dll"]