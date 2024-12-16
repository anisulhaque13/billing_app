# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj file
COPY ["SimpleBilling.csproj", "./"]
RUN dotnet restore "SimpleBilling.csproj"

# Copy source code and build
COPY . .
RUN dotnet build "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy the published application from publish stage
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SimpleBilling.dll"]
