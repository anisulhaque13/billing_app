# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj file (ensure correct relative path)
COPY ["SimpleBilling/SimpleBilling.csproj", "SimpleBilling/"]

# Restore dependencies
RUN dotnet restore "SimpleBilling/SimpleBilling.csproj"

# Copy all source code
COPY SimpleBilling/ SimpleBilling/

# Build the application
RUN dotnet build "SimpleBilling/SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/SimpleBilling
RUN dotnet publish "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy the appsettings.json file
COPY ["SimpleBilling/appsettings.json", "./appsettings.json"]

# Copy the published application from the publish stage
COPY --from=publish /app/publish .

# Entrypoint
ENTRYPOINT ["dotnet", "SimpleBilling.dll"]
