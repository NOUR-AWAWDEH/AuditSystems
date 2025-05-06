# Base image for the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["src/server/Infrastructure/AuditSystem.Host/AuditSystem.Host.csproj", "src/server/Infrastructure/AuditSystem.Host/"]
COPY ["src/server/Core/AuditSystem.Application/AuditSystem.Application.csproj", "src/server/Core/AuditSystem.Application/"]
COPY ["src/server/Core/AuditSystem.Contract/AuditSystem.Contract.csproj", "src/server/Core/AuditSystem.Contract/"]
COPY ["src/server/Core/AuditSystem.Domain/AuditSystem.Domain.csproj", "src/server/Core/AuditSystem.Domain/"]
COPY ["src/server/Infrastructure/AuditSystem.DataAccess/AuditSystem.DataAccess.csproj", "src/server/Infrastructure/AuditSystem.DataAccess/"]
COPY ["src/server/Core/AuditSystem.BusinessLogic/AuditSystem.BusinessLogic.csproj", "src/server/Core/AuditSystem.BusinessLogic/"]
COPY ["src/server/Infrastructure/AuditSystem.Auth/AuditSystem.Auth.csproj", "src/server/Infrastructure/AuditSystem.Auth/"]

# Restore packages
RUN dotnet restore "src/server/Infrastructure/AuditSystem.Host/AuditSystem.Host.csproj"

# Copy everything else
COPY . .

# Build the project
WORKDIR "/src/src/server/Infrastructure/AuditSystem.Host"
RUN dotnet build "AuditSystem.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuditSystem.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuditSystem.Host.dll"]