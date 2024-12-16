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
WORKDIR /src/SimpleBilling
RUN dotnet restore "SimpleBilling.csproj"

# Copy all source code and build the application
COPY SimpleBilling/ SimpleBilling/
RUN dotnet build "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/SimpleBilling
RUN dotnet publish "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy the published application
COPY --from=publish /app/publish .

# Entrypoint
ENTRYPOINT ["dotnet", "SimpleBilling.dll"]
