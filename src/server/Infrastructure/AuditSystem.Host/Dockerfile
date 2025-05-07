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
RUN dotnet restore "/src/src/server/Infrastructure/AuditSystem.Host/AuditSystem.Host.csproj"
COPY . /src
WORKDIR "/src/src/server/Infrastructure/AuditSystem.Host"
RUN dotnet build "AuditSystem.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuditSystem.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Create directory for data protection keys
RUN mkdir -p /app/keys && chmod 777 /app/keys

# Copy published files
COPY --from=publish /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Set the entrypoint
ENTRYPOINT ["dotnet", "AuditSystem.Host.dll"]