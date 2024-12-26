##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["SimpleBilling/SimpleBilling.csproj", "SimpleBilling/"]
#RUN dotnet restore "./SimpleBilling/SimpleBilling.csproj"
#COPY . .
#WORKDIR "/src/SimpleBilling"
#RUN dotnet build "./SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "SimpleBilling.dll"]
#


############################################Azure############################################################
# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081

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
RUN ls -la
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

# Switch back to app user for runtime
USER app
ENTRYPOINT ["dotnet", "SimpleBilling.dll"]