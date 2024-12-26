# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081

# Copy the HTTPS certificate
COPY ["../https-dev-cert.pfx", "/app/certs/https-dev-cert.pfx"]

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Switch to root user for build steps
USER root

# Copy and restore the .csproj file
COPY ["SimpleBilling.csproj", "./"]
RUN dotnet restore "./SimpleBilling.csproj"

# Copy the rest of the source code
COPY . ./SimpleBilling

# Build the application
WORKDIR /src/SimpleBilling
RUN dotnet build "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/SimpleBilling
RUN dotnet publish "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configure HTTPS with the certificate
ENV ASPNETCORE_URLS=https://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
ENV CERTIFICATE_PATH=/app/certs/https-dev-cert.pfx
ENV CERTIFICATE_PASSWORD=121

# Switch back to app user for runtime
USER app
ENTRYPOINT ["dotnet", "SimpleBilling.dll"]
