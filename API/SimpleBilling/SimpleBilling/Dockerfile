# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["SimpleBilling/SimpleBilling.csproj", "SimpleBilling/"]
RUN dotnet restore "SimpleBilling/SimpleBilling.csproj"

# Copy all source code and build the application
COPY SimpleBilling/ SimpleBilling/
RUN dotnet build "SimpleBilling/SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleBilling/SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy appsettings.json
COPY ["SimpleBilling/appsettings.json", "./appsettings.json"]

# Copy the published application
COPY --from=publish /app/publish .

# Entrypoint
ENTRYPOINT ["dotnet", "SimpleBilling.dll"]
