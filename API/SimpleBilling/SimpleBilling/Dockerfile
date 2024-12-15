# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8085
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the .csproj file (use the correct relative path)
COPY ["SimpleBilling/SimpleBilling.csproj", "SimpleBilling/"]

# Restore dependencies
RUN dotnet restore "./SimpleBilling/SimpleBilling.csproj"

# Copy all project files and build
COPY . .
WORKDIR "/src/SimpleBilling"
RUN dotnet build "./SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "SimpleBilling.dll"]