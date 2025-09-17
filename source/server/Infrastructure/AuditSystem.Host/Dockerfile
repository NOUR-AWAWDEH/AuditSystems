FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /source
COPY ["source/server/Infrastructure/AuditSystem.Host/AuditSystem.Host.csproj", "source/server/Infrastructure/AuditSystem.Host/"]
COPY ["source/server/Core/AuditSystem.Application/AuditSystem.Application.csproj", "source/server/Core/AuditSystem.Application/"]
COPY ["source/server/Core/AuditSystem.Contract/AuditSystem.Contract.csproj", "source/server/Core/AuditSystem.Contract/"]
COPY ["source/server/Core/AuditSystem.Domain/AuditSystem.Domain.csproj", "source/server/Core/AuditSystem.Domain/"]
COPY ["source/server/Infrastructure/AuditSystem.DataAccess/AuditSystem.DataAccess.csproj", "source/server/Infrastructure/AuditSystem.DataAccess/"]
COPY ["source/server/Core/AuditSystem.BusinessLogic/AuditSystem.BusinessLogic.csproj", "source/server/Core/AuditSystem.BusinessLogic/"]
COPY ["source/server/Infrastructure/AuditSystem.Auth/AuditSystem.Auth.csproj", "source/server/Infrastructure/AuditSystem.Auth/"]
RUN dotnet restore "source/server/Infrastructure/AuditSystem.Host/AuditSystem.Host.csproj"
COPY . /source
WORKDIR "/source/source/server/Infrastructure/AuditSystem.Host"
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